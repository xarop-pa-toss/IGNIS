using IGNIS.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Tesseract;

namespace IGNIS;

public class StatsExtractor
{
    private Image _image;
    private readonly TesseractEngine _tesseract;

    public StatsExtractor(TesseractEngine tesseract, Image image)
    {
        _image = image;
        _tesseract = tesseract
    }

    public Dictionary<int, string> ExtractPlayerNames()
    {
        var playerNameImages = ImageProcessor.GetPlayerNameImagesDict(_image);
        var playerNames = new Dictionary<int, string>();
        
        for(int i = 0; i < playerNameImages.Count; i++)
        {
            var playerNameExtracted = OcrService.ExtractDigitsFromImage(_tesseract, playerNameImages[i]);
            if (string.IsNullOrEmpty(playerNameExtracted)) { continue; }
            
            playerNames.Add(i + 1, playerNameExtracted);
        }

        return playerNames;
    }
    
    public async Task<List<Stats>> ExtractStats(List<StatsImages> x)
    {
        //TODO: extract stats
    }
}
    