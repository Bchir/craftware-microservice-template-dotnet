using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Craftware.ErrorHandling.Tests;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void ShouldBeAbleToResolveHandlers()
    {
        var services = new ServiceCollection();
        services.AddExceptionHandler<DummyException, DummyExceptionHandler>();
        var sp = services.BuildServiceProvider();

        var handler = sp.GetService<IExceptionHandler>();
        var specificHandler = sp.GetService<ExceptionHandler<DummyException>>();

        specificHandler.Should().NotBeNull();
        handler.Should().BeOfType<DummyExceptionHandler>();
    }

    [Fact]
    public void ShouldBeAbleToExtendProblemDetails()
    {
        var services = new ServiceCollection();
        services.AddProblemDetailsExtensions();
        var sp = services.BuildServiceProvider();

        var handler = sp.GetService<ProblemDetailsFactory>();

        handler.Should().NotBeNull();
    }
}