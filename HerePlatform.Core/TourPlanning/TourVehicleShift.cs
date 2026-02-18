namespace HerePlatform.Core.TourPlanning;

/// <summary>
/// A work shift for a vehicle.
/// </summary>
public class TourVehicleShift
{
    /// <summary>
    /// Shift start location and time.
    /// </summary>
    public TourShiftEnd? Start { get; set; }

    /// <summary>
    /// Shift end location and time.
    /// </summary>
    public TourShiftEnd? End { get; set; }
}
