namespace HerePlatform.Core.Weather;

/// <summary>
/// A current weather observation from the HERE Weather API.
/// </summary>
public class WeatherObservation
{
    /// <summary>
    /// Temperature in degrees Celsius.
    /// </summary>
    public double? Temperature { get; set; }

    /// <summary>
    /// Relative humidity in percent.
    /// </summary>
    public double? Humidity { get; set; }

    /// <summary>
    /// Wind speed in km/h.
    /// </summary>
    public double? WindSpeed { get; set; }

    /// <summary>
    /// Wind direction as compass text (e.g. "NW").
    /// </summary>
    public string? WindDirection { get; set; }

    /// <summary>
    /// Human-readable weather description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Weather icon identifier.
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    /// Observation timestamp in UTC.
    /// </summary>
    public string? Timestamp { get; set; }

    /// <summary>
    /// Dew point temperature in degrees Celsius.
    /// </summary>
    public double? DewPoint { get; set; }

    /// <summary>
    /// Feels-like temperature in degrees Celsius.
    /// </summary>
    public double? Comfort { get; set; }

    /// <summary>
    /// Barometric pressure in mbar.
    /// </summary>
    public double? BarometerPressure { get; set; }

    /// <summary>
    /// Visibility in km.
    /// </summary>
    public double? Visibility { get; set; }

    /// <summary>
    /// UV index (0-11+).
    /// </summary>
    public int? UvIndex { get; set; }

    /// <summary>
    /// Precipitation probability in percent.
    /// </summary>
    public double? PrecipitationProbability { get; set; }
}
