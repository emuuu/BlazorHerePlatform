using HerePlatform.Core.Routing;
using HerePlatform.Core.Services;

namespace HerePlatform.RestClient.Services;

internal sealed class RestRoutingService : IRoutingService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public RestRoutingService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public Task<RoutingResult> CalculateRouteAsync(RoutingRequest request)
        => throw new NotImplementedException();
}
