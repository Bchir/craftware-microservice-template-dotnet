using Asp.Versioning;
using Microsoft.Extensions.DependencyInjection;

namespace Craftware.ApiVersioning;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddVersioning(this IServiceCollection services, IVersionCollection versionCollection)
    {
        VersionState.Instance.VersionCollection = versionCollection;

        services.AddSingleton(versionCollection);

        var sunsets = versionCollection
            .GetAllAvailableVersions()
            .Where(s => s.SunsetPolicy is not null);

        services
            .AddApiVersioning(options =>
                {
                    options.ApiVersionReader = new UrlSegmentApiVersionReader();
                    options.ReportApiVersions = true;
                    foreach (var sunset in sunsets)
                        options.Policies.Sunset(sunset.ApiVersion).Per(sunset.SunsetPolicy!);
                })
            .EnableApiVersionBinding()
            .AddApiExplorer(opts =>
                {
                    opts.GroupNameFormat = "'v'VVV";
                    opts.SubstituteApiVersionInUrl = true;
                });
        return services;
    }

    public static IServiceCollection ConfigureSwaggerVersioning(this IServiceCollection services, SwaggerDocumentConfiguration config)
    {
        return services.AddSingleton(config)
            .ConfigureOptions<SwaggerVersioningOptions>();
    }
}