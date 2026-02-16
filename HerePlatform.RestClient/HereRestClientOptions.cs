namespace HerePlatform.RestClient;

/// <summary>
/// Configuration options for the HERE REST API client.
/// Exactly one authentication method must be configured.
/// </summary>
public class HereRestClientOptions
{
    /// <summary>
    /// API Key authentication (simplest method).
    /// Appended as ?apiKey=... query parameter.
    /// </summary>
    public string? ApiKey { get; set; }

    /// <summary>
    /// OAuth 2.0 Access Key ID (server-to-server, HMAC-SHA256).
    /// Must be set together with <see cref="AccessKeySecret"/>.
    /// </summary>
    public string? AccessKeyId { get; set; }

    /// <summary>
    /// OAuth 2.0 Access Key Secret (server-to-server, HMAC-SHA256).
    /// Must be set together with <see cref="AccessKeyId"/>.
    /// </summary>
    public string? AccessKeySecret { get; set; }

    /// <summary>
    /// External identity provider token callback.
    /// Called to obtain a Bearer token for each request batch.
    /// </summary>
    public Func<CancellationToken, Task<string>>? TokenProvider { get; set; }

    /// <summary>
    /// HTTP request timeout. Default: 30 seconds.
    /// </summary>
    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(30);

    internal void Validate()
    {
        int methodCount = 0;

        if (!string.IsNullOrWhiteSpace(ApiKey))
            methodCount++;

        bool hasOAuthId = !string.IsNullOrWhiteSpace(AccessKeyId);
        bool hasOAuthSecret = !string.IsNullOrWhiteSpace(AccessKeySecret);

        if (hasOAuthId || hasOAuthSecret)
        {
            if (!hasOAuthId || !hasOAuthSecret)
                throw new InvalidOperationException(
                    "Both AccessKeyId and AccessKeySecret must be provided for OAuth 2.0 authentication.");
            methodCount++;
        }

        if (TokenProvider is not null)
            methodCount++;

        if (methodCount == 0)
            throw new InvalidOperationException(
                "No authentication method configured. Set ApiKey, AccessKeyId/AccessKeySecret, or TokenProvider.");

        if (methodCount > 1)
            throw new InvalidOperationException(
                "Multiple authentication methods configured. Use exactly one of: ApiKey, AccessKeyId/AccessKeySecret, or TokenProvider.");
    }
}
