using OpenTelemetrySample.Services.Abstractions;
using OpenTelemetrySample.Services.Models.Entities;
using Refit;

namespace OpenTelemetrySample.Services.Implementations;

public sealed class PlaceholderService : IPlaceholderService
{
    private readonly IPlaceholderApi _api = RestService
        .For<IPlaceholderApi>("https://jsonplaceholder.typicode.com");

    public async Task<IEnumerable<PlaceholderPost>> GetPosts(CancellationToken ct = default)
    {
       return await _api.GetPosts(ct);
    }
}
