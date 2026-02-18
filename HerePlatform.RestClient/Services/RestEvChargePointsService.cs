using System.Text.Json;
using HerePlatform.Core.Coordinates;
using HerePlatform.Core.EvChargePoints;
using HerePlatform.Core.Services;
using HerePlatform.RestClient.Internal;

namespace HerePlatform.RestClient.Services;

internal sealed class RestEvChargePointsService : IEvChargePointsService
{
    private const string BaseUrl = "https://ev-v2.cc.api.here.com/ev/stations.json";

    private readonly IHttpClientFactory _httpClientFactory;

    public RestEvChargePointsService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<EvChargePointsResult> SearchStationsAsync(EvChargePointsRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var prox = $"{HereApiHelper.FormatCoord(request.Position)},{HereApiHelper.Invariant(request.Radius)}";

        var connectorTypes = request.ConnectorTypes is { Count: > 0 }
            ? string.Join(",", request.ConnectorTypes.Select(c => HereApiHelper.GetEnumMemberValue(c)))
            : null;

        var qs = HereApiHelper.BuildQueryString(
            ("prox", prox),
            ("connectortype", connectorTypes),
            ("maxresults", HereApiHelper.Invariant(request.MaxResults)));

        var url = $"{BaseUrl}?{qs}";

        var client = _httpClientFactory.CreateClient(HereApiHelper.ClientName);
        using var response = await client.GetAsync(url, cancellationToken).ConfigureAwait(false);

        await HereApiHelper.EnsureSuccessOrThrowAsync(response, "evChargePoints", cancellationToken).ConfigureAwait(false);

        var json = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        var hereResponse = JsonSerializer.Deserialize<HereEvChargePointsResponse>(json, HereJsonDefaults.Options);

        return MapToResult(hereResponse);
    }

    private static EvChargePointsResult MapToResult(HereEvChargePointsResponse? hereResponse)
    {
        if (hereResponse?.EvStations is null or { Count: 0 })
            return new EvChargePointsResult { Stations = [] };

        return new EvChargePointsResult
        {
            Stations = hereResponse.EvStations.Select(s => new EvStation
            {
                PoolId = s.PoolId,
                Address = s.Address?.Label,
                Position = s.Position is not null
                    ? new LatLngLiteral(s.Position.Lat, s.Position.Lng)
                    : null,
                TotalNumberOfConnectors = s.TotalNumberOfConnectors,
                Connectors = s.Connectors?.Select(c => new EvConnector
                {
                    SupplierName = c.SupplierName,
                    ConnectorTypeName = c.ConnectorType?.Name,
                    ConnectorTypeId = c.ConnectorType?.Id,
                    MaxPowerLevel = c.MaxPowerLevel,
                    ChargeCapacity = c.ChargeCapacity,
                    FixedCable = c.FixedCable
                }).ToList()
            }).ToList()
        };
    }
}
