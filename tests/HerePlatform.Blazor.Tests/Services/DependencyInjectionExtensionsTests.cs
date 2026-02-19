using HerePlatform.Blazor;
using HerePlatform.Blazor.Maps;
using Microsoft.Extensions.DependencyInjection;

namespace HerePlatform.Blazor.Tests.Services;

[TestFixture]
public class DependencyInjectionExtensionsTests
{
    [Test]
    public void AddHerePlatformBlazor_WithApiKey_RegistersService()
    {
        var services = new ServiceCollection();

        services.AddHerePlatformBlazor("test-key");

        var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();
        var service = scope.ServiceProvider.GetService<IHerePlatformKeyService>();

        Assert.That(service, Is.Not.Null);
        Assert.That(service, Is.InstanceOf<HerePlatformKeyService>());
    }

    [Test]
    public async Task AddHerePlatformBlazor_WithApiKey_ServiceReturnsCorrectKey()
    {
        var services = new ServiceCollection();
        services.AddHerePlatformBlazor("my-api-key");

        var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IHerePlatformKeyService>();

        var opts = await service.GetApiOptions();
        Assert.That(opts.ApiKey, Is.EqualTo("my-api-key"));
    }

    [Test]
    public void AddHerePlatformBlazor_WithOptions_RegistersService()
    {
        var services = new ServiceCollection();
        var apiOpts = new HereApiLoadOptions("key") { Language = "de-DE" };

        services.AddHerePlatformBlazor(apiOpts);

        var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();
        var service = scope.ServiceProvider.GetService<IHerePlatformKeyService>();

        Assert.That(service, Is.Not.Null);
    }

    [Test]
    public async Task AddHerePlatformBlazor_WithOptions_PreservesAllSettings()
    {
        var services = new ServiceCollection();
        var apiOpts = new HereApiLoadOptions("key")
        {
            Language = "de-DE",
            UseHarpEngine = true,
            LoadClustering = true
        };

        services.AddHerePlatformBlazor(apiOpts);

        var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IHerePlatformKeyService>();
        var result = await service.GetApiOptions();

        Assert.That(result.Language, Is.EqualTo("de-DE"));
        Assert.That(result.UseHarpEngine, Is.True);
        Assert.That(result.LoadClustering, Is.True);
    }

    [Test]
    public void AddHerePlatformBlazor_WithCustomService_RegistersService()
    {
        var services = new ServiceCollection();
        var customService = new HerePlatformKeyService("custom");

        services.AddHerePlatformBlazor(customService);

        var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();
        var service = scope.ServiceProvider.GetService<IHerePlatformKeyService>();

        Assert.That(service, Is.SameAs(customService));
    }

    [Test]
    public void AddHerePlatformBlazor_ReturnsSameServiceCollection()
    {
        var services = new ServiceCollection();

        var result = services.AddHerePlatformBlazor("key");

        Assert.That(result, Is.SameAs(services));
    }

    [Test]
    public void AddHerePlatformBlazor_RegistersAsScoped()
    {
        var services = new ServiceCollection();
        services.AddHerePlatformBlazor("key");

        var descriptor = services.Single(d => d.ServiceType == typeof(IHerePlatformKeyService));

        Assert.That(descriptor.Lifetime, Is.EqualTo(ServiceLifetime.Scoped));
    }

    [Test]
    public void AddHerePlatformBlazor_WithCustomService_RegistersAsScoped()
    {
        var services = new ServiceCollection();
        services.AddHerePlatformBlazor(new HerePlatformKeyService("key"));

        var descriptor = services.Single(d => d.ServiceType == typeof(IHerePlatformKeyService));

        Assert.That(descriptor.Lifetime, Is.EqualTo(ServiceLifetime.Scoped));
    }

    [Test]
    public void AddHerePlatformBlazor_NullApiKey_Throws()
    {
        var services = new ServiceCollection();
        Assert.Throws<ArgumentNullException>(() => services.AddHerePlatformBlazor((string)null!));
    }

    [Test]
    public void AddHerePlatformBlazor_EmptyApiKey_Throws()
    {
        var services = new ServiceCollection();
        Assert.Throws<ArgumentException>(() => services.AddHerePlatformBlazor(""));
    }

    [Test]
    public void AddHerePlatformBlazor_WhitespaceApiKey_Throws()
    {
        var services = new ServiceCollection();
        Assert.Throws<ArgumentException>(() => services.AddHerePlatformBlazor("   "));
    }

    [Test]
    public void AddHerePlatformBlazor_NullOptions_Throws()
    {
        var services = new ServiceCollection();
        Assert.Throws<ArgumentNullException>(() => services.AddHerePlatformBlazor((HereApiLoadOptions)null!));
    }

    [Test]
    public void AddHerePlatformBlazor_NullKeyService_Throws()
    {
        var services = new ServiceCollection();
        Assert.Throws<ArgumentNullException>(() => services.AddHerePlatformBlazor((IHerePlatformKeyService)null!));
    }
}
