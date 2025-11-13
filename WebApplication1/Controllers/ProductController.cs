using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly PlantLiefhebbersContext _context;

        public ProductController(PlantLiefhebbersContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult AddProduct([FromBody] Product newProduct)
        {
            _context.product.Add(newProduct);
            _context.SaveChanges();
            return Ok(newProduct);
        }
        
        [HttpGet("datum")]
        public IActionResult GetAllProducts()
        {
            var products = _context.product
                .OrderBy(p => p.veilDatum)
                .ToList();
            return Ok(products);
        }

        [HttpGet("eerste")]
        public async Task<ActionResult<Product>> GetEersteProduct()
        {
            var product = await _context.product
                .OrderBy(p => p.productId)
                .FirstOrDefaultAsync();

            if (product == null)
                return NotFound();

            return product;
        }
    }
}