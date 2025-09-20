using System.ComponentModel;
using IGNIS.Helpers;
using SixLabors.ImageSharp;
using IGNIS.Models;

namespace IGNIS.Constants;

public static class CachedPanelRects
{
    private readonly static Rectangle PlayerNameRect1920X1080 = new Rectangle(0, 0, 350, 35);
    private readonly static Rectangle StatsRect1920X1080 = new Rectangle(218, 134, 106, 576);
    private readonly static SplitStatsRects SplitStatsRect1920x1080 = RectangleHelper.CreateSplitStatsRect(StatsRect1920X1080);
    
    public readonly static Dictionary<int, PanelRects> FinalRects1920x1080;
    public readonly static Dictionary<int, PanelRects> FinalRects2160x1080;
    public readonly static Dictionary<int, PanelRects> FinalRects3840x1080;
    public readonly static Dictionary<int, PanelRects> FinalRects1920x1200;
    
    static CachedPanelRects()
    {
        FinalRects1920x1080 = CreateFinalRects1920x1080();
        FinalRects2160x1080 = RectangleHelper.CreateFinalRects(2160, 1080);
        FinalRects3840x1080 = RectangleHelper.CreateFinalRects(3840, 1080);
        FinalRects1920x1200 = RectangleHelper.CreateFinalRects(1920, 1200);
    }

    private static Dictionary<int, PanelRects> CreateFinalRects1920x1080()
    {
        return new()
        {
            {
                1, new PanelRects(
                    new Rectangle(125, 198, 350, 710),
                    PlayerNameRect1920X1080,
                    StatsRect1920X1080,
                    SplitStatsRect1920x1080
                )
            },
            {
                2, new PanelRects(
                    new Rectangle(585, 198, 350, 710),
                    PlayerNameRect1920X1080,
                    StatsRect1920X1080,
                    SplitStatsRect1920x1080
                )
            },
            {
                3, new PanelRects(
                    new Rectangle(1045, 198, 350, 710),
                    PlayerNameRect1920X1080,
                    StatsRect1920X1080,
                    SplitStatsRect1920x1080
                )
            },
            {
                4, new PanelRects(
                    new Rectangle(1505, 198, 350, 710),
                    PlayerNameRect1920X1080,
                    StatsRect1920X1080,
                    SplitStatsRect1920x1080
                )
            }
        };
    }
    
    /// <summary>
    /// Gets the appropriate cached rectangles for the given image dimensions
    /// </summary>
    /// <param name="imgWidth">Image width</param>
    /// <param name="imgHeight">Image height</param>
    /// <returns>Dictionary of cached rectangles, or null if no exact match found</returns>
    public static Dictionary<int, PanelRects>? GetCachedRectsForDimensions(int imgWidth, int imgHeight)
    {
        return (imgWidth, imgHeight) switch
        {
            (1920, 1080) => FinalRects1920x1080,
            (2160, 1080) => FinalRects2160x1080,
            (3840, 1080) => FinalRects3840x1080,
            (1920, 1200) => FinalRects1920x1200,
            _ => null
        };
    }
    
    /// <summary>
    /// Checks if cached rectangles exist for the given image dimensions
    /// </summary>
    /// <param name="imgWidth">Image width</param>
    /// <param name="imgHeight">Image height</param>
    /// <returns>True if cached rectangles exist for these dimensions</returns>
    public static bool HasCachedRectsForDimensions(int imgWidth, int imgHeight)
    {
        return GetCachedRectsForDimensions(imgWidth, imgHeight) != null;
    }

}