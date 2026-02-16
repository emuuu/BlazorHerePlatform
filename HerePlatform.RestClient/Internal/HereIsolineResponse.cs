namespace HerePlatform.RestClient.Internal;

/// <summary>
/// Internal DTOs matching HERE Isoline Routing v8 response structure.
/// </summary>
internal sealed class HereIsolineResponse
{
    public List<HereIsoline>? Isolines { get; set; }
}

internal sealed class HereIsoline
{
    public HereIsolineRange? Range { get; set; }
    public List<HereIsolinePolygonWrapper>? Polygons { get; set; }
}

internal sealed class HereIsolineRange
{
    public int Value { get; set; }
}

internal sealed class HereIsolinePolygonWrapper
{
    public string? Outer { get; set; }
}
