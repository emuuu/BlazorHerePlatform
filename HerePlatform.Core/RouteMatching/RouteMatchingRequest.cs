using HerePlatform.Core.Routing;

namespace HerePlatform.Core.RouteMatching;

/// <summary>
/// Request for matching a GPS trace to road segments via the HERE Route Matching API.
/// </summary>
public class RouteMatchingRequest
{
    /// <summary>
    /// GPS trace points to match against the road network.
    /// </summary>
    public List<TracePoint>? TracePoints { get; set; }

    /// <summary>
    /// Transport mode for route matching.
    /// </summary>
    public TransportMode TransportMode { get; set; } = TransportMode.Car;
}
