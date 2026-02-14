using BlazorHerePlatform.Docs;
using BlazorHerePlatform.Docs.Services;
using HerePlatformComponents;
using HerePlatformComponents.Maps;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddBlazorHerePlatform(new HereApiLoadOptions("YOUR_API_KEY") { LoadClustering = true, LoadData = true });
builder.Services.AddScoped<IDocContentService, DocContentService>();
builder.Services.AddScoped<IApiDocService, ApiDocService>();

await builder.Build().RunAsync();
