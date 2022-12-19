using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;

namespace Craftware.ApiVersioning;

internal class SwaggerVersioningOptions : IConfigureNamedOptions<SwaggerGenOptions>
{
    private const string DeprecationWarning = "<strong>This API version has been deprecated. Please use one of the new APIs available from the explorer.</strong>";
    private const string BreakLine = "</br>";

    private readonly IApiVersionDescriptionProvider _provider;
    private readonly SwaggerDocumentConfiguration _swaggerDocumentConfiguration;

    public SwaggerVersioningOptions(IApiVersionDescriptionProvider provider, SwaggerDocumentConfiguration swaggerDocumentConfiguration)
    {
        _provider = provider;
        _swaggerDocumentConfiguration = swaggerDocumentConfiguration;
    }

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(
                description.GroupName,
                CreateVersionInfo(description));
        }
    }

    public void Configure(string? name, SwaggerGenOptions options)
    {
        Configure(options);
    }

    private OpenApiInfo CreateVersionInfo(ApiVersionDescription desc)
    {
        var info = new OpenApiInfo()
        {
            Title = $"{_swaggerDocumentConfiguration.Title} {desc.ApiVersion}",
            Version = desc.ApiVersion.ToString()
        };
        GenerateDeprecationWarning(desc, info);

        info.Description += BreakLine + BreakLine + _swaggerDocumentConfiguration.Description;

        return info;
    }

    private static void GenerateDeprecationWarning(ApiVersionDescription desc, OpenApiInfo info)
    {
        var warning = new StringBuilder();
        if (desc.IsDeprecated)
        {
            warning.Append("<div id=\"warning-deprecation\">");
            warning.Append("<div id=\"warning-deprecation-icon\">");
            warning.AppendLine("</div>");

            warning.AppendLine(DeprecationWarning);
            if (desc.SunsetPolicy is SunsetPolicy policy)
            {
                warning.AppendLine(BreakLine);
                warning.AppendLine(policy.GetHtmlSunsetWarning());
            }
            warning.AppendLine("</div>");
        }

        info.Description += warning.ToString();
    }
}