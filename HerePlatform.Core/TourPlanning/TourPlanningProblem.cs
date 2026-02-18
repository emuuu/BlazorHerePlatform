namespace HerePlatform.Core.TourPlanning;

/// <summary>
/// A tour planning problem to optimize vehicle routes for deliveries and pickups.
/// </summary>
public class TourPlanningProblem
{
    /// <summary>
    /// The plan containing jobs to be fulfilled.
    /// </summary>
    public TourPlan? Plan { get; set; }

    /// <summary>
    /// The fleet of vehicles available.
    /// </summary>
    public TourFleet? Fleet { get; set; }
}
