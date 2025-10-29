using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly PlantLiefhebbersContext _context;

        public LoginController(PlantLiefhebbersContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] Klant newKlant)
        {
            if (newKlant == null ||
                string.IsNullOrWhiteSpace(newKlant.naam) ||
                string.IsNullOrWhiteSpace(newKlant.email) ||
                string.IsNullOrWhiteSpace(newKlant.wachtwoord))
            {
                return BadRequest("Missing required fields.");
            }

            // Optional: prevent duplicate emails
            var existing = _context.klant.FirstOrDefault(k => k.email == newKlant.email);
            if (existing != null)
            {
                return Conflict("Email already registered.");
            }

            _context.klant.Add(newKlant);
            _context.SaveChanges();

            return Ok(new { message = "Registration successful", klant = newKlant });
        }
    }
}
