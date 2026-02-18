namespace HerePlatform.RestClient.Internal;

/// <summary>
/// Internal DTOs matching HERE Route Matching API v8 response structure.
/// </summary>
internal sealed class HereRouteMatchingResponse
{
    public List<HereMatchedLink>? RouteLinks { get; set; }
    public List<string>? Warnings { get; set; }
}

internal sealed class HereMatchedLink
{
    public string? LinkId { get; set; }
    public double? Confidence { get; set; }
    public double? SpeedLimit { get; set; }
    public int? FunctionalClass { get; set; }
    public string? Shape { get; set; }
}
