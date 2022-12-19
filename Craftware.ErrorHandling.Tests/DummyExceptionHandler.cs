namespace Craftware.ErrorHandling.Tests;

public class DummyExceptionHandler : ExceptionHandler<DummyException>
{
    public const string ServerErrorCode = "DUMMY_ERROR";
    public override ErrorDetails Sample()
    {
        return new ErrorDetails(400, "this error is dummy", ServerErrorCode);
    }

    protected override ErrorDetails Handle(DummyException exception)
    {
        return new ErrorDetails(400, "this error is dummy", ServerErrorCode);
    }
}
