using Craftware.ErrorHandling;

namespace Service.WebApi.ErrorHandling;

public class NotImplementedExceptionHandler : ExceptionHandler<NotImplementedException>
{
    public const string ErrorCode = "NOT_IMPLEMENTED";

    protected override ErrorDetails Handle(NotImplementedException exception)
        => new(
            StatusCodes.Status400BadRequest,
            $"This method is not implemented yet {exception.TargetSite?.Name}",
            ErrorCode,
            new NotImplementedErrorDetails(exception.TargetSite?.Name)
            );

    public override ErrorDetails Sample()
    {
        return new(
            StatusCodes.Status400BadRequest,
            "This method is not implemented yet GetUser",
            ErrorCode,
            new NotImplementedErrorDetails("GetUser")
            );
    }
}

public record NotImplementedErrorDetails(string? MethodName);