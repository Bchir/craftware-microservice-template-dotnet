using Asp.Versioning;
using Craftware.ApiVersioning.Exceptions;
using Microsoft.AspNetCore.Builder;

namespace Craftware.ApiVersioning;

public static class RouteHandlerBuilderExtensions
{
    public static RouteHandlerBuilder AvailableInVersions(
        this RouteHandlerBuilder routeHandlerBuilder,
        params ApiVersion[] versions)
    {
        if (VersionState.Instance.ApiVersionSet is null)
            throw new ApiVersionSetNotRegistredException();

        var availableVersions = VersionState.Instance.VersionCollection!.GetAllAvailableVersions().ToList();

        routeHandlerBuilder
            .WithApiVersionSet(VersionState.Instance.ApiVersionSet);

        foreach (var version in versions)
        {
            var actualVersion = availableVersions.SingleOrDefault(x => x.ApiVersion == version)
                           ?? throw new ApiVersionNotRegisteredException(version);

            if (actualVersion.IsDeprecated)
                routeHandlerBuilder.HasDeprecatedApiVersion(version);
            else
                routeHandlerBuilder.HasApiVersion(version);
        }
        return routeHandlerBuilder;
    }
}