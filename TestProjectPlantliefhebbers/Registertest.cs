using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using WebApplication1.Controllers;


namespace WebApiTests
{
    public class RegisterTest
    { 
        //Mock usermanager aanmaken omdat de echte usermanager met de database werkt
        private Mock<UserManager<User>> GetMockUserManager()
        {
            var store = new Mock<IUserStore<User>>(); 
            return new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
        }
        
        //Mock rolemanager aanmaken
        private Mock<RoleManager<IdentityRole>> GetMockRoleManager()
        {
            var store = new Mock<IRoleStore<IdentityRole>>();
            return new Mock<RoleManager<IdentityRole>>(store.Object, null, null, null, null);
        }

        [Fact]
        public async Task NoName()
        {
            // maakt een controller aan met de mock UserManager en RoleManager (arrange)
            var userManager = GetMockUserManager();
            var roleManager = GetMockRoleManager();
            var controller = new KlantRegisterController(userManager.Object, roleManager.Object);

            //DTO zonder naam, hoort fout te gaan
            var dto = new KlantRegisterDto
            {
                naam = "",
                email = "Test@gmail.com",
                wachtwoord = "Test1!"
            }; 

            //Act
            var result = await controller.Register(dto);

            //controller hoort badrequest terug te geven want geen naam (assert)
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task NoEmail()
        {
            // maakt een controller aan met de mock UserManager en RoleManager (arrange)
            var userManager = GetMockUserManager();
            var roleManager = GetMockRoleManager();
            var controller = new KlantRegisterController(userManager.Object, roleManager.Object);

            //DTO zonder email, hoort fout te gaan
            var dto = new KlantRegisterDto
            {
                naam = "Test",
                email = "",
                wachtwoord = "Test1!"
            };

            //Act
            var result = await controller.Register(dto);

            //controller hoort badrequest terug te geven want geen email (assert)
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task ExistingEmail()
        {
            //Arrange
            var userManager = GetMockUserManager();
            var roleManager = GetMockRoleManager();

            //email zit al in het systeem
            userManager.Setup(x => x.FindByEmailAsync("Test@gmail.com"))
                       .ReturnsAsync(new User());

            var controller = new KlantRegisterController(userManager.Object, roleManager.Object);
            //DTO met bestaande email
            var dto = new KlantRegisterDto
            {
                naam = "Test",
                email = "Test@gmail.com",
                wachtwoord = "Test1!"
            };

            //Act
            var result = await controller.Register(dto);

            //controller hoort badrequest terug te geven want email bestaat al (assert)
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task ExistingNaam()
        {
            //Arrange
            var userManager = GetMockUserManager();
            var roleManager = GetMockRoleManager();

            //naam zit al in het systeem
            userManager.Setup(x => x.FindByNameAsync("Test"))
                       .ReturnsAsync(new User());

            var controller = new KlantRegisterController(userManager.Object, roleManager.Object);
            //DTO met bestaande naam
            var dto = new KlantRegisterDto
            {
                naam = "Test",
                email = "Test@gmail.com",
                wachtwoord = "Test1!"
            };

            //Act
            var result = await controller.Register(dto);

            //controller hoort badrequest terug te geven want naam bestaat al (assert)
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task ValidInput()
        {
            //Arrange
            var userManager = GetMockUserManager();
            var roleManager = GetMockRoleManager();

            // Geen email of naam bestaat.
            userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                       .ReturnsAsync((User)null);

            userManager.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                       .ReturnsAsync((User)null);

            // UserManager geslaagd, gebruiker is gemaakt
            userManager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                       .ReturnsAsync(IdentityResult.Success);

            var controller = new KlantRegisterController(userManager.Object, roleManager.Object);

            var dto = new KlantRegisterDto
            {
                naam = "Test",
                email = "Test@gmail.com",
                wachtwoord = "Test1!"
            };

            //Act
            var result = await controller.Register(dto);

            //controller hoort een okresult terug te geven want alles is goed ingevuld en het bestaat nog niet. (assert)
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task PasswordTest()
        {
            // Arrange
            var userManager = GetMockUserManager();
            var roleManager = GetMockRoleManager();

            var controller = new KlantRegisterController(userManager.Object, roleManager.Object);

            // DTO met fout wachtwoord
            var dto = new KlantRegisterDto
            {
                naam = "Test",
                email = "Test@gmail.com",
                wachtwoord = "123"
            };

            //Act
            var result = await controller.Register(dto);

            //controller hoort badrequest te geven want wachtwoord voldoet niet aan de eisen (assert)
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}