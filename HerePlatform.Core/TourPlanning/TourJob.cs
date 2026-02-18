namespace HerePlatform.Core.TourPlanning;

/// <summary>
/// A job representing work to be done at one or more places.
/// </summary>
public class TourJob
{
    /// <summary>
    /// Unique job identifier.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Places where deliveries or pickups occur.
    /// </summary>
    public TourJobPlaces? Places { get; set; }
}
