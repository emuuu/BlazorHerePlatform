using System.Text;
using System.Text.Json;
using HerePlatform.Core.Coordinates;
using HerePlatform.Core.MatrixRouting;
using HerePlatform.Core.Routing;
using HerePlatform.Core.Services;
using HerePlatform.RestClient.Internal;

namespace HerePlatform.RestClient.Services;

internal sealed class RestMatrixRoutingService : IMatrixRoutingService
{
    private const string BaseUrl = "https://matrix.router.hereapi.com/v8/matrix";

    private readonly IHttpClientFactory _httpClientFactory;

    public RestMatrixRoutingService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<MatrixRoutingResult> CalculateMatrixAsync(MatrixRoutingRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.Origins);
        ArgumentNullException.ThrowIfNull(request.Destinations);

        if (request.Origins.Count == 0)
            throw new ArgumentException("Origins must not be empty.", nameof(request));
        if (request.Destinations.Count == 0)
            throw new ArgumentException("Destinations must not be empty.", nameof(request));

        var body = new
        {
            origins = request.Origins.Select(o => new { lat = o.Lat, lng = o.Lng }).ToArray(),
            destinations = request.Destinations.Select(d => new { lat = d.Lat, lng = d.Lng }).ToArray(),
            regionDefinition = new
            {
                type = "world"
            },
            transportMode = HereApiHelper.GetEnumMemberValue(request.TransportMode),
            routingMode = "fast"
        };

        var jsonBody = JsonSerializer.Serialize(body, HereJsonDefaults.Options);
        using var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

        var client = _httpClientFactory.CreateClient(HereApiHelper.ClientName);
        using var response = await client.PostAsync($"{BaseUrl}?async=false", content, cancellationToken).ConfigureAwait(false);

        await HereApiHelper.EnsureSuccessOrThrowAsync(response, "matrix", cancellationToken).ConfigureAwait(false);

        var json = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        var hereResponse = JsonSerializer.Deserialize<HereMatrixResponse>(json, HereJsonDefaults.Options);

        return MapToResult(hereResponse, request.Origins.Count, request.Destinations.Count);
    }

    private static MatrixRoutingResult MapToResult(HereMatrixResponse? hereResponse, int numOrigins, int numDestinations)
    {
        if (hereResponse?.Matrix is null)
            return new MatrixRoutingResult
            {
                NumOrigins = numOrigins,
                NumDestinations = numDestinations,
                Matrix = []
            };

        var matrix = hereResponse.Matrix;
        var entries = new List<MatrixEntry>(numOrigins * numDestinations);

        for (var i = 0; i < numOrigins; i++)
        {
            for (var j = 0; j < numDestinations; j++)
            {
                var idx = i * numDestinations + j;

                var errorCode = matrix.ErrorCodes is not null && idx < matrix.ErrorCodes.Count
                    ? matrix.ErrorCodes[idx]
                    : 0;

                if (errorCode != 0)
                    continue;

                var travelTime = matrix.TravelTimes is not null && idx < matrix.TravelTimes.Count
                    ? matrix.TravelTimes[idx]
                    : 0;

                var distance = matrix.Distances is not null && idx < matrix.Distances.Count
                    ? matrix.Distances[idx]
                    : 0;

                entries.Add(new MatrixEntry
                {
                    OriginIndex = i,
                    DestinationIndex = j,
                    Duration = travelTime,
                    Length = distance
                });
            }
        }

        return new MatrixRoutingResult
        {
            NumOrigins = matrix.NumOrigins,
            NumDestinations = matrix.NumDestinations,
            Matrix = entries
        };
    }
}
