using Asp.Versioning;

namespace Craftware.ApiVersioning.Exceptions;

[Serializable]
public class ApiVersionNotRegisteredException : Exception
{
    public ApiVersion Version { get; protected set; }

    public ApiVersionNotRegisteredException(ApiVersion version)
    {
        Version = version;
    }
}