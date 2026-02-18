namespace HerePlatform.Core.TourPlanning;

/// <summary>
/// Statistics for a tour or overall solution.
/// </summary>
public class TourStatistic
{
    /// <summary>
    /// Total cost.
    /// </summary>
    public double? Cost { get; set; }

    /// <summary>
    /// Total distance in meters.
    /// </summary>
    public double? Distance { get; set; }

    /// <summary>
    /// Total duration in seconds.
    /// </summary>
    public double? Duration { get; set; }
}
