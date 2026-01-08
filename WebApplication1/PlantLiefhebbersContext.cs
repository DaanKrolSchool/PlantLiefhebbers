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

    public DbSet<Klant> klant { get; set; }
    public DbSet<Product> product { get; set; }
    public DbSet<Veiling> veiling { get; set; }
    public DbSet<Aanvoerder> aanvoerder { get; set; }
    public DbSet<Veilingmeester> veilingmeester { get; set; }
    public DbSet<ProductVerkoopHistorie> productVerkoopHistorie { get; set; }

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
    public int? makkelijkheid { get; set; }
    public string? seizoensplant { get; set; }
    public int? temperatuur { get; set; }
    public int? water { get; set; }
    public int? leeftijd { get; set; }

    public float minimumPrijs { get; set; }
    public float? maximumPrijs { get; set; }
    public float prijsVerandering { get; set; }
    public string klokLocatie { get; set; }
    public DateTime? veilDatum { get; set; }
    public TimeSpan? veilTijd { get; set; }
    public int aanvoerderId { get; set; }
	public int positie { get; set; }

    public bool isVerkocht { get; set; } = false;
    public float? verkoopPrijs { get; set; }
    public DateTime? verkoopDatum { get; set; }
    public string aanvoerderNaam { get; set; }

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

public class ProductVerkoopHistorie
{
    public int id { get; set; }

    public int productId { get; set; }
    public string soortPlant { get; set; }
    public string aanvoerderNaam { get; set; }

    public int aantalVerkocht { get; set; }
    public float prijsPerStuk { get; set; }

    public DateOnly datum { get; set; }
}