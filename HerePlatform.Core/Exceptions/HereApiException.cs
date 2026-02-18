using System.Net;

namespace HerePlatform.Core.Exceptions;

/// <summary>
/// Thrown when a HERE API call returns a non-success HTTP status code.
/// Contains the status code and the raw error body returned by the API.
/// </summary>
public class HereApiException : Exception
{
    /// <summary>
    /// The HTTP status code returned by the HERE API.
    /// </summary>
    public HttpStatusCode StatusCode { get; }

    /// <summary>
    /// The raw error response body from the HERE API.
    /// </summary>
    public string? ErrorBody { get; }

    /// <summary>
    /// The service that reported the error (e.g. "routing", "geocoding").
    /// </summary>
    public string? Service { get; }

    public HereApiException(HttpStatusCode statusCode, string? errorBody, string? service)
        : base($"HERE API error {(int)statusCode} from {service}: {Truncate(errorBody, 200)}")
    {
        StatusCode = statusCode;
        ErrorBody = errorBody;
        Service = service;
    }

    public HereApiException(string message)
        : base(message)
    {
    }

    public HereApiException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    private static string? Truncate(string? value, int maxLength)
        => value is not null && value.Length > maxLength ? value[..maxLength] + "..." : value;
}
