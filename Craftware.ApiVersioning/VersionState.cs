using Asp.Versioning.Builder;

namespace Craftware.ApiVersioning;

public sealed class VersionState
{
    public ApiVersionSet? ApiVersionSet { get; set; }
    public IVersionCollection? VersionCollection { get; set; }

    private VersionState()
    { }

    private static readonly object @lock = new object();
    private static VersionState? instance = default;

    public static VersionState Instance
    {
        get
        {
            if (instance == null)
            {
                lock (@lock)
                {
                    if (instance == null)
                    {
                        instance = new VersionState();
                    }
                }
            }
            return instance;
        }
    }
}