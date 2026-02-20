namespace HerePlatform.RestClient.Internal;

/// <summary>
/// Internal DTOs matching HERE Matrix Routing v8 response structure.
/// The API returns flat arrays (travelTimes[], distances[]) where
/// index = originIndex * numDestinations + destinationIndex.
/// </summary>
internal sealed class HereMatrixResponse
{
    public HereMatrix? Matrix { get; set; }
}

internal sealed class HereMatrix
{
    public int NumOrigins { get; set; }
    public int NumDestinations { get; set; }
    public List<int>? TravelTimes { get; set; }
    public List<int>? Distances { get; set; }
    public List<int>? ErrorCodes { get; set; }
}
