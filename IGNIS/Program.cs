using IGNIS;
using IGNIS.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using Tesseract;

var builder = WebApplication.CreateSlimBuilder(args);
builder.Services.AddSingleton(_ => new TesseractEngine(Path.Combine("Assets"), "eng", EngineMode.Default));

var app = builder.Build();

// POST /processStatsImage
app.MapPost("/processStatsImage", async (IFormFile file) =>
{
    if (file == null || file.Length == 0)
    {
        return Results.BadRequest("Expected an image file of format .png, .jpg/jpeg or .bmp");
    }
    if (!file.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
    {
        return Results.BadRequest("File is not an image.");
    }
    
    var statsOut = new StatsOutDTO();
    
    using var fileStream = file.OpenReadStream();
    using var image = ImageProcessor.LoadAndPrepare(fileStream);
    
    List<Stats> stats = StatsExtractor.
        
    
    //TODO: Resize image to 1500x--- maintaining ratio.
});

var sampleTodos = TodoGenerator.GenerateTodos().ToArray();

var todosApi = app.MapGroup("/todos");
todosApi.MapGet("/", () => sampleTodos);
todosApi.MapGet("/{id}", (int id) =>
    sampleTodos.FirstOrDefault(a => a.Id == id) is { } todo
        ? Results.Ok(todo)
        : Results.NotFound());

app.Run();