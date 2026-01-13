public class ProductVerkoopDto
{
    public int productId { get; set; }
    public string productNaam { get; set; }
    public string aanvoerderNaam { get; set; }
    public string klantNaam { get; set; }
    public DateOnly? veilDatum { get; set; }
    public int aantalVerkocht { get; set; }
    public float prijsPerStuk { get; set; }
    public float totaalPrijs => aantalVerkocht * prijsPerStuk;
}