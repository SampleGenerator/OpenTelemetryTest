namespace OpenTelemetrySample.Services.Models.Entities;

public class PlaceholderPost
{
    public int UserId { get; init; }
    public int Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Body { get; init; } = string.Empty;
}

