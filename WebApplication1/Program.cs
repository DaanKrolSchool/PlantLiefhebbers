using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Reflection.Metadata;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using WebApplication1.Services;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WebApplication1
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            var builder = WebApplication.CreateBuilder(args);

            // controller classes en api endpoints
            builder.Services.AddControllers();
            builder.Services.AddRouting();

            // jwt
            builder.Services.AddScoped<JwtTokenService>();

            //db connectie
            builder.Services.AddDbContext<PlantLiefhebbersContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // rollen services
            builder.Services
                .AddIdentityApiEndpoints<User>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<PlantLiefhebbersContext>();

            // JWT authenticatie
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Bearer";
                options.DefaultChallengeScheme = "Bearer";
            })

            // bearer token
            .AddJwtBearer("Bearer", options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                        System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
                    )
                };
            });

            // auth
            builder.Services.AddAuthorization();
            builder.Services.AddTransient<IEmailSender<User>, DummyEmailSender>();

            //cors dingen   
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalDev", policy =>
                {
                    policy.WithOrigins("http://localhost:5173")   // frontend origin (exact!)
                        .AllowCredentials()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });


    // Add Swagger services
    builder.Services.AddEndpointsApiExplorer();
    if (builder.Environment.IsDevelopment())
    {
        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            { 
                Name = "Authorization",
                Description = "Please enter a valid token",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = "Bearer",
                    Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    }
                    },
                        new List<string>()
                    }
                });
            });
        }

            var app = builder.Build();

            // Dit is nodig zodat iedereen zijn eigen data bases heeft
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<PlantLiefhebbersContext>();
                db.Database.Migrate();
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("AllowLocalDev");
            app.UseAuthentication();
            app.UseAuthorization();


            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.MapControllers();
            app.MapIdentityApi<User>();

            app.MapFallbackToFile("index.html");

            //Role seeding
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                string[] roles = ["Klant", "Aanvoerder", "Veilingmeester", "Admin"];
                foreach (var role in roles)
                {
                    if (!(await roleManager.RoleExistsAsync(role)))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }
            }

            app.Run();
        }
    }
}


