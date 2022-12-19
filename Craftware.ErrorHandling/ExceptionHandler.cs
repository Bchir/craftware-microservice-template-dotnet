namespace Craftware.ErrorHandling;

public abstract class ExceptionHandler<TException> : IExceptionHandler
    where TException : Exception
{
    protected abstract ErrorDetails Handle(TException exception);

    public ErrorDetails HandleAbstract(Exception exception) => Handle((TException)exception);

    public abstract ErrorDetails Sample();
}