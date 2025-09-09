using System.Text.Json;
using IGNIS;
using IGNIS.Models;
using IGNIS.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using Tesseract;

var builder = WebApplication.CreateSlimBuilder(args);
builder.Services.AddSingleton(_ => new TesseractEngine(Path.Combine("Assets"), "eng", EngineMode.Default));

var app = builder.Build();

// POST /processStatsImage
app.MapPost("/processStatsImage", (IFormFile? file, TesseractEngine tesseractEngine) =>
{
    if (file == null || file.Length == 0)
    {
        return Results.BadRequest("Expected an image file of format .png, .jpg/jpeg or .bmp");
    }
    if (!file.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
    {
        return Results.BadRequest("File is not an image.");
    }
    
    using var fileStream = file.OpenReadStream();
    using var image = ImageProcessor.LoadAndPrepare(fileStream);
    
    var statsExtractor = new StatsExtractor(tesseractEngine, image);
    var stats = statsExtractor.ExtractStats();
    var statsHash = StatsHasher.HashStats(stats);
    
    var statsOut = new StatsOutDTO()
    {
        CreatedOn = DateTime.Now,
        Stats = stats,
        StatsHash = statsHash
    };
    
    return Results.Ok(statsOut);

    //TODO: Resize image to 1500x--- maintaining ratio.
});

app.Run();