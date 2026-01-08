namespace WebApplication1.ClientApp.DTOs
{
    public class ProductGeschiedenisDto
    {
        public DateTime? veilDatum { get; set; }
        public float? verkoopPrijs { get; set; }
        public bool isVerkocht { get; set; }
        public string soortPlant { get; set; }
        public string naam { get; set; }
        public int productId { get; set; }
        public int aantal { get; set; }


    }
}
