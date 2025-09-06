using IGNIS;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;

var builder = WebApplication.CreateSlimBuilder(args);
var app = builder.Build();

// POST /processStatsImage
app.MapPost("/processStatsImage", async (IFormFile file) =>
{
    if (file == null || file.Length == 0)
    {
        return Results.BadRequest("Expected an image file of format .png, .jpg/jpeg or .bmp");
    }
    
    var imageHandler = new ImageHandler(file);
    var StatsList = imageHandler.GetStatsFromImage();
    
    
    List<Image> processableSnippets = imageHandler.SplitIntoProcessableChunks();
    
    //TODO: Resize image to 1500x--- maintaining ratio
});

var sampleTodos = TodoGenerator.GenerateTodos().ToArray();

var todosApi = app.MapGroup("/todos");
todosApi.MapGet("/", () => sampleTodos);
todosApi.MapGet("/{id}", (int id) =>
    sampleTodos.FirstOrDefault(a => a.Id == id) is { } todo
        ? Results.Ok(todo)
        : Results.NotFound());

app.Run();