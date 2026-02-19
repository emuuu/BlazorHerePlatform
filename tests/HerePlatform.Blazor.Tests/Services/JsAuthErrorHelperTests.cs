using HerePlatform.Core.Exceptions;
using HerePlatform.Blazor.Maps.Services;
using Microsoft.JSInterop;

namespace HerePlatform.Blazor.Tests.Services;

[TestFixture]
public class JsAuthErrorHelperTests
{
    [Test]
    public void ThrowIfAuthError_WithAuthMarker_ThrowsHereApiAuthenticationException()
    {
        var jsEx = new JSException("Error: HERE_AUTH_ERROR:routing:HTTP 401");

        var ex = Assert.Throws<HereApiAuthenticationException>(() =>
            JsAuthErrorHelper.ThrowIfAuthError(jsEx, "routing"));

        Assert.That(ex!.Service, Is.EqualTo("routing"));
        Assert.That(ex.InnerException, Is.SameAs(jsEx));
        Assert.That(ex.Message, Does.Contain("routing"));
    }

    [Test]
    public void ThrowIfAuthError_WithoutAuthMarker_DoesNotThrow()
    {
        var jsEx = new JSException("Some other JS error");

        Assert.DoesNotThrow(() =>
            JsAuthErrorHelper.ThrowIfAuthError(jsEx, "routing"));
    }

    [Test]
    public void ThrowIfAuthError_NullMessage_DoesNotThrow()
    {
        var jsEx = new JSException(null!);

        Assert.DoesNotThrow(() =>
            JsAuthErrorHelper.ThrowIfAuthError(jsEx, "routing"));
    }
}
