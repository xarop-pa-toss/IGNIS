using IGNIS.Constants;
using IGNIS.Models;
using SixLabors.ImageSharp;
namespace IGNIS.Helpers;

public class RectangleHelper
{
    public static Rectangle ScaleRect(Rectangle rect, double scaleX, double scaleY)
    {
        return new Rectangle(
            (int)(rect.X * scaleX),
            (int)(rect.Y * scaleY),
            (int)(rect.Width * scaleX),
            (int)(rect.Height * scaleY)
        );
    }
    
    public static SplitStatsRects CreateSplitStatsRect(Rectangle statsRect)
    {
        var slices = new List<Rectangle>();
        int sliceHeight = statsRect.Height / 12;

        for (int i = 0; i < 12; i++)
        {
            int y = statsRect.Y + i * sliceHeight;
            slices.Add(new Rectangle(statsRect.X, y, statsRect.Width, sliceHeight));
        }
        
        var statsRects = new SplitStatsRects()
        {
            Kills = slices[0],
            Accuracy = slices[1],
            ShotsFired = slices[2],
            ShotsHit = slices[3],
            Deaths = slices[4],
            StimsUsed = slices[5],
            Accidentals = slices[6],
            SamplesExtracted = slices[7],
            StratagemsUsed = slices[8],
            MeleeKills = slices[9],
            TimesReinforcing = slices[10],
            FriendlyFireDamage = slices[11]
        };
        
        return statsRects;
    }
    
    public static Dictionary<int, PanelRects> CreateFinalRects(int imgWidth, int imgHeight)
    {
        // Scale vs base 1920x1080
        var scaleX = (double)imgWidth / 1920;
        var scaleY = (double)imgHeight / 1080;

        var scaled = new Dictionary<int, PanelRects>();
        foreach (var (key, p) in CachedPanelRects.FinalRects1920x1080)
        {
            Rectangle fullRect = RectangleHelper.ScaleRect(p.FullPanelRect, scaleX, scaleY);
            Rectangle playerNameRect = RectangleHelper.ScaleRect(p.PlayerNameRect, scaleX, scaleY);
            Rectangle fullStatsRect = RectangleHelper.ScaleRect(p.FullStatsRect, scaleX, scaleY);
            SplitStatsRects splitStatsRect = RectangleHelper.CreateSplitStatsRect(fullStatsRect);
            
            scaled[key] = new PanelRects(
                fullRect,
                playerNameRect,
                fullStatsRect,
                splitStatsRect
            );
        }

        return scaled;
    }
}