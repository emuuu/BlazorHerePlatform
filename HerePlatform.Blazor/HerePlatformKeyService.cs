using System;
using System.Threading.Tasks;

namespace HerePlatform.Blazor;

public interface IHerePlatformKeyService
{
    Task<Maps.HereApiLoadOptions> GetApiOptions();
    bool IsApiInitialized { get; }
    void MarkApiInitialized();
    void UpdateApiKey(string apiKey);
}

public class HerePlatformKeyService : IHerePlatformKeyService
{
    private Maps.HereApiLoadOptions _initOptions;
    public bool IsApiInitialized { get; private set; }

    public void MarkApiInitialized() => IsApiInitialized = true;

    public HerePlatformKeyService(string apiKey)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(apiKey);
        _initOptions = new Maps.HereApiLoadOptions(apiKey);
    }

    public HerePlatformKeyService(Maps.HereApiLoadOptions opts)
    {
        ArgumentNullException.ThrowIfNull(opts);
        _initOptions = opts;
    }

    public Task<Maps.HereApiLoadOptions> GetApiOptions()
    {
        return Task.FromResult(_initOptions);
    }

    public void UpdateApiKey(string apiKey)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(apiKey);
        _initOptions = new Maps.HereApiLoadOptions(apiKey);
        IsApiInitialized = false;
    }
}
