using SixLabors.ImageSharp;
namespace IGNIS.Services;

public static class DebugImageExporter
{
    private static readonly string BaseDebugPath = Path.Combine(AppContext.BaseDirectory, "debug_images");

    public static void SaveImage(Image image, string subFolder, string fileName)
    {
        try
        {
            var folder = Path.Combine(BaseDebugPath, subFolder);
            Directory.CreateDirectory(folder);

            var path = Path.Combine(folder, fileName + ".png");
            image.Save(path);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DebugImageExporter] Failed to save image {fileName}: {ex.Message}");
        }
    }
}