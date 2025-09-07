using System.Net.Mime;
using SixLabors.ImageSharp;
using EnvironmentName = Microsoft.AspNetCore.Hosting.EnvironmentName;
namespace IGNIS.Models;

public class StatsRectangles
{
    public Rectangle PlayerName { get; set; }
    public Rectangle Kills { get; set; }
    public Rectangle Accuracy { get; set; }
    public Rectangle ShotsFired { get; set; }
    public Rectangle ShotsHit { get; set; }
    public Rectangle Deaths { get; set; }
    public Rectangle StimsUsed { get; set; }
    public Rectangle Accidentals { get; set; }
    public Rectangle SamplesExtracted { get; set; }
    public Rectangle StratagemsUsed { get; set; }
    public Rectangle MeleeKills { get; set; }
    public Rectangle TimesReinforcing { get; set; }
    public Rectangle FriendlyFireDamage { get; set; }
}