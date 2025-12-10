using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace WebApplication1.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("[controller]")]
    public class InlogController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly JwtTokenService _tokenService;

        public InlogController(SignInManager<User> signInManager, UserManager<User> userManager, JwtTokenService tokenService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenService = tokenService;
        }

        [HttpGet("id/{id}")]
        public async Task<ActionResult<KlantDto>> GetKlantID(String id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var dto = new KlantDto
            {
                klantId = user.Id,
                naam = user.UserName,
                adres = user.adres,
                email = user.Email
            };

            return dto;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.email)
                ?? await _userManager.FindByNameAsync(dto.email);

            if (user == null)
                return NotFound("Gebruiker niet gevonden.");

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.wachtwoord, false);

            if (!result.Succeeded)
                return Unauthorized("Ongeldig Wachtwoord.");


            var roles = await _userManager.GetRolesAsync(user);
            var token = _tokenService.GenerateToken(user, roles);

            var klantDto = new KlantDto
            {
                klantId = user.Id,
                naam = user.UserName,
                adres = user.adres,
                email = user.Email
            };

            return Ok(new { token, rol = roles.FirstOrDefault(), klant = klantDto });
        }

        [HttpGet("email/{email}")]
        public async Task<ActionResult<KlantDto>> GetKlantEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return NotFound();

            var dto = new KlantDto
            {
                klantId = user.Id,
                naam = user.UserName,
                adres = user.adres,
                email = user.Email
            };

            return dto;
        }
    }
}
