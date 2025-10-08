using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Reflection.Metadata;
using Microsoft.OpenApi.Models;

using var db = new PlantLiefhebbersContext();

//// Note: This sample requires the database to be created before running.
//Console.WriteLine($"Database path: {db.DbPath}.");

// Create
Console.WriteLine("Inserting a new blog");
db.Add(new Klant { klantId = 44321, naam = "john" });
await db.SaveChangesAsync();

//// Read
//Console.WriteLine("Querying for a blog");
//var blog = await db.Blogs
//    .OrderBy(b => b.BlogId)
//    .FirstAsync();

//// Update
//Console.WriteLine("Updating the blog and adding a post");
//blog.Url = "";
//blog.Posts.Add(
//    new Post { Title = "Hello World", Content = "I wrote an app using EF Core!" });
//await db.SaveChangesAsync();

//// Delete
//Console.WriteLine("Delete the blog");
//db.Remove(blog);
//await db.SaveChangesAsync();

namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddRouting();

            // Add Swagger services
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" }); });

            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
                });
            }

            app.UseRouting();
            app.UseHttpsRedirection();
            app.MapControllers();
            app.Run();
        }
    }
}
