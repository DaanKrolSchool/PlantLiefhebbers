using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("[controller]")]
    public class InlogController : ControllerBase
    {
        private readonly PlantLiefhebbersContext _context;

        public InlogController(PlantLiefhebbersContext context)
        {
            _context = context;
        }

        [HttpGet("id/{id}")]
        public async Task<ActionResult<KlantDto>> GetKlantID(int id)
        {
            var klant = await _context.klant.FindAsync(id);
            if (klant == null)

                return NotFound();


            // model naar dto
            var dto = new KlantDto
            {
                klantId = klant.klantId,
                naam = klant.naam,
                adres = klant.adres,
                email = klant.email
            };

            return dto;
        }

        [HttpGet("test")]
        public IActionResult Test() => Ok("Controller werkt");



        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var klant = await _context.klant
                .FirstOrDefaultAsync(k => k.email == dto.email && k.wachtwoord == dto.wachtwoord);
            if (klant == null)
                return NotFound();
            // model naar dto
            var klantDto = new KlantDto
            {
                klantId = klant.klantId,
                naam = klant.naam,
                adres = klant.adres,
                email = klant.email
            };
            return Ok(klantDto);
        }

        [HttpGet("email/{email}")]
        public async Task<ActionResult<KlantDto>> GetKlantEmail(string email)
        {
            var klant = await _context.klant
                .FirstOrDefaultAsync(k => k.email == email);

            if (klant == null)
                return NotFound();

            // model naar dto
            var dto = new KlantDto
            {
                klantId = klant.klantId,
                naam = klant.naam,
                adres = klant.adres,
                email = klant.email
            };

            return dto;
        }

    }
}
