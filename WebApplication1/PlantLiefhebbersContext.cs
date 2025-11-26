using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

public class PlantLiefhebbersContext : DbContext
{
    public DbSet<Klok> Klokken { get; set; } = null!;
    public DbSet<Klant> Klanten { get; set; } = null!;
    public DbSet<Veiling> Veilingen { get; set; } = null!;
    public DbSet<Product> Producten { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;

    //public PlantLiefhebbersContext()
    //{
    //    //var folder = Environment.SpecialFolder.LocalApplicationData;
    //    //var path = Environment.GetFolderPath(folder);
    //    DbPath = System.IO.Path.Join(Environment.CurrentDirectory, "plantliefebbersontext.db");
    //}
    
    // protected override void OnConfiguring(DbContextOptionsBuilder options)
    //    => options.UseSqlite($"Data Source={DbPath}");
    
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlServer(
            "Server=localhost,1433;Database=PlantLiefhebbers;User Id=sa;Password=iDTyjZx7dRL4;TrustServerCertificate=True;");
    }
}

public class Klok
{
    public int KlokId { get; set; }
    public string Locatie { get; set; } = null!;
    public string? Adres { get; set; }
}

public class Klant
{
    public int KlantId { get; set; }
    public string Naam { get; set; } = null!;
    public string Adres { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Wachtwoord { get; set; } = null!;
    public string Rol { get; set; } = null!;
    
    public int LocatieId { get; set; }
    public Klok Locatie { get; set; } = null!;
}

public class Veiling
{
    public int VeilingId { get; set; }
    public DateTime VeilDatum { get; set; }

    public int LocatieId { get; set; }
    public Klok Locatie { get; set; } = null!;
}

public class Product
{
    public int ProductId { get; set; }
    public string Naam { get; set; } = null!;
    public string Soort { get; set; } = null!;
    public int Aantal { get; set; }
    public int Afmeting { get; set; }
    public string SoortAfmeting { get; set; } = null!;
    public decimal MinimumPrijs { get; set; }
    public decimal? PrijsVerandering { get; set; }
    public decimal? MaximumPrijs { get; set; }
    public DateTime? VeilDatum { get; set; }

    public int LocatieId { get; set; }
    public Klok Locatie { get; set; } = null!;

    public int AanvoerderId { get; set; }
    public Klant Aanvoerder { get; set; } = null!;
}

public class Order
{
    public int OrderId { get; set; }

    public int KoperId { get; set; }
    public Klant Koper { get; set; } = null!;

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
}