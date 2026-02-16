using System.Net;
using HerePlatform.RestClient.Auth;

namespace HerePlatform.RestClient.Tests;

[TestFixture]
public class OAuthTokenManagerTests
{
    [Test]
    public void ComputeHmacSha256_ProducesExpectedSignature()
    {
        // Known test vector
        var result = HereOAuthTokenManager.ComputeHmacSha256("key", "data");
        Assert.That(result, Is.Not.Null.And.Not.Empty);
        // HMAC-SHA256("key", "data") is a well-known value
        Assert.That(result, Is.EqualTo("UDH+PZicbRU3oBP6bnOdojRj/a7DtwE32Cjjas4iG9A="));
    }

    [Test]
    public async Task GetTokenAsync_SendsCorrectRequest()
    {
        var tokenJson = """{"access_token":"test-token-abc","token_type":"bearer","expires_in":86399}""";
        var mockHandler = MockHttpHandler.WithJson(tokenJson);
        var httpClient = new HttpClient(mockHandler);

        var manager = new HereOAuthTokenManager("my-key-id", "my-key-secret", httpClient);
        var token = await manager.GetTokenAsync(CancellationToken.None);

        Assert.That(token, Is.EqualTo("test-token-abc"));
        Assert.That(mockHandler.LastRequest, Is.Not.Null);
        Assert.That(mockHandler.LastRequest!.Method, Is.EqualTo(HttpMethod.Post));
        Assert.That(mockHandler.LastRequest!.RequestUri!.ToString(),
            Is.EqualTo("https://account.api.here.com/oauth2/token"));

        // Verify Authorization header contains OAuth params
        var authHeader = mockHandler.LastRequest!.Headers.GetValues("Authorization").First();
        Assert.That(authHeader, Does.StartWith("OAuth "));
        Assert.That(authHeader, Does.Contain("oauth_consumer_key=\"my-key-id\""));
        Assert.That(authHeader, Does.Contain("oauth_signature_method=\"HMAC-SHA256\""));
        Assert.That(authHeader, Does.Contain("oauth_version=\"1.0\""));
    }

    [Test]
    public async Task GetTokenAsync_CachesToken()
    {
        var tokenJson = """{"access_token":"cached-token","token_type":"bearer","expires_in":86399}""";
        var mockHandler = MockHttpHandler.WithJson(tokenJson);
        var httpClient = new HttpClient(mockHandler);

        var manager = new HereOAuthTokenManager("key", "secret", httpClient);

        var token1 = await manager.GetTokenAsync(CancellationToken.None);
        var token2 = await manager.GetTokenAsync(CancellationToken.None);

        Assert.That(token1, Is.EqualTo("cached-token"));
        Assert.That(token2, Is.EqualTo("cached-token"));
        // Only one HTTP request should have been made
        Assert.That(mockHandler.AllRequests, Has.Count.EqualTo(1));
    }
}
