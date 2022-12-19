namespace Service.WebApi;

public record ServiceMetadata
{
    public required string ServiceName { get; set; }
    public required string Description { get; set; }
    public required Uri BluePrintUri { get; set; }
}