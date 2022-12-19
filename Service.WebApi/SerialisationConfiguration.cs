using Microsoft.AspNetCore.Http.Json;
using System.Text.Json.Serialization;
using MvcJsonOptions = Microsoft.AspNetCore.Mvc.JsonOptions;

namespace Service.WebApi;

public static class SerialisationConfiguration
{

    public static IServiceCollection AddDefaultSerialisation(this IServiceCollection services)
    {
        return services
             .Configure<JsonOptions>(options => options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()))
             .Configure<MvcJsonOptions>(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
    }
}