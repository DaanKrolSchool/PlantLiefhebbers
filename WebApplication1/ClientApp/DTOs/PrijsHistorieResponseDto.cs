using WebApplication1.ClientApp.DTOs;

public class PrijsHistorieResponseDto
{
    public string soortPlant { get; set; } = "";
    public string aanvoerderNaam { get; set; } = "";

    public float avgAanvoerder { get; set; }
    public List<PrijsPuntDto> last10Aanvoerder { get; set; } = new();

    public float avgAlleAanvoerders { get; set; }
    public List<PrijsPuntDto> last10AlleAanvoerders { get; set; } = new();
}
