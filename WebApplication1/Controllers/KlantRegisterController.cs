using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;


namespace WebApplication1.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("[controller]")]
    public class KlantRegisterController : ControllerBase
    {
        private readonly PlantLiefhebbersContext _context;

        public KlantRegisterController(PlantLiefhebbersContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] KlantRegisterDto dto)
        {
            // === Validation ===
            if (string.IsNullOrWhiteSpace(dto.naam))
                return BadRequest("Naam is verplicht.");

            if (string.IsNullOrWhiteSpace(dto.email))
                return BadRequest("Email is verplicht.");

            if (string.IsNullOrWhiteSpace(dto.wachtwoord) || dto.wachtwoord.Length < 6)
                return BadRequest("Wachtwoord moet minstens 6 tekens lang zijn.");
                        
            bool emailBestaat = await _context.klant.AnyAsync(k => k.email == dto.email);
            if (emailBestaat)
                return BadRequest("Email bestaat al.");


            // dto naar model
            var klant = new Klant
            {
                naam = dto.naam,
                adres = dto.adres,
                email = dto.email,
                wachtwoord = dto.wachtwoord // later hashen
            };

            await _context.klant.AddAsync(klant);
            await _context.SaveChangesAsync();


            // model naar dto
            var klantDto = new KlantDto
            {
                klantId = klant.klantId,
                naam = klant.naam,
                email = klant.email
            };

            return Ok(new
            {
                message = "Registratie succesvol!",
                klant = klantDto
            });
        }
    }
}