using SixLabors.ImageSharp;
namespace IGNIS;

public class StatsExtractor
{
    private int _panelWidth;
    private int _panelHeight;
    private Dictionary<string, Rectangle> base

    public StatsExtractor(Image fullImage)
    {
        _panelHeight = fullImage.Height;
    }
    private int GetPlayerAmount()
}