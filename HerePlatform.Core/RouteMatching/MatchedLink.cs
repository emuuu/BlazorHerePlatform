using HerePlatform.Core.Coordinates;

namespace HerePlatform.Core.RouteMatching;

/// <summary>
/// A road segment matched to a GPS trace by the HERE Route Matching API.
/// </summary>
public class MatchedLink
{
    /// <summary>
    /// HERE link ID for the matched road segment.
    /// </summary>
    public string? LinkId { get; set; }

    /// <summary>
    /// Confidence of the match (0.0 to 1.0).
    /// </summary>
    public double? Confidence { get; set; }

    /// <summary>
    /// Speed limit on the road segment in km/h, if available.
    /// </summary>
    public double? SpeedLimit { get; set; }

    /// <summary>
    /// Functional class of the road (1-5, where 1 = major highway).
    /// </summary>
    public int? FunctionalClass { get; set; }

    /// <summary>
    /// Geometry of the matched road segment.
    /// </summary>
    public List<LatLngLiteral>? Geometry { get; set; }
}
