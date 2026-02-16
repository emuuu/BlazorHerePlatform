using HerePlatform.Core.Services;
using HerePlatform.Core.Traffic;

namespace HerePlatform.RestClient.Services;

internal sealed class RestTrafficService : ITrafficService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public RestTrafficService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public Task<TrafficIncidentsResult> GetTrafficIncidentsAsync(double north, double south, double east, double west)
        => throw new NotImplementedException();

    public Task<TrafficFlowResult> GetTrafficFlowAsync(double north, double south, double east, double west)
        => throw new NotImplementedException();
}
