using System.Net;
using HerePlatform.Core.Exceptions;

namespace HerePlatform.RestClient.Internal;

internal static class HereApiHelper
{
    public static void EnsureAuthSuccess(HttpResponseMessage response, string serviceName)
    {
        if (response.StatusCode is HttpStatusCode.Unauthorized or HttpStatusCode.Forbidden)
        {
            throw new HereApiAuthenticationException(
                $"HERE API authentication failed for {serviceName} (HTTP {(int)response.StatusCode}).",
                serviceName);
        }
    }

    public static string BuildQueryString(params (string key, string? value)[] parameters)
    {
        var pairs = parameters
            .Where(p => p.value is not null)
            .Select(p => $"{Uri.EscapeDataString(p.key)}={Uri.EscapeDataString(p.value!)}");

        return string.Join("&", pairs);
    }
}
