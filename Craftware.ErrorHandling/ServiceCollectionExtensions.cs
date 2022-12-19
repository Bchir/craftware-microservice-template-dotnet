using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace Craftware.ErrorHandling;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddExceptionHandler<TException, TExceptionHandler>(this IServiceCollection services)
        where TException : Exception
        where TExceptionHandler : ExceptionHandler<TException>
    {
        services.AddSingleton<ExceptionHandler<TException>, TExceptionHandler>();
        services.AddSingleton<IExceptionHandler, TExceptionHandler>();
        return services;
    }

    public static IServiceCollection AddProblemDetailsExtensions(this IServiceCollection services)
    {
        return services.AddProblemDetails(options =>
            {
                options.CustomizeProblemDetails = (p) =>
                {
                    p.ProblemDetails.Extensions["traceId"] = Activity.Current?.Id ?? p.HttpContext.TraceIdentifier;
                    p.ProblemDetails.Instance = p.HttpContext.Request.Path.Value;
                };
            });
    }
}