namespace HerePlatform.RestClient.Internal;

/// <summary>
/// Internal DTOs matching HERE Geocoding &amp; Search v7 response structure.
/// </summary>
internal sealed class HereGeocodeResponse
{
    public List<HereGeocodeItem>? Items { get; set; }
}

internal sealed class HereGeocodeItem
{
    public string? Title { get; set; }
    public HerePosition? Position { get; set; }
    public HereAddress? Address { get; set; }
    public string? ResultType { get; set; }
}

internal sealed class HerePosition
{
    public double Lat { get; set; }
    public double Lng { get; set; }
}

internal sealed class HereAddress
{
    public string? Label { get; set; }
}
