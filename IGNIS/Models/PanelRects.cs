using IGNIS.Constants;
using SixLabors.ImageSharp;
namespace IGNIS.Models;

public class PanelRects
{
    internal Rectangle FullPanelRect;
    internal Rectangle PlayerNameRect;
    internal Rectangle FullStatsRect;
    internal SplitStatsRects SplitStatsRect;
    
    public PanelRects(Rectangle fullPanelRect, Rectangle playerNameRect, Rectangle fullStatsRect, SplitStatsRects splitStatsRect)
    {
        FullPanelRect = fullPanelRect;
        PlayerNameRect = playerNameRect;
        FullStatsRect = fullStatsRect;
        SplitStatsRect = splitStatsRect;
    }
}