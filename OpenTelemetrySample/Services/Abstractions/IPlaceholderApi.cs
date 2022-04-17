using OpenTelemetrySample.Services.Models.Entities;
using Refit;

namespace OpenTelemetrySample.Services.Abstractions;

public interface IPlaceholderApi
{
    [Get("/posts")]
    Task<IEnumerable<PlaceholderPost>> GetPosts(CancellationToken ct = default);
}
