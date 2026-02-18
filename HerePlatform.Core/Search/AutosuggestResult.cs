namespace HerePlatform.Core.Search;

/// <summary>
/// Result of a HERE Autosuggest API call.
/// </summary>
public class AutosuggestResult
{
    /// <summary>
    /// List of suggestion items.
    /// </summary>
    public List<AutosuggestItem>? Items { get; set; }
}
