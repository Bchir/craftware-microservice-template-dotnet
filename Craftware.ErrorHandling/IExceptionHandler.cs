namespace Craftware.ErrorHandling;

public interface IExceptionHandler
{
    internal ErrorDetails HandleAbstract(Exception exception);

    internal ErrorDetails Sample();
}