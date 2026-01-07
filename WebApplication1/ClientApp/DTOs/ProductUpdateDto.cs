public class ProductUpdateDto
{
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
}