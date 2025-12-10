//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using WebApplication1.Controllers;
//using WebApplication1;
//using System.Threading.Tasks;

//namespace WebApiTests
//{

//    public class Inlogtests
//    {
//        private PlantLiefhebbersContext GetInMemoryContext(string dbName)
//        {
//            var options = new DbContextOptionsBuilder<PlantLiefhebbersContext>()
//                .UseInMemoryDatabase(databaseName: dbName)
//                .Options;

//            return new PlantLiefhebbersContext(options);
//        }



//        [Fact]
//        public void Connectie_test()
//        {
//            var dbName = "test";

//            using (var context = GetInMemoryContext(dbName))
//            {
//                var controller = new InlogController(context);

//                var result = controller.Test();
//                var okResult = Assert.IsType<OkObjectResult>(result);

//                Assert.Equal("Controller werkt", okResult.Value);
//            }
//        }

//        [Fact]
//        public async Task Id_test()
//        {
//            var dbName = "SimpeleTestDb";

//            using (var context = GetInMemoryContext(dbName))
//            {
//                var controller = new InlogController(context);

//                var result = controller.GetKlantID(1);

//                Assert.IsType<System.Threading.Tasks.Task<ActionResult<KlantDto>>>(result);
//            }
//        }
//    }
//}