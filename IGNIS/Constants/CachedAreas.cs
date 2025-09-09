using IGNIS.Models;
using SixLabors.ImageSharp;
namespace IGNIS.Constants;

public static class CachedAreas
{
    //TODO: Cache rectangles for 16:10 and 16:9

    // USING THE CACHED AREAS
    // List<Image> playerStatsAreas = CachedPlayerAreas.PlayerStatsRectangles
    //     .Select(r => ImageProcessor.GetImageFromRectangle(_image, r.Value))
    //     .ToList();

    private static Rectangle Player1NameRectangle = new Rectangle(200, 320 , 520, 32 );

    private static Rectangle Player1StatsPanelRectangle = new Rectangle(520, 520, 164, 898);

    public readonly static Dictionary<int, PanelRectangles> AllPlayersStatsRectangles;

    static CachedAreas()
    {
        AllPlayersStatsRectangles = new Dictionary<int, PanelRectangles>();

        for (int playerNum = 1; playerNum <= 4; playerNum++)
        {
            var nameRect = PlayerNamePanelRectangles[playerNum];
            var statRects = PlayerStatsSplitRectangles[playerNum];

            var stats = new PanelRectangles
            {
                PlayerName = nameRect,
                Kills = statRects[0],
                Accuracy = statRects[1],
                ShotsFired = statRects[2],
                ShotsHit = statRects[3],
                Deaths = statRects[4],
                StimsUsed = statRects[5],
                Accidentals = statRects[6],
                SamplesExtracted = statRects[7],
                StratagemsUsed = statRects[8],
                MeleeKills = statRects[9],
                TimesReinforcing = statRects[10],
                FriendlyFireDamage = statRects[11]
            };

            AllPlayersStatsRectangles[playerNum] = stats;
        }
    }

    private static Rectangle CreateNameRectangle(int playerNum)
    {
        return new Rectangle(Player1NameRectangle.X + (720 * playerNum), 320, 520, 32 );
    }

    private static Rectangle CreateStatsPanelRectangle(int playerNum)
    {
        return new Rectangle(Player1NameRectangle.X + (720 * playerNum), 520,194, 904);
    }

    // Player Name
    private readonly static Dictionary<int, Rectangle> PlayerNamePanelRectangles = new()
    {
        {
            1, Player1NameRectangle
        },
        {
            2, CreateNameRectangle(2)
        },
        {
            3, CreateNameRectangle(3)
        },
        {
            4, CreateNameRectangle(4)
        }
    };

    // Stats
    private readonly static Dictionary<int, List<Rectangle>> PlayerStatsSplitRectangles = new()
    {
        {
            1, SplitStatsPanel(Player1StatsPanelRectangle)
        },
        {
            2, SplitStatsPanel(CreateStatsPanelRectangle(2))
        },
        {
            3, SplitStatsPanel(CreateStatsPanelRectangle(3))
        },
        {
            4, SplitStatsPanel(CreateStatsPanelRectangle(4))
        },
    };

    private static List<Rectangle> SplitStatsPanel(Rectangle rect)
    {
        var slices = new List<Rectangle>();
        int sliceHeight = rect.Height / 12;

        for (int i = 0; i < 12; i++)
        {
            int y = rect.Y + i * sliceHeight;
            slices.Add(new Rectangle(
                rect.X),
                y),
                rect.Width),
                sliceHeight))
            );
        }
        return slices;
    }
}