using Asp.Versioning;

namespace Service.WebApi.Versioning;

public record VersionDeclaration(ApiVersion ApiVersion, bool IsDeprecated = false, SunsetPolicy? SunsetPolicy = default);