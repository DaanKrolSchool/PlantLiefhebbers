using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers
{
    [ApiController]
    //[Authorize (Roles = "Klant, Aanvoerder, Admin, Veilingmeester")]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly PlantLiefhebbersContext _context;

        public ProductController(PlantLiefhebbersContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult AddProduct([FromBody] ProductCreateDto newProductDto)
        {
            var newProduct = new Product
            {
                naam = newProductDto.naam,
                soortPlant = newProductDto.soortPlant,
                aantal = newProductDto.aantal,
                potMaat = newProductDto.potMaat,
                steelLengte = newProductDto.steelLengte,
                minimumPrijs = newProductDto.minimumPrijs,
                maximumPrijs = newProductDto.maximumPrijs,
                klokLocatie = newProductDto.klokLocatie,
                veilDatum = newProductDto.veilDatum,
                aanvoerderId = newProductDto.aanvoerderId
            };

            _context.product.Add(newProduct);
            _context.SaveChanges();

            var productDto = new ProductDto
            {
                productId = newProduct.productId,
                naam = newProduct.naam,
                soortPlant = newProduct.soortPlant,
                aantal = newProduct.aantal,
                potMaat = newProduct.potMaat,
                steelLengte = newProduct.steelLengte,
                minimumPrijs = newProduct.minimumPrijs,
                maximumPrijs = newProduct.maximumPrijs,
                klokLocatie = newProduct.klokLocatie,
                veilDatum = newProduct.veilDatum,
                aanvoerderId = newProduct.aanvoerderId
            };

            return Ok(productDto);
        }
        
        [HttpGet("datum")]
        public IActionResult GetAllProducts()
        {
            var products = _context.product
                .OrderBy(p => p.veilDatum)
                .Select(p => new ProductDto
                {
                    productId = p.productId,
                    naam = p.naam,
                    soortPlant = p.soortPlant,
                    aantal = p.aantal,
                    potMaat = p.potMaat,
                    steelLengte = p.steelLengte,
                    minimumPrijs = p.minimumPrijs,
                    maximumPrijs = p.maximumPrijs,
                    klokLocatie = p.klokLocatie,
                    veilDatum = p.veilDatum,
                    aanvoerderId = p.aanvoerderId
                })
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

            var productDto = new ProductDto
            {
                productId = product.productId,
                naam = product.naam,
                soortPlant = product.soortPlant,
                aantal = product.aantal,
                potMaat = product.potMaat,
                steelLengte = product.steelLengte,
                minimumPrijs = product.minimumPrijs,
                maximumPrijs = product.maximumPrijs,
                klokLocatie = product.klokLocatie,
                veilDatum = product.veilDatum,
                aanvoerderId = product.aanvoerderId
            };

            return Ok(productDto);
        }
    }
}