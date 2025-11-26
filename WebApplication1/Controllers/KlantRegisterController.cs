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
        public async Task<IActionResult> Register([FromBody] Klant klant)
        {
            // === Validation ===
            if (string.IsNullOrWhiteSpace(klant.naam))
                return BadRequest("Naam is verplicht.");

            if (string.IsNullOrWhiteSpace(klant.email))
                return BadRequest("Email is verplicht.");

            if (string.IsNullOrWhiteSpace(klant.wachtwoord) || klant.wachtwoord.Length < 6)
                return BadRequest("Wachtwoord moet minstens 6 tekens lang zijn.");
                        
            bool emailBestaat = await _context.klant.AnyAsync(k => k.email == klant.email);
            if (emailBestaat)
                return BadRequest("Email bestaat al.");

            await _context.klant.AddAsync(klant);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Registratie succesvol!",
                klantId = klant.klantId,
                klant.naam,
                klant.email
            });
        }
    }
}