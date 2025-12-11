using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Aanvoerder")]
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
        [Authorize]
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
                    prijsVerandering = p.prijsVerandering,
                    maximumPrijs = p.maximumPrijs,
                    klokLocatie = p.klokLocatie,
                    veilDatum = p.veilDatum,
                    aanvoerderId = p.aanvoerderId,
                    positie = p.positie
                })
                .ToList();

            return Ok(products);
        }

        [HttpGet("eerste")]
        [AllowAnonymous]
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
                prijsVerandering = product.prijsVerandering,
                maximumPrijs = product.maximumPrijs,
                klokLocatie = product.klokLocatie,
                veilDatum = product.veilDatum,
                aanvoerderId = product.aanvoerderId
            };

            return Ok(productDto);
        }


        [HttpGet("volgende")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<string>>> GetVolgendeNamen()
        {
            // Haal het eerste product op
            var eerste = await _context.product
                .OrderBy(p => p.productId)
                .FirstOrDefaultAsync();

            if (eerste == null)
                return Ok(new List<string> { "�", "�", "�" });

            // Haal de 3 producten NA het eerste op
            var volgende = await _context.product
                .Where(p => p.productId > eerste.productId)
                .OrderBy(p => p.productId)
                .Take(3)
                .Select(p => p.naam)
                .ToListAsync();

            // Vul aan met "�" tot we er 3 hebben
            while (volgende.Count < 3)
            {
                volgende.Add("�");
            }

            return Ok(volgende);
        }


        [HttpPut("aanvoerder/{id}")]
        [Authorize(Roles = "Aanvoerder")]
        public async Task<IActionResult> PutProduct(int id, [FromBody] ProductUpdateDto productUpdateDto)
        {
            if (id != productUpdateDto.productId) return BadRequest();

            var product = await _context.product.FindAsync(id);
            if (product == null) return NotFound();

            product.naam = productUpdateDto.naam;
            product.soortPlant = productUpdateDto.soortPlant;
            product.aantal = productUpdateDto.aantal;
            product.minimumPrijs = productUpdateDto.minimumPrijs;

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.product.Any(e => e.productId == id)) return NotFound();
                else throw;
            }

            return NoContent();
        }
        
        [HttpPut("veilingMeester/{id}")]
        [Authorize(Roles = "Veilingmeester")]
        public async Task<IActionResult> PutProduct(int id, [FromBody] ProductUpdateVMDto productUpdateVMDto)
        {
            if (id != productUpdateVMDto.productId) return BadRequest();

            var product = await _context.product.FindAsync(id);
            if (product == null) return NotFound();
            
            product.prijsVerandering = productUpdateVMDto.prijsVerandering;
            product.maximumPrijs = productUpdateVMDto.maximumPrijs;

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.product.Any(e => e.productId == id)) return NotFound();
                else throw;
            }

            return NoContent();
        }
        
        [HttpPut("positie/{id}")]
        [Authorize(Roles = "Veilingmeester")]
        public async Task<IActionResult> PutProduct(int id, [FromBody] ProductPositieDto productPositieDto)
        {
            if (id != productPositieDto.productId) return BadRequest();

            var product = await _context.product.FindAsync(id);
            if (product == null) return NotFound();

            product.positie = productPositieDto.positie;

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.product.Any(e => e.productId == id)) return NotFound();
                else throw;
            }

            return NoContent();
        }
        


        [HttpDelete("{id}")]
        //[Authorize(Roles = "Aanvoerder")]
        [Authorize] // TIJDELIJKE AANPASSING VOOR VEILING (nu worden de producten verwijderd ipv gekoppeld aan een klant die heeft gekocht)
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.product.FindAsync(id);
            if (product == null) return NotFound();

            _context.product.Remove(product);
            await _context.SaveChangesAsync();
            return NoContent();
        }





    }
}