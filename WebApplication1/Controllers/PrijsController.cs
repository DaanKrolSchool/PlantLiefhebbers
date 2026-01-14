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

    [HttpGet("priceRijnsburg")]
    public IActionResult GetPriceRijnsburg()
    {
        return Ok(_priceService.GetPriceRijnsburg());
    }

    [HttpGet("priceNaaldwijk")]
    public IActionResult GetPriceNaaldwijk()
    {
        return Ok(_priceService.GetPriceNaaldwijk());
    }

    [HttpGet("priceEelde")]
    public IActionResult GetPriceEelde()
    {
        return Ok(_priceService.GetPriceEelde());
    }

    [HttpGet("priceAalsmeer")]
    public IActionResult GetPriceAalsmeer()
    {
        return Ok(_priceService.GetPriceAalsmeer());
    }

}
