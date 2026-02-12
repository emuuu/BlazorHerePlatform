namespace HerePlatformComponents.Maps.Events;

/// <summary>
/// Event data for generic state change events (InfoBubble open/close, Icon load state, etc.).
/// </summary>
public class StateChangeEventArgs
{
    /// <summary>
    /// The state value.
    /// </summary>
    public string? State { get; set; }

    /// <summary>
    /// The HERE event type string.
    /// </summary>
    public string? Type { get; set; }
}
