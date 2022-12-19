using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Craftware.ApiVersioning;

public static class WebApplicationExtensions
{
    public static WebApplication UseVersionSet(this WebApplication webApplication)
    {
        var versionset = webApplication.NewApiVersionSet();
        var versions = webApplication.Services.GetRequiredService<IVersionCollection>();

        foreach (var item in versions.GetAllAvailableVersions())
        {
            if (item.IsDeprecated)
                versionset.HasDeprecatedApiVersion(item.ApiVersion);
            else
                versionset.HasApiVersion(item.ApiVersion);
        }
        VersionState.Instance.ApiVersionSet = versionset.Build();
        return webApplication;
    }
}