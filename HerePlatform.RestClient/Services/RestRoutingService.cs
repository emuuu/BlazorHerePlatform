using System.Runtime.Serialization;
using System.Text.Json;
using HerePlatform.Core.Coordinates;
using HerePlatform.Core.Routing;
using HerePlatform.Core.Services;
using HerePlatform.Core.Utilities;
using HerePlatform.RestClient.Internal;

namespace HerePlatform.RestClient.Services;

internal sealed class RestRoutingService : IRoutingService
{
    private const string BaseUrl = "https://router.hereapi.com/v8/routes";

    private readonly IHttpClientFactory _httpClientFactory;

    public RestRoutingService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<RoutingResult> CalculateRouteAsync(RoutingRequest request)
    {
        var returnParts = new List<string> { "summary" };
        if (request.ReturnPolyline)
            returnParts.Add("polyline");
        if (request.ReturnInstructions)
            returnParts.Add("turnByTurnActions");

        var parameters = new List<(string key, string? value)>
        {
            ("origin", $"{request.Origin.Lat},{request.Origin.Lng}"),
            ("destination", $"{request.Destination.Lat},{request.Destination.Lng}"),
            ("transportMode", GetEnumMemberValue(request.TransportMode)),
            ("routingMode", GetEnumMemberValue(request.RoutingMode)),
            ("return", string.Join(",", returnParts)),
            ("alternatives", request.Alternatives > 0 ? request.Alternatives.ToString() : null)
        };

        // Via waypoints
        if (request.Via is { Count: > 0 })
        {
            foreach (var via in request.Via)
                parameters.Add(("via", $"{via.Lat},{via.Lng}"));
        }

        // Avoid features
        if (request.Avoid != RoutingAvoidFeature.None)
        {
            var avoidFeatures = GetAvoidFeatures(request.Avoid);
            if (avoidFeatures.Length > 0)
                parameters.Add(("avoid[features]", string.Join(",", avoidFeatures)));
        }

        // Truck options
        if (request.TransportMode == TransportMode.Truck && request.Truck is not null)
            AddTruckParameters(parameters, request.Truck);

        // EV options
        if (request.Ev is not null)
            AddEvParameters(parameters, request.Ev);

        var qs = HereApiHelper.BuildQueryString(parameters.ToArray());
        var url = $"{BaseUrl}?{qs}";

        var client = _httpClientFactory.CreateClient("HereApi");
        var response = await client.GetAsync(url);

        HereApiHelper.EnsureAuthSuccess(response, "routing");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var hereResponse = JsonSerializer.Deserialize<HereRoutingResponse>(json, HereJsonDefaults.Options);

        return MapToResult(hereResponse);
    }

    private static RoutingResult MapToResult(HereRoutingResponse? hereResponse)
    {
        if (hereResponse?.Routes is null or { Count: 0 })
            return new RoutingResult { Routes = [] };

        return new RoutingResult
        {
            Routes = hereResponse.Routes.Select(route => new Route
            {
                Sections = route.Sections?.Select(section =>
                {
                    var rs = new RouteSection
                    {
                        Polyline = section.Polyline,
                        Transport = section.Transport?.Mode,
                        Summary = section.Summary is not null ? new RouteSummary
                        {
                            Duration = section.Summary.Duration,
                            Length = section.Summary.Length,
                            BaseDuration = section.Summary.BaseDuration
                        } : null,
                        TurnByTurnActions = MapActions(section)
                    };

                    // Decode polyline
                    if (!string.IsNullOrEmpty(section.Polyline))
                    {
                        try
                        {
                            rs.DecodedPolyline = FlexiblePolyline.Decode(section.Polyline);
                        }
                        catch
                        {
                            // If decoding fails, leave DecodedPolyline null
                        }
                    }

                    return rs;
                }).ToList()
            }).ToList()
        };
    }

