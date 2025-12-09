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




//using var db = new PlantLiefhebbersContext();
//Console.WriteLine($"Database path: {db.DbPath}.");

////Create
//var nogeenklant = new Klant { klantId = 06006, naam = "tettesttest", adres = "denhaag", email = "test@email.com", wachtwoord = "0000" };
//db.klant.Add(nogeenklant);
//db.SaveChanges();
//Console.WriteLine("Created: " + nogeenklant.naam);

//// Read
//var klant = db.klant.FirstOrDefault(k => k.klantId == 02006);
//Console.WriteLine("Read: " + klant.naam);

//// Update
//klant.naam = "updatedtesttesttest";
//db.SaveChanges();
//Console.WriteLine("Updated");

//// Delete
//if (klant != null)
//{
//    db.klant.Remove(klant);
//    db.SaveChanges();
//    Console.WriteLine("Deleted: " + klant.naam);
//}

namespace WebApplication1
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddRouting();
            builder.Services.AddScoped<JwtTokenService>();
            builder.Services.AddDbContext<PlantLiefhebbersContext>();

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
            //using (var scope = app.Services.CreateScope())
            //{
            //    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            //    string[] roles = ["Klant", "Aanvoerder", "Veilingmeester", "Admin"];
            //    foreach (var role in roles)
            //    {
            //        if (!(await roleManager.RoleExistsAsync(role)))
            //        {
            //            await roleManager.CreateAsync(new IdentityRole(role));
            //        }
            //    }
            //}

            app.Run();
        }
    }
}


