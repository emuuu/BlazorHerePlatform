using System.Collections.Generic;
using HerePlatform.Core.Coordinates;

namespace HerePlatform.Core.TourPlanning;

/// <summary>
/// A stop within a tour.
/// </summary>
public class TourStop
{
    /// <summary>
    /// Geographic location of the stop.
    /// </summary>
    public LatLngLiteral? Location { get; set; }

    /// <summary>
    /// Activities performed at this stop.
    /// </summary>
    public List<TourActivity>? Activities { get; set; }
}
