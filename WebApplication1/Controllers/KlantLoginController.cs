using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KlantLoginController
    {
        private readonly PlantLiefhebbersContext _context;

        public KlantLoginController(PlantLiefhebbersContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Klant>> GetKlant(int id)
        {
            var klant = await _context.klant.FindAsync(id);
            if (klant == null)
                return NotFound();
            return klant;
        }
    }
}
