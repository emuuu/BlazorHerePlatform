namespace HerePlatformComponents.Maps.Events;

/// <summary>
/// Event data for wheel (scroll) events on the map.
/// </summary>
public class MapWheelEventArgs
{
    /// <summary>
    /// Scroll delta value.
    /// </summary>
    public double Delta { get; set; }

    /// <summary>
    /// X coordinate in the map viewport (pixels).
    /// </summary>
    public double ViewportX { get; set; }

    /// <summary>
    /// Y coordinate in the map viewport (pixels).
    /// </summary>
    public double ViewportY { get; set; }
}
