using System.Net;

namespace HerePlatform.RestClient.Tests;

internal class MockHttpHandler : HttpMessageHandler
{
    private readonly Func<HttpRequestMessage, HttpResponseMessage> _handler;

    public HttpRequestMessage? LastRequest { get; private set; }
    public List<HttpRequestMessage> AllRequests { get; } = [];

    public MockHttpHandler(HttpResponseMessage response)
        : this(_ => response)
    {
    }

    public MockHttpHandler(Func<HttpRequestMessage, HttpResponseMessage> handler)
    {
        _handler = handler;
    }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        LastRequest = request;
        AllRequests.Add(request);
        return Task.FromResult(_handler(request));
    }

    public static MockHttpHandler WithJson(string json, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        return new MockHttpHandler(new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
        });
    }

    public static MockHttpHandler WithStatus(HttpStatusCode statusCode)
    {
        return new MockHttpHandler(new HttpResponseMessage(statusCode));
    }
}
