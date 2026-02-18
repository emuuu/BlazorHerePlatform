using HerePlatform.Core.Coordinates;

namespace HerePlatform.Core.TourPlanning;

/// <summary>
/// Start or end point of a vehicle shift.
/// </summary>
public class TourShiftEnd
{
    /// <summary>
    /// Geographic location.
    /// </summary>
    public LatLngLiteral Location { get; set; }

    /// <summary>
    /// Time in ISO 8601 format.
    /// </summary>
    public string? Time { get; set; }
}
