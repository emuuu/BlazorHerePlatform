using System.Collections.Generic;

namespace HerePlatform.Core.TourPlanning;

/// <summary>
/// A type of vehicle in the fleet.
/// </summary>
public class TourVehicleType
{
    /// <summary>
    /// Unique vehicle type identifier.
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// Routing profile (e.g. "car", "truck").
    /// </summary>
    public string? Profile { get; set; }

    /// <summary>
    /// Cost factors for this vehicle type.
    /// </summary>
    public TourVehicleCosts? Costs { get; set; }

    /// <summary>
    /// Work shifts defining start/end locations and times.
    /// </summary>
    public List<TourVehicleShift>? Shifts { get; set; }

    /// <summary>
    /// Cargo capacity dimensions.
    /// </summary>
    public List<int>? Capacity { get; set; }

    /// <summary>
    /// Number of vehicles of this type.
    /// </summary>
    public int Amount { get; set; } = 1;
}
