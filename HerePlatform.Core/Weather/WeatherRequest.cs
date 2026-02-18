using HerePlatform.Core.Coordinates;

namespace HerePlatform.Core.Weather;

/// <summary>
/// Request parameters for the HERE Destination Weather API.
/// </summary>
public class WeatherRequest
{
    /// <summary>
    /// Location to get weather for (latitude, longitude).
    /// </summary>
    public LatLngLiteral Location { get; set; }

    /// <summary>
    /// Weather products to request.
    /// </summary>
    public List<WeatherProduct>? Products { get; set; }

    /// <summary>
    /// Language for weather descriptions (BCP 47, e.g. "de", "en").
    /// </summary>
    public string? Lang { get; set; }
}
