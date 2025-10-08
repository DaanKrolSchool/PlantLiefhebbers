using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

public class PlantLiefhebbersContext : DbContext
{
    public DbSet<Klant> klant { get; set; }
    public DbSet<Product> product { get; set; }

    public string DbPath { get; }

    public PlantLiefhebbersContext()
    {
        //var folder = Environment.SpecialFolder.LocalApplicationData;
        //var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(Environment.CurrentDirectory, "plantliefebbersontext.db");
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}

public class Klant {
    public int klantId { get; set; }
    public string naam { get; set; }

}

public class Product {
    public int productId { get; set; }
    public string naam { get; set; }
    public string beschrijving { get; set; }
    public decimal prijs { get; set; }
}
