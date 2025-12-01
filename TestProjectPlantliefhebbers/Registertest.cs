using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Controllers;
using WebApplication1;
using System.Threading.Tasks;

namespace WebApiTests
{

    public class Registertest
    {
        private PlantLiefhebbersContext GetInMemoryContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<PlantLiefhebbersContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new PlantLiefhebbersContext(options);
        }



        [Fact]
        public async Task register_test()
        {
            var dbName = "test";

            using (var context = GetInMemoryContext(dbName))
            {
                var controller = new KlantRegisterController(context);
                
                var klantDto = new KlantRegisterDto
                {
                    naam = null,
                    adres = "TestAdres",
                    email = "TestEmail",
                    wachtwoord = "TestWachtwoord"
                };

                var result = await controller.Register(klantDto);
                Assert.IsType<BadRequestObjectResult>(result);
                //var badResult = Assert.IsType<BadRequestObjectResult>(result);
                //Assert.Equal("Naam is verplicht.", badResult.Value);

            }
        }
    }
}