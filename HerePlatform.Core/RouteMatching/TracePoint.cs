using HerePlatform.Core.Coordinates;

namespace HerePlatform.Core.RouteMatching;

/// <summary>
/// A GPS trace point with timestamp and optional heading/speed.
/// </summary>
public class TracePoint
{
    /// <summary>
    /// Geographic position of the GPS reading.
    /// </summary>
    public LatLngLiteral Position { get; set; }

    /// <summary>
    /// Timestamp of the GPS reading.
    /// </summary>
    public DateTimeOffset Timestamp { get; set; }

    /// <summary>
    /// Heading in degrees (0-360), if available.
    /// </summary>
    public double? Heading { get; set; }

    /// <summary>
    /// Speed in m/s, if available.
    /// </summary>
    public double? Speed { get; set; }
}
