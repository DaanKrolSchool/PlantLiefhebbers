public class VerkochtProductDto
{
    public int productId { get; set; }
    public string soortPlant { get; set; }
    public string aanvoerderNaam { get; set; }
    public int aantalVerkocht { get; set; }
    public float prijsPerStuk { get; set; }
    public DateOnly datum { get; set; }
}