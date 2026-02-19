using HerePlatform.Core.Coordinates;
using HerePlatform.Blazor.Maps.Coordinates;

namespace HerePlatform.Blazor.Maps;

public class RectOptions : ListableEntityOptionsBase
{
    /// <summary>
    /// Bounding box for the rectangle.
    /// </summary>
    public GeoRect? Bounds { get; set; }

    /// <summary>
    /// Visual style for the rectangle.
    /// </summary>
    public StyleOptions? Style { get; set; }

    /// <summary>
    /// Extrusion height in meters (3D rendering).
    /// </summary>
    public double? Extrusion { get; set; }

    /// <summary>
    /// Elevation in meters (3D rendering).
    /// </summary>
    public double? Elevation { get; set; }
}
