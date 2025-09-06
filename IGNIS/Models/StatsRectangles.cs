using System.Net.Mime;
using SixLabors.ImageSharp;
using EnvironmentName = Microsoft.AspNetCore.Hosting.EnvironmentName;
namespace IGNIS.Models;

public static class StatsRectangles
{
    public readonly static Dictionary<string, Rectangle> PanelAreas = new()
    {
        {
            "1", new Rectangle(100, 155, 555, 245)
        },
        {
            "2", new Rectangle(465, 155, 555, 245)
        },
        {
            "3", new Rectangle(820, 155, 555, 245)
        },
        {
            "4", new Rectangle(1183, 155, 555, 245)
        }
    };
    
    public readonly static Dictionary<string, Rectangle> StatsCropRectangles = BuildStatsCropRectangles();

    private static Dictionary<string, Rectangle> BuildStatsCropRectangles()
    {
        var statsCropRectangles = new Dictionary<string, Rectangle>();
        
        // Crop Area 1 is Rectangle 260,260,82,452
        // Each Stat Crop Area is 452/12 = 38px high
        // Based off relative offsets
        var statAreaOffsets = new (string Name, int OffsetY, int Height)[]
        {
            
        }
    }
}