using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Reflection.Metadata;
using Microsoft.OpenApi.Models;

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
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddRouting();
            builder.Services.AddDbContext<PlantLiefhebbersContext>();

            //cors dingen
            builder.Services.AddCors(options =>
            {
               options.AddPolicy("AllowLocalDev",
                   policy => policy.AllowAnyOrigin()
                                   .AllowAnyMethod()
                                   .AllowAnyHeader());
            });

            
           // Add Swagger services
           //builder.Services.AddEndpointsApiExplorer();
           //builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" }); });

           var app = builder.Build();
            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            //    app.UseSwagger();
            //    app.UseSwaggerUI(c =>
            //    {
            //        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
            //    });
            //}

            app.UseRouting();

            //html dingen
            app.UseDefaultFiles();    
            app.UseStaticFiles();    
            app.MapControllers();
            app.MapFallbackToFile("index.html");

            
            app.UseCors("AllowLocalDev");
            app.UseHttpsRedirection();
            app.MapControllers();
            app.Run();
        }
    }
}


