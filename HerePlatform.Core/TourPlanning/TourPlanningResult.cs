using System.Collections.Generic;

namespace HerePlatform.Core.TourPlanning;

/// <summary>
/// Result of a tour planning optimization.
/// </summary>
public class TourPlanningResult
{
    /// <summary>
    /// Optimized tours for the fleet.
    /// </summary>
    public List<Tour>? Tours { get; set; }

    /// <summary>
    /// Job IDs that could not be assigned to any vehicle.
    /// </summary>
    public List<string>? UnassignedJobs { get; set; }

    /// <summary>
    /// Overall statistics for the solution.
    /// </summary>
    public TourStatistic? Statistic { get; set; }
}
