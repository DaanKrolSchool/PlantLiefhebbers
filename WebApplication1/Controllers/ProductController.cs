using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Security.Claims;
using WebApplication1.ClientApp.DTOs;
using Microsoft.Data.SqlClient;


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

        //[HttpGet("id/{id}")]
        //[Authorize]
        //public async Task<ActionResult<PlantPrijsSamenvattingDto>> GetPrijsGeschiedenis(int id)
        //{


        //    var product = await _context.product.FindAsync(id);


        //    var dto = new PlantPrijsSamenvattingDto
        //    {
        //        soortPlant = product.soortPlant,
        //        aanvoerderNaam = product.aanvoerderNaam
        //    };

        //    return Ok(dto);
        //}

        [HttpGet("veilinginfo/{id}")]
        [AllowAnonymous] // of [Authorize] als je wil
        public async Task<ActionResult<ProductVeilingInfoDto>> GetVeilingInfo(int id)
        {

            var dto = await _context.product
                .AsNoTracking()
                .Where(x => x.productId == id && !x.isVerkocht)
                .Select(x => new ProductVeilingInfoDto
                {
                    productId = x.productId,
                    naam = x.naam,
                    soortPlant = x.soortPlant,

                    aantal = x.aantal,
                    potMaat = x.potMaat ?? 0,
                    steelLengte = x.steelLengte ?? 0,

                    makkelijkheid = x.makkelijkheid ?? 0,
                    seizoensplant = x.seizoensplant,
                    temperatuur = x.temperatuur ?? 0,
                    water = x.water ?? 0,
                    leeftijd = x.leeftijd ?? 0,

                    minimumPrijs = x.minimumPrijs,
                    maximumPrijs = x.maximumPrijs ?? 0,
                    prijsVerandering = x.prijsVerandering,

                    veilDatum = x.veilDatum,
                    veilTijd = x.veilTijd,

                    aanvoerderNaam = x.aanvoerderNaam,
                    positie = x.positie
                })
                .FirstOrDefaultAsync();

            if (dto == null) return NotFound();
            return Ok(dto);
        }

        [HttpGet("prijsgeschiedenis/{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<PrijsPuntDto>>> GetPrijsGeschiedenis(int id)
        {
            var product = await _context.product
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.productId == id);

            if (product == null)
                return NotFound();

            var max = product.maximumPrijs ?? product.minimumPrijs;
            var min = product.minimumPrijs;
            var step = product.prijsVerandering <= 0 ? 0.1f : product.prijsVerandering;

            var lijst = new List<PrijsPuntDto>();
            var now = DateTime.Now;

            float current = max;

            for (int i = 0; i < 20; i++)
            {
                lijst.Add(new PrijsPuntDto
                {
                    datum = DateOnly.FromDateTime(now.AddSeconds(-i)),
                    prijs = current
                });

                current -= step;
                if (current < min) break;
            }

            lijst.Reverse();
            return Ok(lijst);
        }

        [HttpGet("historie/soort/{soortPlant}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<PrijsPuntDto>>> GetHistoriePerSoort(string soortPlant)
        {
            var lijst = await _context.productVerkoopHistorie
                .AsNoTracking()
                .Where(x => x.soortPlant == soortPlant)
                .OrderByDescending(x => x.id)
                .Take(20)
                .Select(x => new PrijsPuntDto
                {
                    datum = x.datum,
                    prijs = x.prijsPerStuk
                })
                .ToListAsync();

            return Ok(lijst);
        }



        [HttpPut("tijd/{id}")]
        [Authorize(Roles = "Veilingmeester")]
        public async Task<IActionResult> ZetVeilTijd(int id, [FromBody] ProductTijdDto dto)
        {
            if (id != dto.productId)
                return BadRequest();

            var product = await _context.product.FindAsync(id);
            if (product == null)
                return NotFound();

            // Datum moet al gezet zijn door aanvoerder
            if (product.veilDatum == null)
                return BadRequest("Veildatum is nog niet gezet door de aanvoerder.");

            // Tijd parsen
            if (!TimeSpan.TryParse(dto.veilTijd, out var parsedTijd))
                return BadRequest("Ongeldige tijd. Gebruik HH:mm.");

            product.veilTijd = parsedTijd;

            await _context.SaveChangesAsync();
            return NoContent();
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
                makkelijkheid = newProductDto.makkelijkheid,
                temperatuur = newProductDto.temperatuur,
                water = newProductDto.water,
                leeftijd = newProductDto.leeftijd,
                seizoensplant = newProductDto.seizoensplant,
                minimumPrijs = newProductDto.minimumPrijs,
                maximumPrijs = newProductDto.maximumPrijs,
                klokLocatie = newProductDto.klokLocatie,
                veilDatum = newProductDto.veilDatum,
                aanvoerderId = newProductDto.aanvoerderId,
                aanvoerderNaam = newProductDto.aanvoerderNaam
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
                makkelijkheid = newProduct.makkelijkheid,
                temperatuur = newProductDto.temperatuur,
                water = newProductDto.water,
                leeftijd = newProductDto.leeftijd,
                seizoensplant = newProductDto.seizoensplant,
                minimumPrijs = newProduct.minimumPrijs,
                maximumPrijs = newProduct.maximumPrijs,
                klokLocatie = newProduct.klokLocatie,
                veilDatum = newProduct.veilDatum,
                aanvoerderId = newProduct.aanvoerderId,
                aanvoerderNaam = newProductDto.aanvoerderNaam
            };

            return Ok(productDto);
        }
        
        [HttpGet("datum")]
        [Authorize]
        public IActionResult GetAllProducts()
        {
            var products = _context.product
                .OrderBy(p => p.veilDatum == null || p.veilTijd == null)
                .ThenBy(p => p.veilDatum)
                .ThenBy(p => p.veilTijd)
                .ThenBy(p => p.productId)
                .Select(p => new ProductDto
                {
                    productId = p.productId,
                    naam = p.naam,
                    soortPlant = p.soortPlant,
                    aantal = p.aantal,
                    potMaat = p.potMaat,
                    steelLengte = p.steelLengte,
                    makkelijkheid = p.makkelijkheid,
                    temperatuur = p.temperatuur,
                    water = p.water,
                    leeftijd = p.leeftijd,
                    seizoensplant = p.seizoensplant,
                    minimumPrijs = p.minimumPrijs,
                    prijsVerandering = p.prijsVerandering,
                    maximumPrijs = p.maximumPrijs,
                    klokLocatie = p.klokLocatie,
                    veilDatum = p.veilDatum,
                    veilTijd = p.veilTijd,
                    aanvoerderId = p.aanvoerderId,
                    positie = p.positie
                })
                .ToList();

            return Ok(products);
        }

        [HttpGet("eerste")]
        [AllowAnonymous]
        public async Task<ActionResult<ProductDto>> GetEersteProduct()
        {
            var now = DateTime.Now;

            var product = await _context.product
                .Where(p => p.veilDatum != null && p.veilDatum <= now && !p.isVerkocht)
                .OrderBy(p => p.veilDatum)
                .ThenBy(p => p.productId)
                .FirstOrDefaultAsync();

            if (product == null)
                return Ok(null);

            return Ok(new ProductDto
            {
                productId = product.productId,
                naam = product.naam,
                soortPlant = product.soortPlant,
                aantal = product.aantal,
                potMaat = product.potMaat,
                steelLengte = product.steelLengte,
                makkelijkheid = product.makkelijkheid,
                temperatuur = product.temperatuur,
                water = product.water,
                leeftijd = product.leeftijd,
                seizoensplant = product.seizoensplant,
                minimumPrijs = product.minimumPrijs,
                prijsVerandering = product.prijsVerandering,
                maximumPrijs = product.maximumPrijs,
                klokLocatie = product.klokLocatie,
                veilDatum = product.veilDatum,
                aanvoerderId = product.aanvoerderId,
                positie = product.positie,
                aanvoerderNaam = product.aanvoerderNaam
            });
        }


        [HttpGet("volgende")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<string>>> GetVolgendeNamen()
        {
            var now = DateTime.Now;

            // Als er al een actieve is: pak eerst die, anders pak eerstvolgende toekomstige
            var eerste = await _context.product
                .Where(p => p.veilDatum != null && !p.isVerkocht)
                .OrderBy(p => p.veilDatum)
                .ThenBy(p => p.productId)
                .FirstOrDefaultAsync(p => p.veilDatum <= now);

            DateTime? basisTijd = eerste?.veilDatum;

            var query = _context.product
                .Where(p => p.veilDatum != null);

            // als er actieve is -> alles NA die
            // anders -> gewoon de eerstvolgende aankomende
            if (basisTijd != null)
            {
                query = query.Where(p => p.veilDatum > basisTijd);
            }
            else
            {
                query = query.Where(p => p.veilDatum > now);
            }

            var volgende = await query
                .OrderBy(p => p.veilDatum)
                .ThenBy(p => p.productId)
                .Take(3)
                .Select(p => p.naam)
                .ToListAsync();

            while (volgende.Count < 3)
                volgende.Add("🪴");

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
        


        [HttpPatch("{id}")]
        
        [Authorize]
        public async Task<IActionResult> BuyProduct(int id, [FromQuery] int hoeveelheidKopen, [FromQuery] int price)
        {
            var product = await _context.product.FindAsync(id);
            if (product == null) return NotFound();

            // hoeveelheid -1
            // product.aantal -= 1;

            // bepaal prijs per stuk (wat koper betaalt)
            var prijsPerStuk = product.maximumPrijs ?? product.minimumPrijs;


            // voorkomen dat het onder 0 gaat
            if (product.aantal <= 1)
            {
                product.isVerkocht = true;
                _context.productVerkoopHistorie.Add(new ProductVerkoopHistorie
                {
                    productId = product.productId,
                    soortPlant = product.soortPlant,
                    aanvoerderNaam = product.aanvoerderNaam,
                    aantalVerkocht = hoeveelheidKopen,
                    prijsPerStuk = price,
                    datum = DateOnly.FromDateTime(DateTime.Now)
                });
            }
            else
            {
                // Meer dan 1 → min 1
                if (hoeveelheidKopen <= product.aantal) {
                    product.aantal -= hoeveelheidKopen;
                    _context.productVerkoopHistorie.Add(new ProductVerkoopHistorie
                    {
                        productId = product.productId,
                        soortPlant = product.soortPlant,
                        aanvoerderNaam = product.aanvoerderNaam,
                        aantalVerkocht = hoeveelheidKopen,
                        prijsPerStuk = price,
                        datum = DateOnly.FromDateTime(DateTime.Now)
                    });
                }
                else
                {
                    product.aantal -= product.aantal;
                    _context.productVerkoopHistorie.Add(new ProductVerkoopHistorie
                    {
                        productId = product.productId,
                        soortPlant = product.soortPlant,
                        aanvoerderNaam = product.aanvoerderNaam,
                        aantalVerkocht = product.aantal,
                        prijsPerStuk = price,
                        datum = DateOnly.FromDateTime(DateTime.Now)
                    } );
                }

            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("verkoop/{id}")]
        [Authorize(Roles = "Klant")]
        public async Task<IActionResult> VerkoopProduct(int id)
        {
            var product = await _context.product.FindAsync(id);
            if (product == null) return NotFound();

            if (product.isVerkocht)
                return BadRequest("Dit product is al verkocht.");

            product.isVerkocht = true;
            product.verkoopPrijs = product.maximumPrijs ?? product.minimumPrijs;
            product.verkoopDatum = DateTime.Now;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        //[HttpPut("productIsVerkocht")]
        //[Authorize(Roles = "Klant")]
        //public async Task <IActionResult> ProductIsVerkocht()
        //{
        //    var product = _context.product;
        //    if (product == null) return NotFound(int id);

        //    if (product.isVerkocht)
        //        return BadRequest("Dit product is al verkocht.");
        //    product.isVerkocht = true;

        //    return Ok(new ProductGeschiedenisDto
        //    {
        //        productId = product.productId,
        //        naam = product.naam,
        //        soortPlant = product.soortPlant,
        //        aantal = product.aantal,
        //        veilDatum = product.veilDatum,
        //        isVerkocht = product.isVerkocht,
        //        verkoopPrijs = product.verkoopPrijs
        //    });

        //}


        [HttpGet("verkocht")]
        [Authorize(Roles = "Veilingmeester,Aanvoerder")]
        public IActionResult GetAlleVerkochteProducten()
        {
            var verkocht = _context.product
                .Where(p => p.isVerkocht)
                .OrderByDescending(p => p.verkoopDatum)
                .Select(p => new VerkochtProductDto
                {
                    productId = p.productId,
                    naam = p.naam,
                    verkoopPrijs = p.verkoopPrijs,
                    verkoopDatum = p.verkoopDatum
                })
                .ToList();

            return Ok(verkocht);
        }




        

    }
} 