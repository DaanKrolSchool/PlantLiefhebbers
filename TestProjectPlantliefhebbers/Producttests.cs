using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Controllers;
using WebApplication1;
using System.Threading.Tasks;

namespace WebApiTests
{

    public class ProductControllerTests
    {
        private PlantLiefhebbersContext GetInMemoryContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<PlantLiefhebbersContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            return new PlantLiefhebbersContext(options);
        }

        [Fact]
        public async Task Get_products_ordered_by_date()
        {
            var dbName = "SimpeleTestDb_Datum";

            using (var context = GetInMemoryContext(dbName))
            {
                //arrange
                context.product.AddRange(
                    new Product
                    {

                        productId = 1,
                        naam = "Plant Pedro",
                        soortPlant = "Cactus",
                        veilDatum = new DateTime(2024, 7, 1),
                        klokLocatie = "Amsterdam",
                    },

                    new Product
                    {
                        productId = 2,
                        naam = "Plant Pablo",
                        soortPlant = "Bloem",
                        veilDatum = new DateTime(2024, 6, 1),
                        klokLocatie = "Rotterdam",
                    },

                    new Product
                    {
                        productId = 3,
                        naam = "Plant Juan",
                        soortPlant = "Boom",
                        veilDatum = new DateTime(2024, 5, 1),
                        klokLocatie = "Utrecht",
                    }
            );
                context.SaveChanges();


                var controller = new ProductController(context);

                //act
                var result = controller.GetAllProducts();

                //assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                var products = Assert.IsType<List<ProductDto>>(okResult.Value);
                Assert.Equal(3, products.Count);

                Assert.Collection(products,
                p => Assert.Equal(new DateTime(2024, 5, 1), p.veilDatum),
                p => Assert.Equal(new DateTime(2024, 6, 1), p.veilDatum),
                p => Assert.Equal(new DateTime(2024, 7, 1), p.veilDatum)
            );
            }
        }
        [Fact]
        public void Add_product_test()
        {
            var dbName = "SimpeleTestDb_Teovoegen";
            using (var context = GetInMemoryContext(dbName))
            {

                //arrange
                var controller = new ProductController(context);
                var newProductDto = new ProductCreateDto
                {
                    naam = "Corostische",
                    soortPlant = "Gras",
                    aantal = 5,
                    potMaat = 10,
                    steelLengte = 30,
                    minimumPrijs = 15.0f,
                    maximumPrijs = 50.0f,
                    klokLocatie = "Amsterdam",
                    veilDatum = new DateTime(2024, 8, 1),
                    aanvoerderId = 1
                };

                //act
                var result = controller.AddProduct(newProductDto);

                //assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                var productDto = Assert.IsType<ProductDto>(okResult.Value);
                Assert.Equal("Corostische", productDto.naam);
                Assert.Equal("Gras", productDto.soortPlant);
                Assert.Equal(5, productDto.aantal);
                Assert.Equal(10, productDto.potMaat);
                Assert.Equal(30, productDto.steelLengte);
                Assert.Equal(15.0f, productDto.minimumPrijs);
                Assert.Equal(50.0f, productDto.maximumPrijs);
                Assert.Equal("Amsterdam", productDto.klokLocatie);
                Assert.Equal(new DateTime(2024, 8, 1), productDto.veilDatum);
                Assert.Equal(1, productDto.aanvoerderId);
            }
        }
        [Fact]
        public void Add_product_missing_optional_fields_test()
        {
            var dbName = "SimpeleTestDb_MissingField";
            using (var context = GetInMemoryContext(dbName))
            {
                //arrange
                var controller = new ProductController(context);
                var newProductDto = new ProductCreateDto
                {
                    naam = "Cactus",
                    soortPlant = "Succulent",
                    aantal = 3,
                    potMaat = null,
                    steelLengte = null,
                    minimumPrijs = 20.0f,
                    maximumPrijs = 100.0f,
                    klokLocatie = "Rotterdam",
                    veilDatum = new DateTime(2024, 9, 1),
                    aanvoerderId = 2
                };
                //act
                var result = controller.AddProduct(newProductDto);
                //assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                var productDto = Assert.IsType<ProductDto>(okResult.Value);
                Assert.Equal("Cactus", productDto.naam);
                Assert.Equal("Succulent", productDto.soortPlant);
                Assert.Equal(3, productDto.aantal);
                Assert.Null(productDto.potMaat);
                Assert.Null(productDto.steelLengte);
                Assert.Equal(20.0f, productDto.minimumPrijs);
                Assert.Equal(100.0f, productDto.maximumPrijs);
                Assert.Equal("Rotterdam", productDto.klokLocatie);
                Assert.Equal(new DateTime(2024, 9, 1), productDto.veilDatum);
                Assert.Equal(2, productDto.aanvoerderId);
            }
        }

        [Fact]
        public async Task Eerste_Product_Vinden_Test()
        {
            var dbName = "SimpeleTestDb_Eerste_Product";
            using (var context = GetInMemoryContext(dbName))
            {
                //arrange
                context.product.AddRange(
                    new Product
                    {
                        productId = 1,
                        naam = "Corostisch",
                        soortPlant = "Gras",
                        veilDatum = new DateTime(2024, 5, 1),
                        klokLocatie = "Amsterdam",

                    },
                    new Product
                    {
                        productId = 2,
                        soortPlant = "FruitPlant",
                        naam = "Bananenplant",
                        veilDatum = new DateTime(2024, 6, 1),
                        klokLocatie = "Rotterdam",
                    },
                    new Product
                    {
                        productId = 3,
                        naam = "blaadjes",
                        soortPlant = "Struik",
                        veilDatum = new DateTime(2024, 7, 1),
                        klokLocatie = "Utrecht",
                    }
            );
                context.SaveChanges();
            }
            using (var context = GetInMemoryContext(dbName))
            {
                var controller = new ProductController(context);
                //act
                var result = await controller.GetEersteProduct();
                var actionResult = Assert.IsType<ActionResult<Product>>(result);
                //assert
                var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
                var productDto = Assert.IsType<ProductDto>(okResult.Value);
                Assert.Equal("Corostisch", productDto.naam);
                Assert.Equal(new DateTime(2024, 5, 1), productDto.veilDatum);
            }

        }
        [Fact]
        public async Task Geen_Producten_Beschikbaar_Test()
        {
            var dbName = "SimpeleTestDb_Empty";
            using (var context = GetInMemoryContext(dbName))
            {
                //arrange
                var controller = new ProductController(context);
                //act
                var result = await controller.GetEersteProduct();
                var actionResult = Assert.IsType<ActionResult<Product>>(result);
                //assert
                var notFoundResult = Assert.IsType<NotFoundResult>(actionResult.Result);
            }
        }
        [Fact]
        public async Task Product_verwijderen_test()
        {
            var dbName = "SimpeleTestDb_Delete";
            using (var context = GetInMemoryContext(dbName))
            {
                //arrange
                context.product.AddRange(
                    new Product
                    {
                        productId = 1,
                        naam = "Gras",
                        soortPlant = "Struik",
                        veilDatum = new DateTime(2024, 7, 1),
                        klokLocatie = "Amsterdam",
                    }
            );
                context.SaveChanges();
            }
            using (var context = GetInMemoryContext(dbName))
            {
                var controller = new ProductController(context);
                //act
                var result = await controller.DeleteProduct(1);
                //assert
                var noContentResult = Assert.IsType<NoContentResult>(result);
                var deletedProduct = context.product.Find(1);
                Assert.Null(deletedProduct);
            }
        }
    }
}