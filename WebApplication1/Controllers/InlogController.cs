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
        public async Task<ActionResult<Klant>> GetKlantID(int id)
        {
            var klant = await _context.klant.FindAsync(id);
            if (klant == null)
                return NotFound();
            return klant;
        }

        [HttpGet("email/{email}")]
        public async Task<ActionResult<Klant>> GetKlantEmail(string email)
        {
            var klant = await _context.klant
                .FirstOrDefaultAsync(k => k.email == email);

            if (klant == null)
                return NotFound();

            return klant;
        }

    }
}
