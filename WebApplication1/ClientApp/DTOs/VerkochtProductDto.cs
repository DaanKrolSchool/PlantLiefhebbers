public class VerkochtProductDto
{
    public int productId { get; set; }
    public string naam { get; set; }
    public float? verkoopPrijs { get; set; }
    public DateTime? verkoopDatum { get; set; }
    public Boolean isVerkocht { get; set; }
}