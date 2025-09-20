using IGNIS;
using IGNIS.Models;
using IGNIS.Services;
using Tesseract;

string TESSROOT_DEBUG = Path.Combine(AppContext.BaseDirectory, @"..\..\..\", @"Tesseract");
string TESSROOT_BUILD = Path.Combine(AppContext.BaseDirectory, @"..\..\..\", @"Tesseract");

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddSingleton(_ => new TesseractEngine(Path.Combine(TESSROOT_DEBUG, "tessdata"), "Sinclair", EngineMode.Default));

var app = builder.Build();
var group = app.MapGroup("/").DisableAntiforgery();

// POST /processStatsImage
//TODO: Make async
//TODO: Train Tesseract model. Optimize for digits with HD2 font
group.MapPost("/extractstats", (IFormFile? screenshot, TesseractEngine tesseractEngine) =>
{
    if (screenshot == null || screenshot.Length == 0)
        return Results.BadRequest("Expected an image screenshot.");

    if (!screenshot.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
        return Results.BadRequest("Screenshot is not an image.");

    using var stream = screenshot.OpenReadStream();
    using var image = ImageProcessor.LoadAndPrepare(stream);
    
    var stats = new StatsExtractor(tesseractEngine, image).ExtractStats();
    var statsOut = new StatsOutDTO
    {
        Stats = stats,
        StatsHash = StatsHasher.HashStats(stats),
        CreatedOn = DateTime.Now
    };

    return Results.Ok(statsOut);
});

app.Run();