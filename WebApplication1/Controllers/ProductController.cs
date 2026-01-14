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

        /*[HttpGet("id/{id}")]
        [Authorize]
        public async Task<ActionResult<PlantPrijsSamenvattingDto>> GetPrijsGeschiedenis(int id)
        {


            var product = await _context.product.FindAsync(id);


            var dto = new PlantPrijsSamenvattingDto
            {
                soortPlant = Product.soortPlant,
                aanvoerderNaam = x.Product.Aanvoerder.UserName
            };

            return Ok(dto);
        }*/

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

                    aanvoerderNaam = x.Aanvoerder.UserName,
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

        /*[HttpGet("historie/soort/{soortPlant}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<PrijsPuntDto>>> GetHistoriePerSoort(string soortPlant)
        {
            var lijst = await _context.productVerkoopHistorie
                .Include(x => x.Product)
                .AsNoTracking()
                .Where(x => x.Product.soortPlant == soortPlant)
                .OrderByDescending(x => x.id)
                .Take(20)
                .Select(x => new PrijsPuntDto
                {
                    datum = x.Product.veilDatum.Value,
                    prijs = x.prijsPerStuk,
                    aanvoerderNaam = x.Product.Aanvoerder.UserName
                })
                .ToListAsync();

            return Ok(lijst);
        }*/



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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

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
                aanvoerderId = userId
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
                aanvoerderId = newProduct.aanvoerderId
            };

            return Ok(productDto);
        }
        
        [HttpPost("UploadImage")]
        [Authorize(Roles = "Aanvoerder")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadImage([FromForm] UploadImageRequest request)
        {
            if (request.Image == null || request.Image.Length == 0)
                return BadRequest("No file uploaded");

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var filePath = Path.Combine(uploadsFolder, $"{request.ProductId}{Path.GetExtension(request.Image.FileName)}");

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await request.Image.CopyToAsync(stream);
            }

            return Ok(new { message = "Image uploaded successfully", path = $"/images/{request.ProductId}{Path.GetExtension(request.Image.FileName)}" });
        }
        
        [HttpGet("aanvoerder/own")]
        [Authorize(Roles = "Aanvoerder")]
        public IActionResult GetOwnProducts()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            var products = _context.product
                .Where(p => p.aanvoerderId == userId && !p.isVerkocht)
                .OrderBy(p => p.veilDatum)
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
                    aanvoerderId = p.aanvoerderId,
                    positie = p.positie
                })
                .ToList();

            return Ok(products);
        }
        
        [HttpGet("veilingmeester/all")]
        [Authorize(Roles = "Veilingmeester")]
        public IActionResult GetAllProducts()
        {
            var products = _context.product
                .Where(p => !p.isVerkocht)
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
                    aanvoerderNaam = p.Aanvoerder.UserName,
                    positie = p.positie
                })
                .ToList();

            return Ok(products);
        }

        [HttpGet("klant/eerste/{kloklocatie}")]
        [AllowAnonymous]
        public async Task<ActionResult<ProductDto>> GetEersteProduct(string kloklocatie)
        {
            var now = DateOnly.FromDateTime(DateTime.Now);

            var product = await _context.product
                .Include(p => p.Aanvoerder)
                .Where(p => p.veilDatum != null && p.veilDatum <= now && !p.isVerkocht && p.klokLocatie == kloklocatie)
                .OrderBy(p => p.veilDatum)
                .ThenBy(p => p.productId)
                .FirstOrDefaultAsync();

            if (product == null)
                return Ok(null);

            var prijsService = HttpContext.RequestServices.GetService<Prijs>();
            if (prijsService != null)
            {
                prijsService.SetProduct(product);
            }

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
                aanvoerderNaam = product.Aanvoerder.UserName
            });
        }


        [HttpGet("klant/volgende/{kloklocatie}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<string>>> GetVolgendeNamen(string kloklocatie)
        {
            var now = DateOnly.FromDateTime(DateTime.Now);

            // Als er al een actieve is: pak eerst die, anders pak eerstvolgende toekomstige
            var eerste = await _context.product
                .Where(p => p.veilDatum != null && !p.isVerkocht && p.klokLocatie == kloklocatie)
                .OrderBy(p => p.veilDatum)
                .ThenBy(p => p.productId)
                .FirstOrDefaultAsync(p => p.veilDatum <= now);

            DateOnly? basisTijd = eerste?.veilDatum;

            var query = _context.product
                .Where(p => p.veilDatum != null);

            // als er actieve is -> alles NA die
            // anders -> gewoon de eerstvolgende aankomende
            if (basisTijd != null)
            {
                query = query.Where(p => p.veilDatum > basisTijd && p.klokLocatie == kloklocatie);
            }
            else
            {
                query = query.Where(p => p.veilDatum > now && p.klokLocatie == kloklocatie) ;
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
                    aantalVerkocht = hoeveelheidKopen,
                    prijsPerStuk = price
                });
            }
            else
            {
                // Meer dan 1 → min 1
                if (hoeveelheidKopen < product.aantal) {
                    product.aantal -= hoeveelheidKopen;
                    _context.productVerkoopHistorie.Add(new ProductVerkoopHistorie
                    {
                        productId = product.productId,
                        aantalVerkocht = hoeveelheidKopen,
                        prijsPerStuk = price
                    });
                }
                else
                {
                    product.aantal -= product.aantal;
                    product.isVerkocht = true;
                    _context.productVerkoopHistorie.Add(new ProductVerkoopHistorie
                    {
                        productId = product.productId,
                        aantalVerkocht = product.aantal,
                        prijsPerStuk = price
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
        [Authorize(Roles = "Aanvoerder")]
        public IActionResult GetEigenVerkochteProducten()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var verkocht = _context.productVerkoopHistorie
                .Include(v => v.Product)
                .Where(v => v.Product.aanvoerderId == userId)
                .OrderByDescending(v => v.id)
                .Select(v => new VerkochtProductDto
                {
                    productId = v.productId,
                    soortPlant = v.Product.soortPlant,
                    klantNaam = v.Klant.UserName,
                    aantalVerkocht = v.aantalVerkocht,
                    prijsPerStuk = v.prijsPerStuk,
                    datum = v.Product.veilDatum.Value
                })
                .ToList();

            return Ok(verkocht);
        }

        [HttpGet("alleverkocht")]
        [Authorize(Roles = "Veilingmeester")]
        public IActionResult GetAlleVerkochteProducten()
        {
            var verkocht = _context.productVerkoopHistorie
                .Where(v => v.aantalVerkocht > 0)
                .OrderByDescending(v => v.id)
                .Select(v => new VerkochtProductDto
                {
                    productId = v.productId,
                    soortPlant = v.Product.soortPlant,
                    aanvoerderNaam = v.Product.Aanvoerder.UserName,
                    klantNaam = v.Klant.UserName,
                    aantalVerkocht = v.aantalVerkocht,
                    prijsPerStuk = v.prijsPerStuk,
                    datum = v.Product.veilDatum.Value
                })
                .ToList();

            return Ok(verkocht);
        }

        [HttpGet("historie/product/{productId}")]
        [AllowAnonymous]
        public async Task<ActionResult<PrijsHistorieResponseDto>> GetPrijsHistorieVoorPopup(int productId)
        {
            await using var connectt = new SqlConnection(_context.Database.GetConnectionString());
            await connectt.OpenAsync();

            // als eerste moet de informatie opgehaald worden soortPlant + aanvoerderId + aanvoerderNaam
            string soortPlant;
            string aanvoerderId;
            string aanvoerderNaam;

            await using (var cmd = new SqlCommand(@"
        SELECT p.soortPlant,
               p.aanvoerderId,
               u.UserName AS aanvoerderNaam
        FROM product p
        LEFT JOIN AspNetUsers u ON u.Id = p.aanvoerderId
        WHERE p.productId = @productId
    ", connectt))
            {
                cmd.Parameters.AddWithValue("@productId", productId);

                await using var r = await cmd.ExecuteReaderAsync();
                if (!await r.ReadAsync())
                    return NotFound();

                soortPlant = r.GetString(0);
                aanvoerderId = r.GetString(1);
                aanvoerderNaam = r.IsDBNull(2) ? "—" : r.GetString(2);
            }

            // De laatste 10 uit de lijst moet worden opgehaald
            async Task<List<PrijsPuntDto>> ReadLast10Async(SqlCommand cmd2)
            {
                var list = new List<PrijsPuntDto>();
                await using var r2 = await cmd2.ExecuteReaderAsync();
                while (await r2.ReadAsync())
                {
                    // gebruik p.veilDatum (DateOnly) van Product
                    var d = r2.GetDateTime(0);
                    var prijs = r2.GetFloat(1);
                    var user = r2.IsDBNull(2) ? "—" : r2.GetString(2);

                    list.Add(new PrijsPuntDto
                    {
                        datum = DateOnly.FromDateTime(d),
                        prijs = prijs,
                        aanvoerderNaam = user
                    });
                }
                return list;
            }

            // dan nu de Laatste 10 van deze aanvoerder (voor dezelfde plantten soorten )
            List<PrijsPuntDto> last10Aanvoerder;
            await using (var cmd = new SqlCommand(@"
        SELECT TOP 10 
               CAST(p.veilDatum AS datetime2) AS datum,
               h.prijsPerStuk,
               u.UserName
        FROM productVerkoopHistorie h
        JOIN product p ON p.productId = h.productId
        LEFT JOIN AspNetUsers u ON u.Id = p.aanvoerderId
        WHERE h.aantalVerkocht > 0
          AND p.soortPlant = @soortPlant
          AND p.aanvoerderId = @aanvoerderId
        ORDER BY h.id DESC
    ", connectt))
            {
                cmd.Parameters.AddWithValue("@soortPlant", soortPlant);
                cmd.Parameters.AddWithValue("@aanvoerderId", aanvoerderId);
                last10Aanvoerder = await ReadLast10Async(cmd);
            }

            // het gemiddlede uitrekenen van deze aanvoerder van dezelfde plantensoorten
            float avgAanvoerder = 0;
            await using (var cmd = new SqlCommand(@"
        SELECT AVG(CAST(h.prijsPerStuk AS float))
        FROM productVerkoopHistorie h
        JOIN product p ON p.productId = h.productId
        WHERE h.aantalVerkocht > 0
          AND p.soortPlant = @soortPlant
          AND p.aanvoerderId = @aanvoerderId
    ", connectt))
            {
                cmd.Parameters.AddWithValue("@soortPlant", soortPlant);
                cmd.Parameters.AddWithValue("@aanvoerderId", aanvoerderId);

                var obj = await cmd.ExecuteScalarAsync();
                avgAanvoerder = (obj == null || obj == DBNull.Value) ? 0 : Convert.ToSingle(obj);
            }

            // de laatste 10 verkopen laten zien van alle aanvoerders 
            List<PrijsPuntDto> last10Alle;
            await using (var cmd = new SqlCommand(@"
        SELECT TOP 10
               CAST(p.veilDatum AS datetime2) AS datum,
               h.prijsPerStuk,
               u.UserName
        FROM productVerkoopHistorie h
        JOIN product p ON p.productId = h.productId
        LEFT JOIN AspNetUsers u ON u.Id = p.aanvoerderId
        WHERE h.aantalVerkocht > 0
          AND p.soortPlant = @soortPlant
        ORDER BY h.id DESC
    ", connectt))
            {
                cmd.Parameters.AddWithValue("@soortPlant", soortPlant);
                last10Alle = await ReadLast10Async(cmd);
            }

            // het gemiddlede berekenen van alle aanvoerders (laatste 10)
            float avgAlle = 0;
            await using (var cmd = new SqlCommand(@"
        SELECT AVG(CAST(h.prijsPerStuk AS float))
        FROM productVerkoopHistorie h
        INNER JOIN product p ON p.productId = h.productId
        WHERE h.aantalVerkocht > 0
          AND p.soortPlant = @soortPlant
    ", connectt))
            {
                cmd.Parameters.AddWithValue("@soortPlant", soortPlant);

                var obj = await cmd.ExecuteScalarAsync();
                avgAlle = (obj == null || obj == DBNull.Value) ? 0 : Convert.ToSingle(obj);
            }

            var response = new PrijsHistorieResponseDto
            {
                soortPlant = soortPlant,
                aanvoerderNaam = aanvoerderNaam,
                avgAanvoerder = avgAanvoerder,
                last10Aanvoerder = last10Aanvoerder,
                avgAlleAanvoerders = avgAlle,
                last10AlleAanvoerders = last10Alle
            };

            return Ok(response);
        }   
    }
}