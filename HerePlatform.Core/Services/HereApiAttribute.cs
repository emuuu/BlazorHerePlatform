namespace HerePlatform.Core.Services;

/// <summary>
/// Declares which HERE API and version a service interface targets.
/// </summary>
[AttributeUsage(AttributeTargets.Interface, Inherited = false)]
public sealed class HereApiAttribute : Attribute
{
    /// <summary>
    /// Official HERE API product name (e.g. "Routing API").
    /// </summary>
    public string ApiName { get; }

    /// <summary>
    /// API version string (e.g. "v8").
    /// </summary>
    public string Version { get; }

    public HereApiAttribute(string apiName, string version)
    {
        ApiName = apiName;
        Version = version;
    }
}
