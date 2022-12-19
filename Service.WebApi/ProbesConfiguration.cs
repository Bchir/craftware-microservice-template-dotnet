using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Service.WebApi;

public static class ProbesConfiguration
{
    private const string LivenessProbe = $"/{LivenessTag}";
    private const string HealthProbe = "/healthz";
    private const string ReadinessProbe = $"/{ReadinessTag}";
    private const string LivenessTag = "livez";
    private const string ReadinessTag = "readyz";

    public static IServiceCollection AddHealth(this IServiceCollection services)
    {
        services
            .AddHealthChecks()
            .AddDiskStorageHealthCheck(x =>
            {
                x.CheckAllDrives = true;
            }, tags: new List<string> { ReadinessTag, LivenessTag });

        return services;
    }

    public static WebApplication UseProbes(this WebApplication app)
    {
        app.MapHealthChecks(HealthProbe, new HealthCheckOptions
        {
            Predicate = (x) => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        app.MapHealthChecks(ReadinessProbe, new HealthCheckOptions
        {
            Predicate = (x) => x.Tags.Contains(ReadinessTag),
            ResultStatusCodes =
                {
                    [HealthStatus.Healthy] = StatusCodes.Status200OK,
                    [HealthStatus.Degraded] = StatusCodes.Status200OK,
                    [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
                }
        });

        app.MapHealthChecks(LivenessProbe, new HealthCheckOptions
        {
            Predicate = (x) => x.Tags.Contains(LivenessTag),
            ResultStatusCodes =
                {
                    [HealthStatus.Healthy] = StatusCodes.Status200OK,
                    [HealthStatus.Degraded] = StatusCodes.Status200OK,
                    [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
                }
        });

        return app;
    }
}