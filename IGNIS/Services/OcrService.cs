using Tesseract;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace IGNIS.Services;

public static class OcrService
{
    private readonly static string TessDataPath = Path.Combine("Assets");

    
    public static string ExtractStringFromImage(TesseractEngine tesseract, Image? image)
    {
        var extractedText  = GetTextFromImage(tesseract, image);
        if (extractedText == null)
        {
            DebugImageExporter.SaveImage(image, $"FAILED", $"{Guid.NewGuid()}");
            return string.Empty;
        }  
        return extractedText;
    }

    public static float ExtractFloatFromImage(TesseractEngine tesseract, Image? image)
    {
        var extractedText = GetTextFromImage(tesseract, image);
        if (string.IsNullOrEmpty(extractedText))
        {
            
            return -1f;
        }
        var textWithReplacedOForZero = Regex.Replace(extractedText, @"[oO]", "0");
        var onlyDigits = Regex.Replace(textWithReplacedOForZero, @"[^0-9.,]", "").Trim();
        if (float.TryParse(onlyDigits, out var result))
        {
            return result;
        }

        return -1f;
    }

    private static string GetTextFromImage(TesseractEngine tesseract, Image? image)
    {
        if (image == null)
        {
            return null;
        }

        using var memStream = new MemoryStream();
        image.SaveAsPng(memStream);
        memStream.Position = 0;

        using var pix = Pix.LoadFromMemory(memStream.ToArray());
        using var page = tesseract.Process(pix);

        return page.GetText();
    }
}