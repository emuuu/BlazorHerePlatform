using System;
using HerePlatform.Core.Services;
using HerePlatform.Blazor.Maps.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HerePlatform.Blazor;

public static class DependencyInjectionExtensions
{
    /// <summary>
    /// Registers HerePlatform.NET.Blazor services. Only one API key per page is supported.
    /// </summary>
    public static IServiceCollection AddHerePlatformBlazor(this IServiceCollection services, string apiKey)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(apiKey);
        services.AddScoped<IHerePlatformKeyService>(_ => new HerePlatformKeyService(apiKey));
        RegisterServices(services);
        return services;
    }

    public static IServiceCollection AddHerePlatformBlazor(this IServiceCollection services, Maps.HereApiLoadOptions opts)
    {
        ArgumentNullException.ThrowIfNull(opts);
        services.AddScoped<IHerePlatformKeyService>(_ => new HerePlatformKeyService(opts));
        RegisterServices(services);
        return services;
    }

    /// <summary>
    /// Registers HerePlatform.NET.Blazor services with a custom key service instance.
    /// <para>
    /// <strong>Warning (Blazor Server):</strong> The provided instance is shared across
    /// all scopes (circuits). Mutable state like <see cref="IHerePlatformKeyService.IsApiInitialized"/>
    /// will leak between users. Prefer the <c>AddHerePlatformBlazor(string apiKey)</c> or
    /// <c>AddHerePlatformBlazor(HereApiLoadOptions)</c> overloads for per-circuit isolation.
    /// </para>
    /// </summary>
    public static IServiceCollection AddHerePlatformBlazor(this IServiceCollection services, IHerePlatformKeyService keyService)
    {
        ArgumentNullException.ThrowIfNull(keyService);
        services.AddScoped<IHerePlatformKeyService>(_ => keyService);
        RegisterServices(services);
        return services;
    }

    private static void RegisterServices(IServiceCollection services)
    {
        services.AddScoped<IRoutingService, RoutingService>();
        services.AddScoped<IGeocodingService, GeocodingService>();
        services.AddScoped<ITrafficService, TrafficService>();
        services.AddScoped<IPublicTransitService, PublicTransitService>();
        services.AddScoped<IWaypointSequenceService, WaypointSequenceService>();
        services.AddScoped<IGeofencingService, GeofencingService>();
        services.AddScoped<IPlacesService, PlacesService>();
        services.AddScoped<IIsolineService, IsolineService>();
        services.AddScoped<IMatrixRoutingService, MatrixRoutingService>();
    }
}
