namespace HerePlatform.Core.TourPlanning;

/// <summary>
/// Cost factors for a vehicle type.
/// </summary>
public class TourVehicleCosts
{
    /// <summary>
    /// Fixed cost per vehicle.
    /// </summary>
    public double Fixed { get; set; }

    /// <summary>
    /// Cost per meter traveled.
    /// </summary>
    public double Distance { get; set; }

    /// <summary>
    /// Cost per second of travel time.
    /// </summary>
    public double Time { get; set; }
}
