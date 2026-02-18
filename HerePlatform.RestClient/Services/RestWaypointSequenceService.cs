using System.Text.Json;
using HerePlatform.Core.Coordinates;
using HerePlatform.Core.Routing;
using HerePlatform.Core.Services;
using HerePlatform.Core.WaypointSequence;
using HerePlatform.RestClient.Internal;

namespace HerePlatform.RestClient.Services;

internal sealed class RestWaypointSequenceService : IWaypointSequenceService
{
    private const string BaseUrl = "https://wps.hereapi.com/v8/findsequence2";

    private readonly IHttpClientFactory _httpClientFactory;

    public RestWaypointSequenceService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<WaypointSequenceResult> OptimizeSequenceAsync(WaypointSequenceRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var parameters = new List<(string key, string? value)>
        {
            ("start", HereApiHelper.FormatCoord(request.Start)),
            ("end", HereApiHelper.FormatCoord(request.End)),
            ("mode", HereApiHelper.MapTransportMode(request.TransportMode))
        };

        if (request.Waypoints is { Count: > 0 })
        {
            for (int i = 0; i < request.Waypoints.Count; i++)
                parameters.Add(($"destination{i + 1}", HereApiHelper.FormatCoord(request.Waypoints[i])));
        }

        var qs = HereApiHelper.BuildQueryString(parameters.ToArray());
        var url = $"{BaseUrl}?{qs}";

        var client = _httpClientFactory.CreateClient(HereApiHelper.ClientName);
        using var response = await client.GetAsync(url, cancellationToken).ConfigureAwait(false);

        await HereApiHelper.EnsureSuccessOrThrowAsync(response, "waypoint-sequence", cancellationToken).ConfigureAwait(false);

        var json = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        var hereResponse = JsonSerializer.Deserialize<HereWaypointSequenceResponse>(json, HereJsonDefaults.Options);

        return MapToResult(hereResponse);
    }

    private static WaypointSequenceResult MapToResult(HereWaypointSequenceResponse? response)
    {
        var result = response?.Results is { Count: > 0 } ? response.Results[0] : null;
        if (result?.Waypoints is null)
            return new WaypointSequenceResult();

        var destinations = result.Waypoints
            .Where(w => w.Id is not null && w.Id.StartsWith("destination", StringComparison.Ordinal))
            .OrderBy(w => w.Sequence)
            .ToList();

        var optimizedIndices = new List<int>();
        var optimizedWaypoints = new List<LatLngLiteral>();

        foreach (var wp in destinations)
        {
            if (int.TryParse(wp.Id!.AsSpan("destination".Length), out var num))
                optimizedIndices.Add(num - 1); // 1-based destination id → 0-based index
            optimizedWaypoints.Add(new LatLngLiteral(wp.Lat, wp.Lng));
        }

        return new WaypointSequenceResult
        {
            OptimizedIndices = optimizedIndices,
            OptimizedWaypoints = optimizedWaypoints,
            TotalDistance = result.Distance,
            TotalDuration = result.Time
        };
    }
}
