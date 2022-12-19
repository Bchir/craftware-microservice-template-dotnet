using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Craftware.ApiVersioning;

public static class SwaggerVersioning
{
    internal const string BannerStyle = @"<style>
     #warning-deprecation {
      background-color: rgb(255, 244, 229);
      color: rgb(102, 60, 0);
      padding:16px;
      padding-top: 6px;
      border-radius: 4px;
    }
    #warning-deprecation-icon::before {
    content: ""\26A0"";
        font-size: xxx-large;
        width: 100%;
        height: 100%;
        position: relative;
        left: 50%;
    }
    </style>";

    public static void AddVersionedEndpoints(
        this SwaggerUIOptions swaggerUIOptions,
        WebApplication app)
    {
        swaggerUIOptions.HeadContent += BannerStyle;

        var descriptions = app.DescribeApiVersions();
        foreach (var desc in descriptions.Reverse())
        {
            var url = $"/swagger/{desc.GroupName}/swagger.json";
            var name = desc.GroupName.ToUpperInvariant();
            var deprecationWarning = desc.IsDeprecated ? "Deprecated" : "";
            swaggerUIOptions.SwaggerEndpoint(url, $"{name} {deprecationWarning}");
        }
    }
}