namespace HerePlatformComponents.Maps.Events;

/// <summary>
/// Event args for map container resize events.
/// </summary>
public class MapResizeEventArgs
{
    /// <summary>
    /// New width of the map container in pixels.
    /// </summary>
    public double Width { get; set; }

    /// <summary>
    /// New height of the map container in pixels.
    /// </summary>
    public double Height { get; set; }
}
