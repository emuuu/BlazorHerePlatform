using System.Collections.Generic;
using HerePlatform.Core.Coordinates;

namespace HerePlatform.Core.TourPlanning;

/// <summary>
/// A single delivery or pickup location with constraints.
/// </summary>
public class TourJobPlace
{
    /// <summary>
    /// Geographic location of the job place.
    /// </summary>
    public LatLngLiteral Location { get; set; }

    /// <summary>
    /// Service duration in seconds at this location.
    /// </summary>
    public int Duration { get; set; }

    /// <summary>
    /// Optional time windows as pairs of ISO 8601 strings [start, end].
    /// </summary>
    public List<List<string>>? Times { get; set; }
}
