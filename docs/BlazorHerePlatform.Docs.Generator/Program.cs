using BlazorHerePlatform.Docs.Generator;

if (args.Length == 0)
{
    Console.Error.WriteLine("Usage: BlazorHerePlatform.Docs.Generator <wwwroot-path>");
    return 1;
}

var wwwrootPath = args[0];
if (!Directory.Exists(wwwrootPath))
{
    Console.Error.WriteLine($"wwwroot path not found: {wwwrootPath}");
    return 1;
}

var dataDir = Path.Combine(wwwrootPath, "data");
Directory.CreateDirectory(dataDir);

Console.WriteLine("Generating API documentation...");
var apiGenerator = new ApiDocGenerator();
await apiGenerator.GenerateAsync(Path.Combine(dataDir, "api-docs.json"));

Console.WriteLine("Generating content index...");
var contentGenerator = new ContentIndexGenerator();
await contentGenerator.GenerateAsync(
    Path.Combine(wwwrootPath, "content"),
    Path.Combine(dataDir, "content-index.json"));

Console.WriteLine("Documentation data generated successfully.");
return 0;
