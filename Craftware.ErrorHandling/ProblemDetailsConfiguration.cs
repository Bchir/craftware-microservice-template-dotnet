using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;

namespace Craftware.ErrorHandling;

public static class ProblemDetailsConfiguration
{
    private static readonly Type ExceptionHandlerType = typeof(ExceptionHandler<>);

    public static WebApplication UseProblemDetails(this WebApplication app)
    {
        _ = app.UseExceptionHandler(exceptionHandlerApp =>
         {
             exceptionHandlerApp.Run(async context =>
             {
                 var exceptionAccessor = context.Features.Get<IExceptionHandlerPathFeature>();

                 if (exceptionAccessor?.Error is null)
                     return;

                 var handlerType = ExceptionHandlerType.MakeGenericType(exceptionAccessor.Error.GetType());
                 var handler = context.RequestServices.GetService(handlerType);

                 if (handler is not IExceptionHandler exceptionHandler)
                     return;

                 var result = exceptionHandler.HandleAbstract(exceptionAccessor.Error);

                 var problemFactory = context.RequestServices.GetRequiredService<ProblemDetailsFactory>();
                 ProblemDetails problem = BuildProblemDetails(context, result, problemFactory);

                 await Results.Problem(problem).ExecuteAsync(context);
             });
         })
            .UseStatusCodePages();

        return app;
    }

    public static ProblemDetails BuildProblemDetails(HttpContext context, ErrorDetails result, ProblemDetailsFactory problemFactory)
    {
        var problem = problemFactory.CreateProblemDetails(context, result.StatusCode, detail: result.Message);

        problem.Extensions["serviceErrorCode"] = result.ServiceErrorCode;
        problem.Extensions["errors"] = result.Error;
        problem.Title ??= ReasonPhrases.GetReasonPhrase(result.StatusCode);
        return problem;
    }
}