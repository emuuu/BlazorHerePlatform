using System.Collections.Generic;

namespace HerePlatform.Core.TourPlanning;

/// <summary>
/// An optimized tour for a single vehicle.
/// </summary>
public class Tour
{
    /// <summary>
    /// Identifier of the vehicle assigned to this tour.
    /// </summary>
    public string? VehicleId { get; set; }

    /// <summary>
    /// Ordered stops in the tour.
    /// </summary>
    public List<TourStop>? Stops { get; set; }

    /// <summary>
    /// Statistics for this tour.
    /// </summary>
    public TourStatistic? Statistic { get; set; }
}
