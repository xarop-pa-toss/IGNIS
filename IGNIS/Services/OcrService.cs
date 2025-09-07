using Tesseract;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.IO;

namespace IGNIS;

public class OcrService
{
    private readonly string _tessDataPath = Path.Combine("Assets");

    public string ExtractText(Image image)
    {
        // Tesseract wants to use System.Drawing.Bitmap so the ImageSharp image needs conversion
        using var memStream = new MemoryStream();
        image.SaveAsBmp(memStream);
        memStream.Position = 0;

        using var bitmap = new System.Drawing.Bitmap(memStream);
        using var engine = new TesseractEngine(_tessDataPath, "eng", EngineMode.Default);
        using var page = engine.Process(bitmap);

        return page.GetText();
    }
    
}