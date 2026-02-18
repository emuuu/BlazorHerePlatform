using System.Collections.Generic;

namespace HerePlatform.Core.TourPlanning;

/// <summary>
/// Delivery and pickup locations for a job.
/// </summary>
public class TourJobPlaces
{
    /// <summary>
    /// Delivery locations.
    /// </summary>
    public List<TourJobPlace>? Deliveries { get; set; }

    /// <summary>
    /// Pickup locations.
    /// </summary>
    public List<TourJobPlace>? Pickups { get; set; }
}
