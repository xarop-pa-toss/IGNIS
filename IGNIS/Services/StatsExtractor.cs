using SixLabors.ImageSharp;
namespace IGNIS;

public static class StatsExtractor
{
    private` int _panelWidth;
    private int _panelHeight;
    private Dictionary<string, Rectangle> _playerNameCropAreas;

    public static StatsExtractor(Image fullImage)
    {
        _panelHeight = fullImage.Height;
    }
    private int GetPlayerAmount()
}