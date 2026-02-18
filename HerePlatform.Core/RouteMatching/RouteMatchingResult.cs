namespace HerePlatform.Core.RouteMatching;

/// <summary>
/// Result from the HERE Route Matching API.
/// </summary>
public class RouteMatchingResult
{
    /// <summary>
    /// Matched road segments.
    /// </summary>
    public List<MatchedLink>? MatchedLinks { get; set; }

    /// <summary>
    /// Warnings or informational messages from the matching process.
    /// </summary>
    public List<string>? Warnings { get; set; }
}
