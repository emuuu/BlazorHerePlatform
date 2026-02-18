using System.Globalization;
using System.Text;
using System.Text.Json;
using HerePlatform.Core.Coordinates;
using HerePlatform.Core.RouteMatching;
using HerePlatform.Core.Routing;
using HerePlatform.Core.Services;
using HerePlatform.RestClient.Internal;

namespace HerePlatform.RestClient.Services;

internal sealed class RestRouteMatchingService : IRouteMatchingService
{
    private const string BaseUrl = "https://routematching.hereapi.com/v8/match/routelinks";

    private readonly IHttpClientFactory _httpClientFactory;

    public RestRouteMatchingService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<RouteMatchingResult> MatchRouteAsync(RouteMatchingRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var mode = HereApiHelper.MapTransportMode(request.TransportMode);

        var qs = HereApiHelper.BuildQueryString(
            ("mode", mode),
            ("routeMatch", "1"));

        var url = $"{BaseUrl}?{qs}";

        var body = BuildTraceBody(request.TracePoints);

        var client = _httpClientFactory.CreateClient(HereApiHelper.ClientName);
        using var content = new StringContent(body, Encoding.UTF8, "text/csv");
        using var response = await client.PostAsync(url, content, cancellationToken).ConfigureAwait(false);

        await HereApiHelper.EnsureSuccessOrThrowAsync(response, "route-matching", cancellationToken).ConfigureAwait(false);

        var json = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        var hereResponse = JsonSerializer.Deserialize<HereRouteMatchingResponse>(json, HereJsonDefaults.Options);

        return MapToResult(hereResponse);
    }

    private static string BuildTraceBody(List<TracePoint>? tracePoints)
    {
        if (tracePoints is null or { Count: 0 })
            return string.Empty;

        var sb = new StringBuilder(tracePoints.Count * 64);
        for (var i = 0; i < tracePoints.Count; i++)
        {
            var point = tracePoints[i];
            if (i > 0) sb.Append('\n');

            sb.Append(point.Position.Lat.ToString(CultureInfo.InvariantCulture))
              .Append(',')
              .Append(point.Position.Lng.ToString(CultureInfo.InvariantCulture))
              .Append(',')
              .Append(point.Timestamp.ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture))
              .Append(',')
              .Append(point.Heading?.ToString(CultureInfo.InvariantCulture) ?? "")
              .Append(',')
              .Append(point.Speed?.ToString(CultureInfo.InvariantCulture) ?? "");
        }

        return sb.ToString();
    }

    private static RouteMatchingResult MapToResult(HereRouteMatchingResponse? hereResponse)
    {
        var result = new RouteMatchingResult
        {
            Warnings = hereResponse?.Warnings
        };

        if (hereResponse?.RouteLinks is null or { Count: 0 })
        {
            result.MatchedLinks = [];
            return result;
        }

        result.MatchedLinks = hereResponse.RouteLinks.Select(link => new MatchedLink
        {
            LinkId = link.LinkId,
            Confidence = link.Confidence,
            SpeedLimit = link.SpeedLimit,
            FunctionalClass = link.FunctionalClass,
            Geometry = HereApiHelper.DecodeShapeSafe(link.Shape)
        }).ToList();

        return result;
    }
}
