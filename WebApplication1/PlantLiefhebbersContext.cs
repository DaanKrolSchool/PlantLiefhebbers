using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

public class PlantLiefhebbersContext : IdentityDbContext<User>
{
    public PlantLiefhebbersContext(DbContextOptions<PlantLiefhebbersContext> options)
        : base(options)
    {
    }
    public DbSet<Product> product { get; set; }
    public DbSet<Veiling> veiling { get; set; }
    public DbSet<ProductVerkoopHistorie> productVerkoopHistorie { get; set; }

}

public class User : IdentityUser { }

public class Product {
    public int productId { get; set; }
    public string aanvoerderId { get; set; }
    public User Aanvoerder { get; set; }
    public string naam { get; set; }
    public string soortPlant { get; set; }
    public int aantal { get; set; }
    public int? potMaat { get; set; }
    public int? steelLengte { get; set; }
    public int? makkelijkheid { get; set; }
    public string seizoensplant { get; set; }
    public int? temperatuur { get; set; }
    public int? water { get; set; }
    public int leeftijd { get; set; }
    public float minimumPrijs { get; set; }
    public float? maximumPrijs { get; set; }
    public float? prijsVerandering { get; set; }
    public string klokLocatie { get; set; }
    public DateOnly veilDatum { get; set; }
    public TimeSpan? veilTijd { get; set; }
	public int positie { get; set; }
    public float prijs { get; set; }
    public bool isVerkocht { get; set; } = false;
    public float? verkoopPrijs { get; set; }
    public DateTime? verkoopDatum { get; set; }
}

public class Veiling
{
    public int veilingId { get; set; }
    public float startPrijs { get; set; }
    public string startDatum { get; set; }
    public string klokLocatie { get; set; }
}

public class ProductVerkoopHistorie
{
    public int id { get; set; }
    public int productId { get; set; }
    public Product Product { get; set; }
    public string? klantId { get; set; }
    public User Klant { get; set; }
    public int aantalVerkocht { get; set; }
    public float prijsPerStuk { get; set; }
}