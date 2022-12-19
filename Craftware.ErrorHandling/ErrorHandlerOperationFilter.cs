using HttpContextMoq;
using HttpContextMoq.Extensions;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.Json;

namespace Craftware.ErrorHandling;

public class ErrorHandlerOperationFilter : IOperationFilter
{
    private readonly IEnumerable<IExceptionHandler> _exceptionHandlers;
    private readonly ProblemDetailsFactory _problemDetailsFactory;
    private readonly IOptions<JsonOptions> _defaultJsonOptions;

    public ErrorHandlerOperationFilter(
        IEnumerable<IExceptionHandler> exceptionHandlers,
        ProblemDetailsFactory problemDetailsFactory,
        IOptions<JsonOptions> defaultJsonOptions)
    {
        _exceptionHandlers = exceptionHandlers;
        _problemDetailsFactory = problemDetailsFactory;
        _defaultJsonOptions = defaultJsonOptions;
    }

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var fakehttpContext = new HttpContextMock();
        fakehttpContext.SetupUrl("Https://localhost/" + context.ApiDescription.RelativePath);
        var errorSamples = _exceptionHandlers
            .Select(x => x.Sample())
            .Select(x => ProblemDetailsConfiguration.BuildProblemDetails(fakehttpContext, x, _problemDetailsFactory))
        .GroupBy(x => x.Status!);

        foreach (var problems in errorSamples)
        {
            var key = problems.Key == 0 ? "default" : problems.Key.ToString();
            var response = operation.Responses.FirstOrDefault(r => r.Key == key);
            if (response.Key is null)
                continue;
            response.Value.Description = ReasonPhrases.GetReasonPhrase(problems.Key!.Value);

            foreach (var samples in problems)
            {
                response.Value.Content["application/problem+json"].Examples.TryAdd(samples.Extensions["serviceErrorCode"]!.ToString(), new OpenApiExample
                {
                    Summary = samples.Extensions["serviceErrorCode"]!.ToString(),
                    Description = samples.Extensions["serviceErrorCode"]!.ToString(),
                    Value = OpenApiAnyFactory
                    .CreateFromJson(JsonSerializer.Serialize(samples, _defaultJsonOptions.Value.SerializerOptions))
                });
            }
        }
    }
}