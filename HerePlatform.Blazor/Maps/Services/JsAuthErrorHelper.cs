using HerePlatform.Core.Exceptions;
using Microsoft.JSInterop;

namespace HerePlatform.Blazor.Maps.Services;

internal static class JsAuthErrorHelper
{
    internal static void ThrowIfAuthError(JSException ex, string serviceName)
    {
        if (ex.Message?.Contains("HERE_AUTH_ERROR") == true)
            throw new HereApiAuthenticationException(
                $"HERE API authentication failed for '{serviceName}'. Check your API key.",
                serviceName, ex);
    }
}
