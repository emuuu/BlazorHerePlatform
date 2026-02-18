using System.Collections.Generic;

namespace HerePlatform.Core.TourPlanning;

/// <summary>
/// A plan containing the list of jobs.
/// </summary>
public class TourPlan
{
    /// <summary>
    /// Jobs to schedule across the fleet.
    /// </summary>
    public List<TourJob>? Jobs { get; set; }
}
