using BlazorHerePlatform.Docs.Models;

namespace BlazorHerePlatform.Docs.Services;

public interface IApiDocService
{
    Task InitializeAsync();
    ApiTypeDoc? GetApiDoc(string typeName);
}
