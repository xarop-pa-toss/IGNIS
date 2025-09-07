using Tesseract;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace IGNIS;

public static class OcrService
{
    private static readonly string TessDataPath = Path.Combine("Assets");
    

    public static string ExtractDigitsFromImage(TesseractEngine tesseract, Image image)
    {
        using var memStream = new MemoryStream();
        image.SaveAsPng(memStream);
        memStream.Position = 0;

        using var pix = Pix.LoadFromMemory(memStream.ToArray());
        using var page = tesseract.Process(pix);

        var textResult = page.GetText();
        var onlyDigits = Regex.Replace(textResult, @"\D", "");
        
        return onlyDigits?.Trim() ?? string.Empty;
    }
}