using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Security.Claims;
using WebApplication1.ClientApp.DTOs;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class KlantController : ControllerBase
    {
        [Authorize]
        [HttpGet("current")]
        public IActionResult GetCurrentUser()
        {
            var userName = User.FindFirst("UserName")?.Value;

            return Ok(new
            {
                userName
            });
        }
    }
}