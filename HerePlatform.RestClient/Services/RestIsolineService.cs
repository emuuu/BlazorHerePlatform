using HerePlatform.Core.Isoline;
using HerePlatform.Core.Services;

namespace HerePlatform.RestClient.Services;

internal sealed class RestIsolineService : IIsolineService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public RestIsolineService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public Task<IsolineResult> CalculateIsolineAsync(IsolineRequest request)
        => throw new NotImplementedException();
}
