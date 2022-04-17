using OpenTelemetrySample.Services.Models.Entities;

namespace OpenTelemetrySample.Services.Abstractions;

public interface IPlaceholderService
{
    Task<IEnumerable<PlaceholderPost>> GetPosts(CancellationToken ct = default);
}
