using System.Text.Json.Serialization;

namespace HerePlatform.RestClient.Internal;

/// <summary>
/// Internal DTOs matching HERE Destination Weather API v3 response structure.
/// The real API has observations/forecasts at top-level (no "places" wrapper).
/// </summary>
internal sealed class HereWeatherResponse
{
    public List<HereWeatherObservationPlace>? Observations { get; set; }

    [JsonPropertyName("dailyForecasts")]
    public List<HereWeatherForecastPlace>? DailyForecasts { get; set; }

    [JsonPropertyName("extendedDailyForecasts")]
    public List<HereWeatherForecastPlace>? ExtendedDailyForecasts { get; set; }

    [JsonPropertyName("hourlyForecasts")]
    public List<HereWeatherForecastPlace>? HourlyForecasts { get; set; }
}

internal sealed class HereWeatherObservationPlace
{
    public double? Temperature { get; set; }
    public double? Humidity { get; set; }
    public double? WindSpeed { get; set; }
    public string? WindDescShort { get; set; }
    public string? Description { get; set; }
    public string? IconName { get; set; }
    public string? UtcTime { get; set; }
    public double? DewPoint { get; set; }
    public double? Comfort { get; set; }
    public double? BarometerPressure { get; set; }
    public double? Visibility { get; set; }
    public int? UvIndex { get; set; }
    public double? PrecipitationProbability { get; set; }
}

internal sealed class HereWeatherForecastPlace
{
    public List<HereWeatherForecastDay>? Forecasts { get; set; }
}

internal sealed class HereWeatherForecastDay
{
    public string? UtcTime { get; set; }
    public double? HighTemperature { get; set; }
    public double? LowTemperature { get; set; }
    public double? Temperature { get; set; }
    public string? Description { get; set; }
    public string? IconName { get; set; }
    public double? Humidity { get; set; }
    public double? WindSpeed { get; set; }
    public double? PrecipitationProbability { get; set; }
    public double? Comfort { get; set; }
}
