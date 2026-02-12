namespace HerePlatformComponents.Maps.Events;

/// <summary>
/// Event data for engine state change events.
/// </summary>
public class EngineStateChangeEventArgs
{
    /// <summary>
    /// The engine state value.
    /// </summary>
    public int State { get; set; }

    /// <summary>
    /// The HERE event type string.
    /// </summary>
    public string? Type { get; set; }
}
