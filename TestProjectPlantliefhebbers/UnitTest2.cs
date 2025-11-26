using WebApplication1;
using Xunit;


namespace WebApiTests
{
    public class UnitTest2
    {
        [Fact]
        public void rekenen()
        {
            var a = 2;
            var b = 3;

            Assert.Equal(5, a + b);

        }

        [Fact]
        public void rekenen2()
        {
            var a = 23;
            var b = 3;

            Assert.Equal(26, a + b);

        }
    }
}
