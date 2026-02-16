using System.Runtime.Serialization;
using System.Text.Json;
using HerePlatform.Core.Coordinates;
using HerePlatform.Core.Isoline;
using HerePlatform.Core.Routing;
using HerePlatform.Core.Services;
using HerePlatform.Core.Utilities;
using HerePlatform.RestClient.Internal;

namespace HerePlatform.RestClient.Services;

internal sealed class RestIsolineService : IIsolineService
{
    private const string BaseUrl = "https://isoline.router.hereapi.com/v8/isolines";

    private readonly IHttpClientFactory _httpClientFactory;

    public RestIsolineService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IsolineResult> CalculateIsolineAsync(IsolineRequest request)
    {
        var rangeValues = request.Ranges is { Count: > 0 }
            ? string.Join(",", request.Ranges)
            : null;

        var parameters = new List<(string key, string? value)>
        {
            ("origin", $"{request.Center.Lat},{request.Center.Lng}"),
            ("range[values]", rangeValues),
            ("range[type]", GetEnumMemberValue(request.RangeType)),
            ("transportMode", GetEnumMemberValue(request.TransportMode)),
            ("routingMode", GetEnumMemberValue(request.RoutingMode)),
            ("departureTime", request.DepartureTime)
        };

        if (request.Avoid != RoutingAvoidFeature.None)
        {
            var avoidFeatures = GetAvoidFeatures(request.Avoid);
            if (avoidFeatures.Length > 0)
                parameters.Add(("avoid[features]", string.Join(",", avoidFeatures)));
        }

        var qs = HereApiHelper.BuildQueryString(parameters.ToArray());
        var url = $"{BaseUrl}?{qs}";

        var client = _httpClientFactory.CreateClient("HereApi");
        var response = await client.GetAsync(url);

        HereApiHelper.EnsureAuthSuccess(response, "isoline");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var hereResponse = JsonSerializer.Deserialize<HereIsolineResponse>(json, HereJsonDefaults.Options);

        return MapToResult(hereResponse);
    }

    private static IsolineResult MapToResult(HereIsolineResponse? hereResponse)
    {
        if (hereResponse?.Isolines is null or { Count: 0 })
            return new IsolineResult { Isolines = [] };

        return new IsolineResult
        {
            Isolines = hereResponse.Isolines.Select(iso =>
            {
                var polygon = new IsolinePolygon
                {
                    Range = iso.Range?.Value ?? 0
                };

                // Take the first polygon's outer ring
                var encoded = iso.Polygons?.FirstOrDefault()?.Outer;
                if (!string.IsNullOrEmpty(encoded))
                {
                    polygon.EncodedPolyline = encoded;
                    try
                    {
                        polygon.Polygon = FlexiblePolyline.Decode(encoded);
                    }
                    catch
                    {
                        // Leave Polygon null if decoding fails
                    }
                }

                return polygon;
            }).ToList()
        };
    }

    private static string[] GetAvoidFeatures(RoutingAvoidFeature avoid)
    {
        var features = new List<string>();
        if (avoid.HasFlag(RoutingAvoidFeature.Tolls)) features.Add("tollRoad");
        if (avoid.HasFlag(RoutingAvoidFeature.Highways)) features.Add("controlledAccessHighway");
        if (avoid.HasFlag(RoutingAvoidFeature.Ferries)) features.Add("ferry");
        if (avoid.HasFlag(RoutingAvoidFeature.Tunnels)) features.Add("tunnel");
        return features.ToArray();
    }

    private static string GetEnumMemberValue<T>(T value) where T : struct, Enum
    {
        var member = typeof(T).GetMember(value.ToString()!)[0];
        var attr = member.GetCustomAttributes(typeof(EnumMemberAttribute), false)
            .Cast<EnumMemberAttribute>()
            .FirstOrDefault();
        return attr?.Value ?? value.ToString()!.ToLowerInvariant();
    }
}
