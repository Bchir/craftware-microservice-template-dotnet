using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Service.WebApi;
using System.ComponentModel.DataAnnotations;
using Craftware.ErrorHandling;
using Craftware.ApiVersioning;
using Service.WebApi.ErrorHandling;
using Service.WebApi.Versioning;

var builder = WebApplication.CreateBuilder(args);

builder.UseDefaultLogger();
var serviceMetaData = builder.Configuration.GetSection(nameof(ServiceMetadata)).Get<ServiceMetadata>()!;
builder.Services
    .AddHttpClient()
    .AddSingleton(serviceMetaData)
    .AddExceptionHandler<NotImplementedException, NotImplementedExceptionHandler>()
    .AddExceptionHandler<NotSupportedException, NotSupportedExceptionHandler>()
    .AddExceptionHandler<ValidationException, ValidationExceptionHandler>()
    .AddProblemDetailsExtensions()
    .AddCors()
    .AddSwaggerGenerator(serviceMetaData)
    .AddEndpointsApiExplorer()
    .AddDefaultSerialisation()
    .AddHealth()
    .AddVersioning(new ApiVersionsDeclaration());

var app = builder.Build();

app.UseProbes();
app.UseSerilogRequestLogging();
app.UseVersionSet()
    .UseProblemDetails()
    .UseHttpsRedirection()
    .UseForwardedHeaders()
    .UseCors(builder => builder
     .AllowAnyOrigin()
     .AllowAnyMethod()
     .AllowAnyHeader());

app.MapGet("v{version:apiVersion}/TodoAction", () => Results.Conflict())
            .ProducesProblem(StatusCodes.Status409Conflict)
            .AvailableInVersions(new ApiVersion(1, 0), new ApiVersion(2, 0))
            .WithOpenApi();

app.MapGet("v{version:apiVersion}/{count}/NewTodoAction", ([FromRoute] string count) =>
{
    throw new NotImplementedException();
})
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .AvailableInVersions(new ApiVersion(2, 0))
            .WithOpenApi();

app.UseSwaggerDocumentation();
await app.RunAsync();