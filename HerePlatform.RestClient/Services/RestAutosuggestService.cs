using System.Text.Json;
using HerePlatform.Core.Coordinates;
using HerePlatform.Core.Search;
using HerePlatform.Core.Services;
using HerePlatform.RestClient.Internal;

namespace HerePlatform.RestClient.Services;

internal sealed class RestAutosuggestService : IAutosuggestService
{
    private const string AutosuggestBaseUrl = "https://autosuggest.search.hereapi.com/v1/autosuggest";
    private const string AutocompleteBaseUrl = "https://autocomplete.search.hereapi.com/v1/autocomplete";

    private readonly IHttpClientFactory _httpClientFactory;

    public RestAutosuggestService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<AutosuggestResult> SuggestAsync(string query, AutosuggestOptions? options = null)
    {
        var hereResponse = await ExecuteRequestAsync(AutosuggestBaseUrl, query, options, "autosuggest")
            .ConfigureAwait(false);

        return new AutosuggestResult { Items = MapItems(hereResponse) };
    }

    public async Task<AutocompleteResult> AutocompleteAsync(string query, AutosuggestOptions? options = null)
    {
        var hereResponse = await ExecuteRequestAsync(AutocompleteBaseUrl, query, options, "autocomplete")
            .ConfigureAwait(false);

        return new AutocompleteResult { Items = MapItems(hereResponse) };
    }

    private async Task<HereAutosuggestResponse?> ExecuteRequestAsync(
        string baseUrl, string query, AutosuggestOptions? options, string serviceName)
    {
        var opts = options ?? new AutosuggestOptions();

        var qs = HereApiHelper.BuildQueryString(
            ("q", query),
            ("at", opts.At.HasValue ? HereApiHelper.FormatCoord(opts.At.Value) : null),
            ("in", opts.In),
            ("lang", opts.Lang),
            ("limit", opts.Limit.ToString()));

        var url = $"{baseUrl}?{qs}";

        var client = _httpClientFactory.CreateClient("HereApi");
        using var response = await client.GetAsync(url).ConfigureAwait(false);

        HereApiHelper.EnsureAuthSuccess(response, serviceName);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        return JsonSerializer.Deserialize<HereAutosuggestResponse>(json, HereJsonDefaults.Options);
    }

    private static List<AutosuggestItem> MapItems(HereAutosuggestResponse? hereResponse)
    {
        if (hereResponse?.Items is null or { Count: 0 })
            return [];

        return hereResponse.Items.Select(item => new AutosuggestItem
        {
            Title = item.Title,
            Id = item.Id,
            ResultType = item.ResultType,
            Address = MapAddress(item.Address),
            Position = item.Position is not null
                ? new LatLngLiteral(item.Position.Lat, item.Position.Lng)
                : null,
            Highlights = MapHighlights(item.Highlights)
        }).ToList();
    }

    private static AutosuggestAddress? MapAddress(HereAddress? address)
    {
        if (address is null)
            return null;

        return new AutosuggestAddress
        {
            Label = address.Label,
            CountryCode = address.CountryCode,
            CountryName = address.CountryName,
            State = address.State,
            City = address.City,
            District = address.District,
            Street = address.Street,
            PostalCode = address.PostalCode,
            HouseNumber = address.HouseNumber
        };
    }

    private static AutosuggestHighlights? MapHighlights(HereHighlights? highlights)
    {
        if (highlights is null)
            return null;

        return new AutosuggestHighlights
        {
            Title = highlights.Title?.Select(r => new AutosuggestHighlightRange
            {
                Start = r.Start,
                End = r.End
            }).ToArray(),
            Address = highlights.Address?.Select(r => new AutosuggestHighlightRange
            {
                Start = r.Start,
                End = r.End
            }).ToArray()
        };
    }
}
