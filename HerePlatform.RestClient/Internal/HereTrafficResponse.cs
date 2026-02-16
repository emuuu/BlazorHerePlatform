namespace HerePlatform.RestClient.Internal;

/// <summary>
/// Internal DTOs matching HERE Traffic v7 response structure.
/// </summary>
internal sealed class HereTrafficIncidentsResponse
{
    public List<HereTrafficIncidentResult>? Results { get; set; }
}

internal sealed class HereTrafficIncidentResult
{
    public HereTrafficLocation? Location { get; set; }
    public HereIncidentDetails? IncidentDetails { get; set; }
}

internal sealed class HereIncidentDetails
{
    public string? Type { get; set; }
    public string? Criticality { get; set; }
    public string? Description { get; set; }
    public string? StartTime { get; set; }
    public string? EndTime { get; set; }
}

internal sealed class HereTrafficLocation
{
    public string? Description { get; set; }
    public HerePosition? Shape { get; set; }
    public List<HerePosition>? Links { get; set; }
}

// --- Traffic Flow ---

internal sealed class HereTrafficFlowResponse
{
    public List<HereTrafficFlowResult>? Results { get; set; }
}

internal sealed class HereTrafficFlowResult
{
    public HereTrafficLocation? Location { get; set; }
    public HereCurrentFlow? CurrentFlow { get; set; }
}

internal sealed class HereCurrentFlow
{
    public double Speed { get; set; }
    public double FreeFlow { get; set; }
    public double JamFactor { get; set; }
}
