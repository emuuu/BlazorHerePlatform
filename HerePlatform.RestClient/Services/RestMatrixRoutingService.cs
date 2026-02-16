using HerePlatform.Core.MatrixRouting;
using HerePlatform.Core.Services;

namespace HerePlatform.RestClient.Services;

internal sealed class RestMatrixRoutingService : IMatrixRoutingService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public RestMatrixRoutingService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public Task<MatrixRoutingResult> CalculateMatrixAsync(MatrixRoutingRequest request)
        => throw new NotImplementedException();
}
