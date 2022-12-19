using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Exceptions;
using Serilog.Formatting.Compact;

namespace Service.WebApi;

public static class Configuration
{
    public static WebApplicationBuilder UseDefaultLogger(this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        builder.Services.AddOpenTracing();
        builder.Host.UseSerilog((_, sp, configuration) =>
        {
            var serviceMetadata = sp.GetRequiredService<ServiceMetadata>();
            EnrichLog(configuration, serviceMetadata);
            SinkLog(configuration);
        });

        return builder;
    }

    private static void SinkLog(LoggerConfiguration logconf)
    {
        const string LogTemplate = "[{Timestamp:HH:mm:ss}][{Level:u3}][{SourceContext:1}] {Message:lj}{NewLine}{Exception}";
        logconf
        .WriteTo.File(new RenderedCompactJsonFormatter(),
        "log.txt",
       fileSizeLimitBytes: 10 * 1024 * 1024,
       shared: true,
       flushToDiskInterval: TimeSpan.FromSeconds(1),
       rollingInterval: RollingInterval.Day,
       rollOnFileSizeLimit: true,
       retainedFileCountLimit: 2)
            .WriteTo.Console(outputTemplate: LogTemplate);
    }

    private static void EnrichLog(LoggerConfiguration configuration, ServiceMetadata metadata)
    {
        configuration
           .Enrich.WithProperty(nameof(ServiceMetadata.ServiceName), metadata.ServiceName)
           .Enrich.WithExceptionDetails()
           .Enrich.FromLogContext()
           .Enrich.WithThreadId()
           .Enrich.WithThreadName()
           .Enrich.WithProcessId()
           .Enrich.WithEnvironmentName()
           .Enrich.WithMachineName()
           .Enrich.FromGlobalLogContext()
           .Enrich.WithClientAgent()
           .Enrich.WithClientIp()
           .Enrich.WithSpan(new SpanOptions
           {
               IncludeBaggage = true,
               IncludeOperationName = true,
               IncludeTags = true
           });
    }
}