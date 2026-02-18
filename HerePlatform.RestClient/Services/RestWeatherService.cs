using System.Text.Json;
using HerePlatform.Core.Services;
using HerePlatform.Core.Weather;
using HerePlatform.RestClient.Internal;

namespace HerePlatform.RestClient.Services;

internal sealed class RestWeatherService : IWeatherService
{
    private const string BaseUrl = "https://weather.hereapi.com/v3/report";

    private readonly IHttpClientFactory _httpClientFactory;

    public RestWeatherService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<WeatherResult> GetWeatherAsync(WeatherRequest request)
    {
        var products = request.Products is { Count: > 0 }
            ? string.Join(",", request.Products.Select(p => HereApiHelper.GetEnumMemberValue(p)))
            : "observation";

        var qs = HereApiHelper.BuildQueryString(
            ("products", products),
            ("location", HereApiHelper.FormatCoord(request.Location)),
            ("lang", request.Lang));

        var url = $"{BaseUrl}?{qs}";

        var client = _httpClientFactory.CreateClient("HereApi");
        using var response = await client.GetAsync(url).ConfigureAwait(false);

        HereApiHelper.EnsureAuthSuccess(response, "weather");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        var hereResponse = JsonSerializer.Deserialize<HereWeatherResponse>(json, HereJsonDefaults.Options);

        return MapToResult(hereResponse);
    }

    private static WeatherResult MapToResult(HereWeatherResponse? hereResponse)
    {
        var result = new WeatherResult
        {
            Observations = [],
            Forecasts = []
        };

        // Map observations (flat: each element IS the observation with place data)
        if (hereResponse?.Observations is { Count: > 0 })
        {
            result.Observations = hereResponse.Observations.Select(o => new WeatherObservation
            {
                Temperature = o.Temperature,
                Humidity = o.Humidity,
                WindSpeed = o.WindSpeed,
                WindDirection = o.WindDescShort,
                Description = o.Description,
                Icon = o.IconName,
                Timestamp = o.UtcTime,
                DewPoint = o.DewPoint,
                Comfort = o.Comfort,
                BarometerPressure = o.BarometerPressure,
                Visibility = o.Visibility,
                UvIndex = o.UvIndex,
                PrecipitationProbability = o.PrecipitationProbability
            }).ToList();
        }

        // Map forecasts: take from dailyForecasts, extendedDailyForecasts, or hourlyForecasts
        var fcPlace = hereResponse?.DailyForecasts?.FirstOrDefault()
                      ?? hereResponse?.ExtendedDailyForecasts?.FirstOrDefault()
                      ?? hereResponse?.HourlyForecasts?.FirstOrDefault();

        if (fcPlace?.Forecasts is { Count: > 0 })
        {
            result.Forecasts = fcPlace.Forecasts.Select(f => new WeatherForecast
            {
                Date = f.UtcTime,
                TemperatureHigh = f.HighTemperature,
                TemperatureLow = f.LowTemperature,
                Description = f.Description,
                Icon = f.IconName,
                Humidity = f.Humidity,
                WindSpeed = f.WindSpeed,
                PrecipitationProbability = f.PrecipitationProbability,
                Comfort = f.Comfort
            }).ToList();
        }

        return result;
    }
}
