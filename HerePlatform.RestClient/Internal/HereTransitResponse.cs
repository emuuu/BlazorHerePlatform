namespace HerePlatform.RestClient.Internal;

/// <summary>
/// Internal DTOs matching HERE Public Transit v8 response structure.
/// </summary>
internal sealed class HereTransitDeparturesResponse
{
    public List<HereTransitBoard>? Boards { get; set; }
}

internal sealed class HereTransitBoard
{
    public HereTransitPlace? Place { get; set; }
    public List<HereTransitDeparture>? Departures { get; set; }
}

internal sealed class HereTransitPlace
{
    public string? Name { get; set; }
    public HerePosition? Location { get; set; }
}

internal sealed class HereTransitDeparture
{
    public string? Time { get; set; }
    public string? Headsign { get; set; }
    public HereTransitTransport? Transport { get; set; }
}

internal sealed class HereTransitTransport
{
    public string? Name { get; set; }
    public string? Mode { get; set; }
}

// --- Stations ---

internal sealed class HereTransitStationsResponse
{
    public List<HereTransitStationResult>? Stations { get; set; }
}

internal sealed class HereTransitStationResult
{
    public HereTransitPlace? Place { get; set; }
    public double Distance { get; set; }
    public List<HereTransitTransport>? Transports { get; set; }
}
