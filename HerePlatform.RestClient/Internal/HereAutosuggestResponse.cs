namespace HerePlatform.RestClient.Internal;

/// <summary>
/// Internal DTOs matching HERE Autosuggest v1 response structure.
/// </summary>
internal sealed class HereAutosuggestResponse
{
    public List<HereAutosuggestItem>? Items { get; set; }
}

internal sealed class HereAutosuggestItem
{
    public string? Title { get; set; }
    public string? Id { get; set; }
    public string? ResultType { get; set; }
    public HereAddress? Address { get; set; }
    public HerePosition? Position { get; set; }
    public HereHighlights? Highlights { get; set; }
}

internal sealed class HereHighlights
{
    public List<HereHighlightRange>? Title { get; set; }
    public List<HereHighlightRange>? Address { get; set; }
}

internal sealed class HereHighlightRange
{
    public int Start { get; set; }
    public int End { get; set; }
}
