using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using WebApplication1.Controllers;

public class RegisterTest
{
    private Mock<UserManager<User>> GetMockUserManager()
    {
        var store = new Mock<IUserStore<User>>();
        return new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
    }

    private Mock<RoleManager<IdentityRole>> GetMockRoleManager()
    {
        var store = new Mock<IRoleStore<IdentityRole>>();
        return new Mock<RoleManager<IdentityRole>>(store.Object, null, null, null, null);
    }

    [Fact]
    public async Task MissingName()
    {
        var userManager = GetMockUserManager();
        var roleManager = GetMockRoleManager();
        var controller = new KlantRegisterController(userManager.Object, roleManager.Object);

        var dto = new KlantRegisterDto
        {
            naam = "",
            email = "test@mail.com",
            wachtwoord = "123456"
        };

        var result = await controller.Register(dto);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task ExistingEmail()
    {
        var userManager = GetMockUserManager();
        var roleManager = GetMockRoleManager();

        // Pretend user exists
        userManager.Setup(x => x.FindByEmailAsync("test@mail.com"))
                   .ReturnsAsync(new User());

        var controller = new KlantRegisterController(userManager.Object, roleManager.Object);

        var dto = new KlantRegisterDto
        {
            naam = "Justin",
            email = "test@mail.com",
            wachtwoord = "123456"
        };

        var result = await controller.Register(dto);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task ValidData()
    {
        var userManager = GetMockUserManager();
        var roleManager = GetMockRoleManager();

        // Pretend no user exists
        userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                   .ReturnsAsync((User)null);

        userManager.Setup(x => x.FindByNameAsync(It.IsAny<string>()))
                   .ReturnsAsync((User)null);

        // Pretend create succeeds
        userManager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                   .ReturnsAsync(IdentityResult.Success);

        var controller = new KlantRegisterController(userManager.Object, roleManager.Object);

        var dto = new KlantRegisterDto
        {
            naam = "Test",
            email = "Test@gmail.com",
            wachtwoord = "Test1!"
        };

        var result = await controller.Register(dto);

        Assert.IsType<OkObjectResult>(result);
    }
}
