using HerePlatformComponents.Maps.Services.MatrixRouting;
using System.Threading.Tasks;

namespace HerePlatformComponents.Maps.Services;

/// <summary>
/// Service for HERE Matrix Routing API v8.
/// </summary>
public interface IMatrixRoutingService
{
    /// <summary>
    /// Calculate a routing matrix between multiple origins and destinations.
    /// </summary>
    Task<MatrixRoutingResult> CalculateMatrixAsync(MatrixRoutingRequest request);
}
