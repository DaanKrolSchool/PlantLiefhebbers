using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InlogController : ControllerBase
    {
        private readonly PlantLiefhebbersContext _context;

        public InlogController(PlantLiefhebbersContext context)
        {
            _context = context;
        }

        [HttpGet("id/{id}")]
        public async Task<ActionResult<Klant>> GetKlantId(int id)
        {
            var klant = await _context.Klanten.FindAsync(id);
            if (klant == null)
                return NotFound();
            return klant;
        }

        [HttpGet("Email/{Email}")]
        public async Task<ActionResult<Klant>> GetKlantEmail(string Email)
        {
            var klant = await _context.Klanten
                .FirstOrDefaultAsync(k => k.Email == Email);

            if (klant == null)
                return NotFound();

            return klant;
        }

    }
}
