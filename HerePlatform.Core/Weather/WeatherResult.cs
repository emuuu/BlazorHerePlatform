namespace HerePlatform.Core.Weather;

/// <summary>
/// Result from the HERE Destination Weather API.
/// </summary>
public class WeatherResult
{
    /// <summary>
    /// Current weather observations (when <see cref="WeatherProduct.Observation"/> was requested).
    /// </summary>
    public List<WeatherObservation>? Observations { get; set; }

    /// <summary>
    /// Forecast entries (when a forecast product was requested).
    /// </summary>
    public List<WeatherForecast>? Forecasts { get; set; }
}
