namespace HerePlatform.Core.Search;

/// <summary>
/// Result of a HERE Autocomplete API call.
/// </summary>
public class AutocompleteResult
{
    /// <summary>
    /// List of autocomplete items.
    /// </summary>
    public List<AutosuggestItem>? Items { get; set; }
}
