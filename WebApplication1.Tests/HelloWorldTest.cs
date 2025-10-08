using System;
using Xunit;

namespace WebApplication1.Tests
{
    public class HelloWorldTest
    {
        [Fact]
        public void PrintHelloWorld()
        {
            Console.WriteLine("hello world");
        }
    }
}