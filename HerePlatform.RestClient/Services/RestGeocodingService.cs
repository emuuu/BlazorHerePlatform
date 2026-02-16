using HerePlatform.Core.Coordinates;
using HerePlatform.Core.Geocoding;
using HerePlatform.Core.Services;

namespace HerePlatform.RestClient.Services;

internal sealed class RestGeocodingService : IGeocodingService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public RestGeocodingService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public Task<GeocodeResult> GeocodeAsync(string query, GeocodeOptions? options = null)
        => throw new NotImplementedException();

    public Task<GeocodeResult> ReverseGeocodeAsync(LatLngLiteral position, GeocodeOptions? options = null)
        => throw new NotImplementedException();
}
