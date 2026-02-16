using System.Net;
using HerePlatform.RestClient.Auth;
using Microsoft.Extensions.Options;

namespace HerePlatform.RestClient.Tests;

[TestFixture]
public class AuthHandlerTests
{
    private static HereAuthHandler CreateHandler(
        HereRestClientOptions options,
        HttpMessageHandler? innerHandler = null)
    {
        var optionsWrapper = Options.Create(options);

        // Create a minimal IHttpClientFactory for OAuth token client
        var factory = new FakeHttpClientFactory(innerHandler);
        var handler = new HereAuthHandler(optionsWrapper, factory);
        handler.InnerHandler = innerHandler ?? new MockHttpHandler(new HttpResponseMessage(HttpStatusCode.OK));
        return handler;
    }

    [Test]
    public async Task ApiKey_AppendsToQueryString()
    {
        var mockInner = new MockHttpHandler(new HttpResponseMessage(HttpStatusCode.OK));
        var options = new HereRestClientOptions { ApiKey = "my-test-key" };
        var handler = CreateHandler(options, mockInner);

        using var client = new HttpClient(handler);
        await client.GetAsync("https://example.com/v1/test");

        Assert.That(mockInner.LastRequest, Is.Not.Null);
        Assert.That(mockInner.LastRequest!.RequestUri!.ToString(), Does.Contain("apiKey=my-test-key"));
    }

    [Test]
    public async Task ApiKey_AppendsWithAmpersandWhenQueryExists()
    {
        var mockInner = new MockHttpHandler(new HttpResponseMessage(HttpStatusCode.OK));
        var options = new HereRestClientOptions { ApiKey = "my-test-key" };
        var handler = CreateHandler(options, mockInner);

        using var client = new HttpClient(handler);
        await client.GetAsync("https://example.com/v1/test?q=hello");

        Assert.That(mockInner.LastRequest!.RequestUri!.ToString(), Does.Contain("&apiKey=my-test-key"));
    }

    [Test]
    public async Task TokenProvider_SetsBearerHeader()
    {
        var mockInner = new MockHttpHandler(new HttpResponseMessage(HttpStatusCode.OK));
        var options = new HereRestClientOptions
        {
            TokenProvider = _ => Task.FromResult("ext-token-123")
        };
        var handler = CreateHandler(options, mockInner);

        using var client = new HttpClient(handler);
        await client.GetAsync("https://example.com/v1/test");

        Assert.That(mockInner.LastRequest, Is.Not.Null);
        Assert.That(mockInner.LastRequest!.Headers.Authorization, Is.Not.Null);
        Assert.That(mockInner.LastRequest!.Headers.Authorization!.Scheme, Is.EqualTo("Bearer"));
        Assert.That(mockInner.LastRequest!.Headers.Authorization!.Parameter, Is.EqualTo("ext-token-123"));
    }

    /// <summary>
    /// Minimal IHttpClientFactory for tests — returns HttpClient with the given handler.
    /// </summary>
    private class FakeHttpClientFactory : IHttpClientFactory
    {
        private readonly HttpMessageHandler? _handler;

        public FakeHttpClientFactory(HttpMessageHandler? handler = null)
        {
            _handler = handler;
        }

        public HttpClient CreateClient(string name)
        {
            return _handler is not null
                ? new HttpClient(_handler)
                : new HttpClient(new MockHttpHandler(new HttpResponseMessage(HttpStatusCode.OK)));
        }
    }
}
