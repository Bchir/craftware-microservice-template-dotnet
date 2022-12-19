using Craftware.ErrorHandling;

namespace Service.WebApi.ErrorHandling;

public class NotSupportedExceptionHandler : ExceptionHandler<NotSupportedException>
{
    public const string ErrorCode = "NOT_SUPPORTED";

    protected override ErrorDetails Handle(NotSupportedException exception)
        => new(
            StatusCodes.Status400BadRequest,
            $"This method is not implemented yet {exception.TargetSite?.Name}",
            ErrorCode,
            new NotSupportedErrorDetails(exception.TargetSite?.Name)
            );

    public override ErrorDetails Sample()
    {
        return new(
            StatusCodes.Status400BadRequest,
            "This method is not implemented yet GetUser",
            ErrorCode,
            new NotSupportedErrorDetails("GetUser")
            );
    }
}

public record NotSupportedErrorDetails(string? Operation);