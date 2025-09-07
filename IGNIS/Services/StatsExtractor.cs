using IGNIS.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
namespace IGNIS;

public class StatsExtractor
{
    private Image _image;

    public StatsExtractor(Image image)
    {
        _image = image;
    }

    public List<string> ExtractPlayerNames()
    {
        var ocrService = new OcrService();
        
    }
    
    public async Task<List<Stats>> ProcessStatImages(List<StatsImages> x)
    {
        
    }
}
    