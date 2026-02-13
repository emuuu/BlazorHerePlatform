using System.Collections.Generic;

namespace HerePlatformComponents.Maps.Services.MatrixRouting;

/// <summary>
/// Result of a matrix routing calculation.
/// </summary>
public class MatrixRoutingResult
{
    /// <summary>
    /// Number of origins in the matrix.
    /// </summary>
    public int NumOrigins { get; set; }

    /// <summary>
    /// Number of destinations in the matrix.
    /// </summary>
    public int NumDestinations { get; set; }

    /// <summary>
    /// Matrix entries containing travel times and distances.
    /// </summary>
    public List<MatrixEntry> Matrix { get; set; } = [];
}

/// <summary>
/// A single entry in the routing matrix.
/// </summary>
public class MatrixEntry
{
    /// <summary>
    /// Index of the origin in the request.
    /// </summary>
    public int OriginIndex { get; set; }

    /// <summary>
    /// Index of the destination in the request.
    /// </summary>
    public int DestinationIndex { get; set; }

    /// <summary>
    /// Travel duration in seconds.
    /// </summary>
    public int Duration { get; set; }

    /// <summary>
    /// Travel distance in meters.
    /// </summary>
    public int Length { get; set; }
}