    private static List<TurnInstruction>? MapActions(HereRouteSection section)
    {
        // HERE v8 returns actions in "turnByTurnActions" or "actions" depending on return param
        var actions = section.TurnByTurnActions ?? section.Actions;
        if (actions is null or { Count: 0 })
            return null;

        return actions.Select(a => new TurnInstruction
        {
            Action = a.Action,
            Instruction = a.Instruction,
            Duration = a.Duration,
            Length = a.Length,
            Offset = a.Offset
        }).ToList();
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

    private static void AddTruckParameters(List<(string key, string? value)> parameters, TruckOptions truck)
    {
        if (truck.Height.HasValue)
            parameters.Add(("truck[height]", ((int)(truck.Height.Value * 100)).ToString()));
        if (truck.Width.HasValue)
            parameters.Add(("truck[width]", ((int)(truck.Width.Value * 100)).ToString()));
        if (truck.Length.HasValue)
            parameters.Add(("truck[length]", ((int)(truck.Length.Value * 100)).ToString()));
        if (truck.GrossWeight.HasValue)
            parameters.Add(("truck[grossWeight]", truck.GrossWeight.Value.ToString()));
        if (truck.WeightPerAxle.HasValue)
            parameters.Add(("truck[weightPerAxle]", truck.WeightPerAxle.Value.ToString()));
        if (truck.AxleCount.HasValue)
            parameters.Add(("truck[axleCount]", truck.AxleCount.Value.ToString()));
        if (truck.TrailerCount.HasValue)
            parameters.Add(("truck[trailerCount]", truck.TrailerCount.Value.ToString()));
        if (truck.TunnelCategory.HasValue)
            parameters.Add(("truck[tunnelCategory]", GetEnumMemberValue(truck.TunnelCategory.Value)));
        if (truck.HazardousGoods != HazardousGoods.None)
        {
            var goods = GetHazardousGoods(truck.HazardousGoods);
            if (goods.Length > 0)
                parameters.Add(("truck[shippedHazardousGoods]", string.Join(",", goods)));
        }
    }

    private static void AddEvParameters(List<(string key, string? value)> parameters, EvOptions ev)
    {
        if (ev.InitialCharge.HasValue)
            parameters.Add(("ev[initialCharge]", ev.InitialCharge.Value.ToString()));
        if (ev.MaxCharge.HasValue)
            parameters.Add(("ev[maxCharge]", ev.MaxCharge.Value.ToString()));
        if (ev.MaxChargeAfterChargingStation.HasValue)
            parameters.Add(("ev[maxChargeAfterChargingStation]", ev.MaxChargeAfterChargingStation.Value.ToString()));
        if (ev.MinChargeAtChargingStation.HasValue)
            parameters.Add(("ev[minChargeAtChargingStation]", ev.MinChargeAtChargingStation.Value.ToString()));
        if (ev.MinChargeAtDestination.HasValue)
            parameters.Add(("ev[minChargeAtDestination]", ev.MinChargeAtDestination.Value.ToString()));
        if (ev.ChargingCurve is not null)
            parameters.Add(("ev[chargingCurve]", ev.ChargingCurve));
        if (ev.FreeFlowSpeedTable is not null)
            parameters.Add(("ev[freeFlowSpeedTable]", ev.FreeFlowSpeedTable));
        if (ev.AuxiliaryConsumption.HasValue)
            parameters.Add(("ev[auxiliaryConsumption]", ev.AuxiliaryConsumption.Value.ToString()));
    }

    private static string[] GetHazardousGoods(HazardousGoods goods)
    {
        var result = new List<string>();
        if (goods.HasFlag(HazardousGoods.Explosive)) result.Add("explosive");
        if (goods.HasFlag(HazardousGoods.Gas)) result.Add("gas");
        if (goods.HasFlag(HazardousGoods.Flammable)) result.Add("flammable");
        if (goods.HasFlag(HazardousGoods.Combustible)) result.Add("combustible");
        if (goods.HasFlag(HazardousGoods.Organic)) result.Add("organic");
        if (goods.HasFlag(HazardousGoods.Poison)) result.Add("poison");
        if (goods.HasFlag(HazardousGoods.RadioActive)) result.Add("radioActive");
        if (goods.HasFlag(HazardousGoods.Corrosive)) result.Add("corrosive");
        if (goods.HasFlag(HazardousGoods.PoisonousInhalation)) result.Add("poisonousInhalation");
        if (goods.HasFlag(HazardousGoods.HarmfulToWater)) result.Add("harmfulToWater");
        if (goods.HasFlag(HazardousGoods.Other)) result.Add("other");
        return result.ToArray();
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
