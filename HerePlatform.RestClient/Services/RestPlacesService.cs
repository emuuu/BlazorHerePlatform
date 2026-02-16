using HerePlatform.Core.Places;
using HerePlatform.Core.Services;

namespace HerePlatform.RestClient.Services;

internal sealed class RestPlacesService : IPlacesService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public RestPlacesService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public Task<PlacesResult> DiscoverAsync(PlacesRequest request)
        => throw new NotImplementedException();

    public Task<PlacesResult> BrowseAsync(PlacesRequest request)
        => throw new NotImplementedException();

    public Task<PlacesResult> LookupAsync(PlacesRequest request)
        => throw new NotImplementedException();
}
