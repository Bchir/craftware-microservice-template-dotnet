using Craftware.ErrorHandling;
using System.ComponentModel.DataAnnotations;

namespace Service.WebApi.ErrorHandling;

public class ValidationExceptionHandler : ExceptionHandler<ValidationException>
{
    public const string ErrorCode = "VALIDATION_ERROR";

    public override ErrorDetails Sample()
    {
        return new(
            StatusCodes.Status409Conflict,
            "One or more validation rule were violated",
            ErrorCode,
            new Dictionary<string, object> { { "fieldName", new string[] { "Rule1", "Rule1" } } });
    }

    protected override ErrorDetails Handle(ValidationException exception)
    {
        return new(
            StatusCodes.Status409Conflict,
            "One or more validation rule were violated",
            ErrorCode,
            new Dictionary<string, object> {
                {
                    exception.ValidationResult.MemberNames.First(),
                    new string[] { exception.ValidationResult?.ErrorMessage! } } });
    }
}