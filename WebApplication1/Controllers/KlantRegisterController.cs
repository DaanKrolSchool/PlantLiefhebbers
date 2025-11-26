using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;


namespace WebApplication1.Controllers
{
    [ApiController]
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
            if (string.IsNullOrWhiteSpace(klant.Naam))
                return BadRequest("Naam is verplicht.");

            if (string.IsNullOrWhiteSpace(klant.Email))
                return BadRequest("Email is verplicht.");

            if (string.IsNullOrWhiteSpace(klant.Wachtwoord) || klant.Wachtwoord.Length < 6)
                return BadRequest("Wachtwoord moet minstens 6 tekens lang zijn.");
                        
            bool EmailBestaat = await _context.Klanten.AnyAsync(k => k.Email == klant.Email);
            if (EmailBestaat)
                return BadRequest("Email bestaat al.");

            await _context.Klanten.AddAsync(klant);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Registratie succesvol!",
                KlantId = klant.KlantId,
                klant.Naam,
                klant.Email
            });
        }
    }
}