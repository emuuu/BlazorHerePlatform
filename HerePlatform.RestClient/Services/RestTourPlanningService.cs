using System.Text;
using System.Text.Json;
using HerePlatform.Core.Coordinates;
using HerePlatform.Core.TourPlanning;
using HerePlatform.Core.Services;
using HerePlatform.RestClient.Internal;

namespace HerePlatform.RestClient.Services;

internal sealed class RestTourPlanningService : ITourPlanningService
{
    private const string BaseUrl = "https://tourplanning.hereapi.com/v3/problems";

    private readonly IHttpClientFactory _httpClientFactory;

    public RestTourPlanningService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<TourPlanningResult> SolveAsync(TourPlanningProblem problem, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(problem);

        var jsonBody = JsonSerializer.Serialize(problem, HereJsonDefaults.Options);
        using var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

        var client = _httpClientFactory.CreateClient(HereApiHelper.ClientName);
        using var response = await client.PostAsync(BaseUrl, content, cancellationToken).ConfigureAwait(false);

        await HereApiHelper.EnsureSuccessOrThrowAsync(response, "tourPlanning", cancellationToken).ConfigureAwait(false);

        var json = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        var hereResponse = JsonSerializer.Deserialize<HereTourPlanningResponse>(json, HereJsonDefaults.Options);

        return MapToResult(hereResponse);
    }

    private static TourPlanningResult MapToResult(HereTourPlanningResponse? hereResponse)
    {
        if (hereResponse is null)
            return new TourPlanningResult { Tours = [], UnassignedJobs = [] };

        return new TourPlanningResult
        {
            Tours = hereResponse.Tours?.Select(t => new Tour
            {
                VehicleId = t.VehicleId,
                Stops = t.Stops?.Select(s => new TourStop
                {
                    Location = s.Location is not null
                        ? new LatLngLiteral(s.Location.Lat, s.Location.Lng)
                        : null,
                    Activities = s.Activities?.Select(a => new TourActivity
                    {
                        Type = a.Type,
                        JobId = a.JobId
                    }).ToList()
                }).ToList(),
                Statistic = MapStatistic(t.Statistic)
            }).ToList() ?? [],
            UnassignedJobs = hereResponse.UnassignedJobs,
            Statistic = MapStatistic(hereResponse.Statistic)
        };
    }

    private static TourStatistic? MapStatistic(HereTourStatistic? stat)
    {
        if (stat is null)
            return null;

        return new TourStatistic
        {
            Cost = stat.Cost,
            Distance = stat.Distance,
            Duration = stat.Duration
        };
    }
}
