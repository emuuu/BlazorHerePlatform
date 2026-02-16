namespace HerePlatform.RestClient.Internal;

/// <summary>
/// Internal DTOs matching HERE Routing v8 response structure.
/// </summary>
internal sealed class HereRoutingResponse
{
    public List<HereRoute>? Routes { get; set; }
}

internal sealed class HereRoute
{
    public List<HereRouteSection>? Sections { get; set; }
}

internal sealed class HereRouteSection
{
    public string? Polyline { get; set; }
    public HereRouteSummary? Summary { get; set; }
    public HereTransport? Transport { get; set; }
    public List<HereRouteAction>? TurnByTurnActions { get; set; }
    public List<HereRouteAction>? Actions { get; set; }
}

internal sealed class HereRouteSummary
{
    public int Duration { get; set; }
    public int Length { get; set; }
    public int? BaseDuration { get; set; }
}

internal sealed class HereTransport
{
    public string? Mode { get; set; }
}

internal sealed class HereRouteAction
{
    public string? Action { get; set; }
    public string? Instruction { get; set; }
    public int Duration { get; set; }
    public int Length { get; set; }
    public int Offset { get; set; }
}
