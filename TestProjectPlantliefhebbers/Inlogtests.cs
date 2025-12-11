using Castle.Core.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Threading.Tasks;
using WebApplication1;
using WebApplication1.Controllers;

namespace WebApiTests
{

    public class Inlogtests
    {

        private Mock<UserManager<User>> GetMockUserManager()
        {
            var store = new Mock<IUserStore<User>>();
            return new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
        }

        private Mock<SignInManager<User>> GetMockSignInManager()
        {
            var store = new Mock<UserManager<User>>();

            var userManager = GetMockUserManager().Object;
            var contextAccessor = new Mock<IHttpContextAccessor>();
            var claimsFactory = new Mock<IUserClaimsPrincipalFactory<User>>();

            return new Mock<SignInManager<User>>(userManager, contextAccessor.Object, claimsFactory.Object, null, null, null, null);
        }

        private readonly JwtTokenService _tokenService;

        [Fact]
        public async Task NonExistingMail()
        {
            var userManager = GetMockUserManager();
            var signInManager = GetMockSignInManager();

            var controller = new InlogController(signInManager.Object, userManager.Object, _tokenService);
            
            var dto = new LoginDto
            {
                email = "",
                wachtwoord = ""
            };
            
            var result = await controller.Login(dto);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task WrongPassword()
        {
            var userManager = GetMockUserManager();
            var signInManager = GetMockSignInManager();

            var mockUser = new User { UserName = "daan", Email = "daan@mail.com" };

            userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                       .ReturnsAsync(mockUser);

            signInManager.Setup(x => x.CheckPasswordSignInAsync(
                It.IsAny<User>(),
                It.IsAny<string>(), 
                It.IsAny<bool>()
            ))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

            var controller = new InlogController(signInManager.Object, userManager.Object, _tokenService);

            var dto = new LoginDto
            {
                email = "daan@mail.com",
                wachtwoord = "1111"
            };

            var result = await controller.Login(dto);

            Assert.IsType<UnauthorizedObjectResult>(result);
        }
    }
}

