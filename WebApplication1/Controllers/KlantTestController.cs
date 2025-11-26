using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KlantTestController : ControllerBase
    {
        private readonly PlantLiefhebbersContext _context;

        public KlantTestController(PlantLiefhebbersContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Haal alle klanten op.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Klant>>> GetKlanten()
        {
            return await _context.Klanten.ToListAsync();
        }

        /// <summary>
        /// Haal een specifieke klant op.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Klant>> GetKlant(int id)
        {
            var klant = await _context.Klanten.FindAsync(id);
            if (klant == null)
                return NotFound();
            return klant;
        }

        /// <summary>
        /// Voeg een nieuwe klant toe.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Klant>> PostKlant([FromBody] Klant klant)
        {
            _context.Klanten.Add(klant);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetKlant), new { id = klant.KlantId }, klant);
        }

        /// <summary>
        /// Pas een bestaande klant aan.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutKlant(int id, [FromBody] Klant klant)
        {
            if (id != klant.KlantId)
                return BadRequest();

            _context.Entry(klant).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Klanten.Any(e => e.KlantId == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        /// <summary>
        /// Verwijder een klant.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKlant(int id)
        {
            var klant = await _context.Klanten.FindAsync(id);
            if (klant == null)
                return NotFound();

            _context.Klanten.Remove(klant);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
