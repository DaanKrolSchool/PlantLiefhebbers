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
    }
}