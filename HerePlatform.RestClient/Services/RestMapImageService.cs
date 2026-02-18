using System.Globalization;
using HerePlatform.Core.MapImage;
using HerePlatform.Core.Services;
using HerePlatform.RestClient.Internal;

namespace HerePlatform.RestClient.Services;

internal sealed class RestMapImageService : IMapImageService
{
    private const string BaseUrl = "https://image.maps.hereapi.com/mia/v3/base/mc";

    private readonly IHttpClientFactory _httpClientFactory;

    public RestMapImageService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<byte[]> GetImageAsync(MapImageRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var lat = request.Center.Lat.ToString(CultureInfo.InvariantCulture);
        var lng = request.Center.Lng.ToString(CultureInfo.InvariantCulture);
        var zoom = request.Zoom.ToString(CultureInfo.InvariantCulture);
        var size = $"{request.Width}x{request.Height}";
        var format = HereApiHelper.GetEnumMemberValue(request.Format);
        var style = HereApiHelper.GetEnumMemberValue(request.Style);
        var ppi = request.Ppi.ToString(CultureInfo.InvariantCulture);

        // Path-based URL construction — colons and semicolons must NOT be URI-escaped
        var url = $"{BaseUrl}/center:{lat},{lng};zoom={zoom}/{size}/{format}?style={style}&ppi={ppi}";

        var client = _httpClientFactory.CreateClient(HereApiHelper.ClientName);
        using var response = await client.GetAsync(url, cancellationToken).ConfigureAwait(false);

        await HereApiHelper.EnsureSuccessOrThrowAsync(response, "mapImage", cancellationToken).ConfigureAwait(false);

        return await response.Content.ReadAsByteArrayAsync(cancellationToken).ConfigureAwait(false);
    }
}
