using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductTestController : ControllerBase
    {
        private readonly PlantLiefhebbersContext _context;
        
        public ProductTestController(PlantLiefhebbersContext context)
        {
            _context = context;
        }
        
        /// <summary>
        /// Voeg een nieuwe product toe.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct([FromBody] Product product)
        {
            _context.product.Add(product);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProduct), new { id = product.productId }, product);
        }
        

        /// <summary>
        /// Haal alle producten op.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducten()
        {
            return await _context.product.ToListAsync();
        }

        /// <summary>
        /// Haal een specifieke product op.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.product.FindAsync(id);
            if (product == null)
                return NotFound();
            return product;
        }
    }
}
