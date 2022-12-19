using Asp.Versioning;
using Craftware.ApiVersioning;
using Service.WebApi.Versioning;

namespace Service.WebApi;

public class ApiVersionsDeclaration : IVersionCollection
{
    private static readonly SunsetPolicy _v1SunsetPolicy
        = new SunsetPolicyBuilder("Api deprecation", new ApiVersion(1, 0))
            .Effective(new DateTimeOffset(DateTime.UtcNow.AddDays(1)))
            .Build();

    public IEnumerable<VersionDeclaration> GetAllAvailableVersions()
    {
        yield return new VersionDeclaration(new ApiVersion(1, 0), true, _v1SunsetPolicy);
        yield return new VersionDeclaration(new ApiVersion(2, 0), false);
    }
}