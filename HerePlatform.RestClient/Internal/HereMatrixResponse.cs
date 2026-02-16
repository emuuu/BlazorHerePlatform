namespace HerePlatform.RestClient.Internal;

/// <summary>
/// Internal DTOs matching HERE Matrix Routing v8 response structure.
/// </summary>
internal sealed class HereMatrixResponse
{
    public HereMatrix? Matrix { get; set; }
}

internal sealed class HereMatrix
{
    public int NumOrigins { get; set; }
    public int NumDestinations { get; set; }
    public List<HereMatrixEntry>? Entries { get; set; }
}

internal sealed class HereMatrixEntry
{
    public int OriginIndex { get; set; }
    public int DestinationIndex { get; set; }
    public int? TravelTime { get; set; }
    public int? Distance { get; set; }
}
