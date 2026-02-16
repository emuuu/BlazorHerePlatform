using HerePlatform.Core.Coordinates;
using System.Collections.Generic;

namespace HerePlatform.Core.Isoline;

/// <summary>
/// Result of an isoline calculation.
/// </summary>
public class IsolineResult
{
    /// <summary>
    /// Calculated isolines.
    /// </summary>
    public List<IsolinePolygon>? Isolines { get; set; }
}

/// <summary>
/// A single isoline polygon.
/// </summary>
public class IsolinePolygon
{
    /// <summary>
    /// Range value for this isoline.
    /// </summary>
    public int Range { get; set; }

    /// <summary>
    /// Decoded polygon coordinates.
    /// </summary>
    public List<LatLngLiteral>? Polygon { get; set; }

    /// <summary>
    /// Encoded flexible polyline (for C#-side decoding if JS decoding failed).
    /// </summary>
    public string? EncodedPolyline { get; set; }
}
