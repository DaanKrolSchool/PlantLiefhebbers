public class ProductCreateDto
{
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
    public string klokLocatie { get; set; }
    public DateTime veilDatum { get; set; }
    public int aanvoerderId { get; set; }
}
