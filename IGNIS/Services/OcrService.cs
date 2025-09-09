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

    
    public static string GetStringFromImage(TesseractEngine tesseract, Image? image)
    {
        var tesseractPage = PrepareImageIntoPage(tesseract, image);
        if (tesseractPage == null)
        {
            return string.Empty;
        }
        
        return tesseractPage.GetText();
    }

    public static float GetFloatFromImage(TesseractEngine tesseract, Image? image)
    {
        var tesseractPage = PrepareImageIntoPage(tesseract, image);
        if (tesseractPage == null)
        {
            return -1;
        }
        
        var textResult = tesseractPage.GetText();
        var onlyDigits = Regex.Replace(textResult, @"[^0-9.,]", "").Trim();

        if (float.TryParse(onlyDigits, out var result))
        {
            return result;
        }

        return -1;
    }

    private static Page PrepareImageIntoPage(TesseractEngine tesseract, Image? image)
    {
        if (image == null)
        {
            return null;
        }

        using var memStream = new MemoryStream();
        image.SaveAsPng(memStream);
        memStream.Position = 0;

        using var pix = Pix.LoadFromMemory(memStream.ToArray());
        return tesseract.Process(pix);
    }
}