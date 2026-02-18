using System.Text.Json;
using HerePlatform.Core.Coordinates;
using HerePlatform.Core.IntermodalRouting;
using HerePlatform.Core.Services;
using HerePlatform.RestClient.Internal;

namespace HerePlatform.RestClient.Services;

internal sealed class RestIntermodalRoutingService : IIntermodalRoutingService
{
    private const string BaseUrl = "https://intermodal.router.hereapi.com/v8/routes";

    private readonly IHttpClientFactory _httpClientFactory;

    public RestIntermodalRoutingService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IntermodalRoutingResult> CalculateRouteAsync(IntermodalRoutingRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var returnParts = new List<string>();
        if (request.ReturnPolyline)
            returnParts.Add("polyline");
        if (request.ReturnTravelSummary)
            returnParts.Add("travelSummary");
        if (request.ReturnActions)
            returnParts.Add("actions");

        var qs = HereApiHelper.BuildQueryString(
            ("origin", HereApiHelper.FormatCoord(request.Origin)),
            ("destination", HereApiHelper.FormatCoord(request.Destination)),
            ("departAt", request.DepartAt),
            ("arriveAt", request.ArriveAt),
            ("alternatives", request.Alternatives > 0 ? request.Alternatives.ToString() : null),
            ("return", returnParts.Count > 0 ? string.Join(",", returnParts) : null),
            ("lang", request.Lang));

        var url = $"{BaseUrl}?{qs}";

        var client = _httpClientFactory.CreateClient(HereApiHelper.ClientName);
        using var response = await client.GetAsync(url, cancellationToken).ConfigureAwait(false);

        await HereApiHelper.EnsureSuccessOrThrowAsync(response, "intermodalRouting", cancellationToken).ConfigureAwait(false);

        var json = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        var hereResponse = JsonSerializer.Deserialize<HereIntermodalRoutingResponse>(json, HereJsonDefaults.Options);

        return MapToResult(hereResponse);
    }

    private static IntermodalRoutingResult MapToResult(HereIntermodalRoutingResponse? hereResponse)
    {
        if (hereResponse?.Routes is null or { Count: 0 })
            return new IntermodalRoutingResult { Routes = [] };

        return new IntermodalRoutingResult
        {
            Routes = hereResponse.Routes.Select(route => new IntermodalRoute
            {
                Sections = route.Sections?.Select(MapSection).ToList()
            }).ToList()
        };
    }

    private static IntermodalSection MapSection(HereIntermodalSection section)
    {
        var result = new IntermodalSection
        {
            Type = section.Type,
            Polyline = section.Polyline,
            Departure = MapPlace(section.Departure),
            Arrival = MapPlace(section.Arrival),
            Summary = section.TravelSummary is not null ? new IntermodalSummary
            {
                Duration = section.TravelSummary.Duration,
                Length = section.TravelSummary.Length
            } : null,
            Transport = section.Transport is not null ? new IntermodalTransport
            {
                Mode = section.Transport.Mode,
                Name = section.Transport.Name,
                Headsign = section.Transport.Headsign,
                ShortName = section.Transport.ShortName,
                Color = section.Transport.Color
            } : null
        };

        result.Geometry = HereApiHelper.DecodeShapeSafe(section.Polyline);

        return result;
    }

    private static IntermodalPlace? MapPlace(HereIntermodalPlaceDto? place)
    {
        if (place is null)
            return null;

        return new IntermodalPlace
        {
            Name = place.Name,
            Position = place.Location is not null
                ? new LatLngLiteral(place.Location.Lat, place.Location.Lng)
                : null,
            Time = place.Time
        };
    }
}
