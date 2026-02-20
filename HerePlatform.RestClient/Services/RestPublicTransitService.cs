using System.Text.Json;
using HerePlatform.Core.Coordinates;
using HerePlatform.Core.Services;
using HerePlatform.Core.Transit;
using HerePlatform.RestClient.Internal;

namespace HerePlatform.RestClient.Services;

internal sealed class RestPublicTransitService : IPublicTransitService
{
    private const string DeparturesBaseUrl = "https://transit.hereapi.com/v8/departures";
    private const string StationsBaseUrl = "https://transit.hereapi.com/v8/stations";

    private readonly IHttpClientFactory _httpClientFactory;

    public RestPublicTransitService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<TransitDeparturesResult> GetDeparturesAsync(LatLngLiteral position, CancellationToken cancellationToken = default)
    {
        var qs = HereApiHelper.BuildQueryString(
            ("in", $"{HereApiHelper.FormatCoord(position)};r=500"));

        var url = $"{DeparturesBaseUrl}?{qs}";

        var client = _httpClientFactory.CreateClient(HereApiHelper.ClientName);
        using var response = await client.GetAsync(url, cancellationToken).ConfigureAwait(false);

        await HereApiHelper.EnsureSuccessOrThrowAsync(response, "transit", cancellationToken).ConfigureAwait(false);

        var json = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        var hereResponse = JsonSerializer.Deserialize<HereTransitDeparturesResponse>(json, HereJsonDefaults.Options);

        return MapDepartures(hereResponse);
    }

    public async Task<TransitStationsResult> SearchStationsAsync(LatLngLiteral position, double radiusMeters = 500, CancellationToken cancellationToken = default)
    {
        var qs = HereApiHelper.BuildQueryString(
            ("in", $"{HereApiHelper.FormatCoord(position)};r={HereApiHelper.Invariant(radiusMeters)}"));

        var url = $"{StationsBaseUrl}?{qs}";

        var client = _httpClientFactory.CreateClient(HereApiHelper.ClientName);
        using var response = await client.GetAsync(url, cancellationToken).ConfigureAwait(false);

        await HereApiHelper.EnsureSuccessOrThrowAsync(response, "transit", cancellationToken).ConfigureAwait(false);

        var json = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        var hereResponse = JsonSerializer.Deserialize<HereTransitStationsResponse>(json, HereJsonDefaults.Options);

        return MapStations(hereResponse);
    }

    private static TransitDeparturesResult MapDepartures(HereTransitDeparturesResponse? hereResponse)
    {
        if (hereResponse?.Boards is null or { Count: 0 })
            return new TransitDeparturesResult { Departures = [] };

        var departures = new List<TransitDeparture>();

        foreach (var board in hereResponse.Boards)
        {
            var stationName = board.Place?.Name;

            if (board.Departures is null) continue;

            foreach (var dep in board.Departures)
            {
                departures.Add(new TransitDeparture
                {
                    LineName = dep.Transport?.Name,
                    Direction = dep.Headsign,
                    DepartureTime = dep.Time,
                    TransportType = dep.Transport?.Mode,
                    StationName = stationName
                });
            }
        }

        return new TransitDeparturesResult { Departures = departures };
    }

    private static TransitStationsResult MapStations(HereTransitStationsResponse? hereResponse)
    {
        if (hereResponse?.Stations is null or { Count: 0 })
            return new TransitStationsResult { Stations = [] };

        return new TransitStationsResult
        {
            Stations = hereResponse.Stations.Select(s => new TransitStation
            {
                Name = s.Place?.Name,
                Position = s.Place?.Location is not null
                    ? new LatLngLiteral(s.Place.Location.Lat, s.Place.Location.Lng)
                    : null,
                Distance = s.Distance,
                TransportTypes = s.Transports?.Select(t => t.Mode!)
                    .Where(m => m is not null)
                    .Distinct()
                    .ToList()
            }).ToList()
        };
    }
}
