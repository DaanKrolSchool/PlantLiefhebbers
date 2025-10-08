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
        /// Voeg een nieuwe klant toe aan de database.
        /// </summary>
        /// <param name="klant">De klant die toegevoegd moet worden.</param>
        /// <returns>De toegevoegde klant.</returns>
        [HttpPost]
        public async Task<ActionResult<Klant>> PostKlant([FromBody] Klant klant)
        {
            _context.klant.Add(klant);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(PostKlant), new { id = klant.klantId }, klant);
        }
    }
}
