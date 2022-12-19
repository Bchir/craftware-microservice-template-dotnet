namespace Craftware.ErrorHandling;

public record ErrorDetails(int StatusCode, string Message, string ServiceErrorCode, object? Error);