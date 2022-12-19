using Craftware.ErrorHandling;
using Craftware.ApiVersioning;

namespace Service.WebApi;

public static class SwaggerConfiguration
{
    public static IServiceCollection AddSwaggerGenerator(this IServiceCollection services, ServiceMetadata serviceMetadata)
    {
        string description =
            $@"{serviceMetadata.Description} </br>
               Refer to <a href=""{serviceMetadata.BluePrintUri}"">{serviceMetadata.ServiceName} BluePrint </a>";

        services.ConfigureSwaggerVersioning(new SwaggerDocumentConfiguration(serviceMetadata.ServiceName, description));

        services.AddSwaggerGen(o =>
        {
            o.OperationFilter<ErrorHandlerOperationFilter>();
            o.UseAllOfForInheritance();
            o.UseAllOfToExtendReferenceSchemas();
            o.UseInlineDefinitionsForEnums();
            o.UseOneOfForPolymorphism();
            o.SupportNonNullableReferenceTypes();
            o.DescribeAllParametersInCamelCase();
            o.OrderActionsBy(x => x.HttpMethod);
        });
        return services;
    }

    public static WebApplication UseSwaggerDocumentation(this WebApplication app)
    {
        var serviceMetadata = app.Services.GetRequiredService<ServiceMetadata>();
        if (!app.Environment.IsProduction())
        {
            app.UseSwagger();
            app.UseSwaggerUI(o =>
            {
                o.EnableValidator();
                o.EnableDeepLinking();
                o.ShowCommonExtensions();
                o.ShowExtensions();
                o.DocumentTitle = serviceMetadata.ServiceName;
                o.AddVersionedEndpoints(app);
            });
        }
        return app;
    }
}