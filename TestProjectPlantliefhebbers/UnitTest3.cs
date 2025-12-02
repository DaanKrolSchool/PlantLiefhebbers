//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using WebApplication1.Controllers;
//using WebApplication1;
//using System.Threading.Tasks;

//namespace WebApiTests
//{

//    public class ProductControllerTests
//    {
//        private PlantLiefhebbersContext GetInMemoryContext(string dbName)
//        {
//            var options = new DbContextOptionsBuilder<PlantLiefhebbersContext>()
//                .UseInMemoryDatabase(databaseName: dbName)
//                .Options;

//            return new PlantLiefhebbersContext(options);
//        }



//        [Fact]
//        public void ProductConnectie_test()
//        {
//            var dbName = "SimpeleTestDb";

//            using (var context = GetInMemoryContext(dbName))
//            {
//                var controller = new InlogController(context);

//                var result = controller.Test();
//                var okResult = Assert.IsType<OkObjectResult>(result);

//                Assert.Equal("Controller werkt", okResult.Value);
//            }
//        }

//        //[Fact]
//        //public async Task IdProduct_test()
//        //{
//        //    var dbName = "SimpeleTestDb";

//        //    using (var context = GetInMemoryContext(dbName))
//        //    {
//        //        var controller = new ProductController(context);

//        //        var result = controller.GetAllProducts;

//        //        Assert.IsType<System.Threading.Tasks.Task<ActionResult<ProductDto>>>(result);
//        //    }
//        //}
//    }
//}