namespace WebApplication1.ClientApp.DTOs
{
    public class ProductVeilingInfoDto
    {
        public int productId { get; set; }
        public string? naam { get; set; }
        public string? soortPlant { get; set; }

        public int aantal { get; set; }
        public int potMaat { get; set; }
        public int steelLengte { get; set; }

        public int makkelijkheid { get; set; }
        public string? seizoensplant { get; set; }
        public int temperatuur { get; set; }
        public int water { get; set; }
        public int leeftijd { get; set; }

        public float minimumPrijs { get; set; }
        public float maximumPrijs { get; set; }
        public float prijsVerandering { get; set; }

        public DateOnly? veilDatum { get; set; }
        public TimeSpan? veilTijd { get; set; }
        public string aanvoerderNaam { get; set; }
        public int positie { get; set; }
    }
}
