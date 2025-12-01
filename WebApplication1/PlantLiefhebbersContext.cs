using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

public class PlantLiefhebbersContext : DbContext
{
    public PlantLiefhebbersContext(DbContextOptions<PlantLiefhebbersContext> options)
        : base(options)
    {
        DbPath = System.IO.Path.Join(Environment.CurrentDirectory, "plantliefebbersontext.db");

    }

    public DbSet<Klant> klant { get; set; }
    public DbSet<Product> product { get; set; }
    public DbSet<Veiling> veiling { get; set; }
    public DbSet<Aanvoerder> aanvoerder { get; set; }
    public DbSet<Veilingmeester> veilingmeester { get; set; }
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
    {
        // Only run UseSqlite if no options (like UseInMemoryDatabase) have been passed in.
        if (!options.IsConfigured)
        {
            options.UseSqlite($"Data Source={DbPath}");
        }
    }
}

public class Klant {
    public int klantId { get; set; }
    public string naam { get; set; }
    public string adres { get; set; }
    public string email { get; set; }
    public string wachtwoord { get; set; }

}

public class Product {
    public int productId { get; set; }
    public string naam { get; set; }
    public string soortPlant { get; set; }
    public int aantal { get; set; }
    public int? potMaat { get; set; }
    public int? steelLengte { get; set; }
    public float minimumPrijs { get; set; }
    public float maximumPrijs { get; set; }
    public float prijsVerandering { get; set; }
    public string klokLocatie { get; set; }
    public DateTime veilDatum { get; set; }
    public int aanvoerderId { get; set; }
}

public class Veiling
{
    public int veilingId { get; set; }
    public float startPrijs { get; set; }
    public string startDatum { get; set; }
    public string klokLocatie { get; set; }

}

public class Aanvoerder
{
    public int aanvoerderId { get; set; }
    public string naam { get; set; }
    public string adres { get; set; }
    public string email { get; set; }
    public string wachtwoord { get; set; }

}

public class Veilingmeester
{
    public int veilingmeesterId { get; set; }
    public string naam { get; set; }
    public string adres { get; set; }
    public string email { get; set; }
    public string wachtwoord { get; set; }

}

public class User : IdentityUser
{
    public string? adres { get; set; }

}