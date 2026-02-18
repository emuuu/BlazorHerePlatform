namespace HerePlatform.Core.TourPlanning;

/// <summary>
/// An activity at a tour stop (e.g. delivery, pickup, departure, arrival).
/// </summary>
public class TourActivity
{
    /// <summary>
    /// Activity type (e.g. "delivery", "pickup", "departure", "arrival").
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    /// Associated job identifier (null for departure/arrival activities).
    /// </summary>
    public string? JobId { get; set; }
}
