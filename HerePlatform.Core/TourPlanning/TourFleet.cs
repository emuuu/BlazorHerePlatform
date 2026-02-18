using System.Collections.Generic;

namespace HerePlatform.Core.TourPlanning;

/// <summary>
/// The fleet definition for tour planning.
/// </summary>
public class TourFleet
{
    /// <summary>
    /// Vehicle types available in the fleet.
    /// </summary>
    public List<TourVehicleType>? Types { get; set; }
}
