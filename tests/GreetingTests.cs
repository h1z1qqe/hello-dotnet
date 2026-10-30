using HelloDotnet;
using Xunit;

namespace HelloDotnet.Tests;

public class GreetingTests
{
    [Fact]
    public void Greet_ReturnsExpectedMessage()
    {
        Assert.Equal("Hello, .NET!", Greeting.Greet(".NET"));
        Assert.Equal("Hello, CI!", Greeting.Greet("CI"));
    }

    [Fact]
    public void SumRange_ReturnsCorrectSum()
    {
        Assert.Equal(55, Greeting.SumRange(1, 10));
        Assert.Equal(5050, Greeting.SumRange(1, 100));
    }

    [Fact]
    public void Fibonacci_ReturnsCorrectValues()
    {
        Assert.Equal(0L, Greeting.Fibonacci(0));
        Assert.Equal(1L, Greeting.Fibonacci(1));
        Assert.Equal(1L, Greeting.Fibonacci(2));
        Assert.Equal(55L, Greeting.Fibonacci(10));
        Assert.Equal(6765L, Greeting.Fibonacci(20));
    }
}
