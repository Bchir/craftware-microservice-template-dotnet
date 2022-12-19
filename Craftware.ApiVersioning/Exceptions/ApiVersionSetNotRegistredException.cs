namespace Craftware.ApiVersioning.Exceptions;

[Serializable]
public class ApiVersionSetNotRegistredException : Exception
{
    public ApiVersionSetNotRegistredException() : base("Please use WebApplication.UseVersionSet before registering endpoints")
    {
    }
}