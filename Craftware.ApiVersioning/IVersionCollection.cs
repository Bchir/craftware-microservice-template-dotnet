using Service.WebApi.Versioning;

namespace Craftware.ApiVersioning
{
    public interface IVersionCollection
    {
        IEnumerable<VersionDeclaration> GetAllAvailableVersions();
    }
}