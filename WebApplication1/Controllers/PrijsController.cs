using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/prijs")]
public class PrijsController : ControllerBase
{
    private readonly Prijs _priceService;

    public PrijsController(Prijs priceService)
    {
        _priceService = priceService;
    }

    [HttpGet("price")]
    public IActionResult GetPrice()
    {
        return Ok(_priceService.GetPrice());
    }
}
