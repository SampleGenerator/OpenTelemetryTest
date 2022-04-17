using System.ComponentModel.DataAnnotations;

namespace OpenTelemetrySample.Entities;

public sealed class Post
{
    public int Id { get; init; }
    public int UserId { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Body { get; init; } = string.Empty;
}
