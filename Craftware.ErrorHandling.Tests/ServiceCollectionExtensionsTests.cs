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
}