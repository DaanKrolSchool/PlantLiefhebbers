using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;


namespace WebApplication1.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("[controller]")]
    public class KlantRegisterController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public KlantRegisterController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] KlantRegisterDto dto)
        {
            // validatie
            if (string.IsNullOrWhiteSpace(dto.naam))
                return BadRequest("Naam is verplicht.");

            if (string.IsNullOrWhiteSpace(dto.email))
                return BadRequest("Email is verplicht.");

            if (string.IsNullOrWhiteSpace(dto.wachtwoord) || dto.wachtwoord.Length < 6)
                return BadRequest("Wachtwoord moet minstens 6 tekens lang zijn.");
                        
            var emailBestaat = await _userManager.FindByEmailAsync(dto.email);
            if (emailBestaat != null)
                return BadRequest("Email bestaat al.");

            var naamBestaat = await _userManager.FindByNameAsync(dto.naam);
            if (naamBestaat != null)
                return BadRequest("Naam bestaat al");

            // dto naar model
            var user = new User
            {
                UserName = dto.naam,
                adres = dto.adres,
                Email = dto.email
            };

            var result = await _userManager.CreateAsync(user, dto.wachtwoord);

            if (!result.Succeeded)
                return BadRequest(new { errors = result.Errors.Select(e => e.Description) });

            // rol toevoegen
            if (!string.IsNullOrWhiteSpace(dto.rol) &&
                await _roleManager.RoleExistsAsync(dto.rol))
            {
                await _userManager.AddToRoleAsync(user, dto.rol);
            }

            return Ok(new { message = "Registratie succesvol!", user.Email });
        }
    }
}