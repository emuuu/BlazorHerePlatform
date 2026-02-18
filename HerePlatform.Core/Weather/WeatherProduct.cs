using HerePlatform.Core.Serialization;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace HerePlatform.Core.Weather;

/// <summary>
/// Weather product types available from the HERE Destination Weather API.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverterEx<WeatherProduct>))]
public enum WeatherProduct
{
    /// <summary>
    /// Current weather observation.
    /// </summary>
    [EnumMember(Value = "observation")]
    Observation,

    /// <summary>
    /// 7-day detailed forecast.
    /// </summary>
    [EnumMember(Value = "forecast7days")]
    Forecast7Days,

    /// <summary>
    /// 7-day simple forecast.
    /// </summary>
    [EnumMember(Value = "forecast7daysSimple")]
    Forecast7DaysSimple,

    /// <summary>
    /// Hourly forecast.
    /// </summary>
    [EnumMember(Value = "forecastHourly")]
    ForecastHourly,

    /// <summary>
    /// Astronomy forecast (sunrise, sunset, moonrise, moonset).
    /// </summary>
    [EnumMember(Value = "forecastAstronomy")]
    ForecastAstronomy
}
