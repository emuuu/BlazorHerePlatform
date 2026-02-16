using HerePlatform.Core.Coordinates;
using HerePlatform.Core.Services;
using HerePlatform.Core.Transit;

namespace HerePlatform.RestClient.Services;

internal sealed class RestPublicTransitService : IPublicTransitService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public RestPublicTransitService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public Task<TransitDeparturesResult> GetDeparturesAsync(LatLngLiteral position)
        => throw new NotImplementedException();

    public Task<TransitStationsResult> SearchStationsAsync(LatLngLiteral position, double radiusMeters = 500)
        => throw new NotImplementedException();
}
