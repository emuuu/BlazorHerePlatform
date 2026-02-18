namespace HerePlatform.Core.Weather;

/// <summary>
/// A weather forecast entry from the HERE Weather API.
/// </summary>
public class WeatherForecast
{
    /// <summary>
    /// Forecast date (e.g. "2025-01-15").
    /// </summary>
    public string? Date { get; set; }

    /// <summary>
    /// High temperature in degrees Celsius.
    /// </summary>
    public double? TemperatureHigh { get; set; }

    /// <summary>
    /// Low temperature in degrees Celsius.
    /// </summary>
    public double? TemperatureLow { get; set; }

    /// <summary>
    /// Human-readable weather description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Weather icon identifier.
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    /// Relative humidity in percent.
    /// </summary>
    public double? Humidity { get; set; }

    /// <summary>
    /// Wind speed in km/h.
    /// </summary>
    public double? WindSpeed { get; set; }

    /// <summary>
    /// Precipitation probability in percent.
    /// </summary>
    public double? PrecipitationProbability { get; set; }

    /// <summary>
    /// Feels-like temperature in degrees Celsius.
    /// </summary>
    public double? Comfort { get; set; }
}
