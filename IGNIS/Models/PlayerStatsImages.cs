using SixLabors.ImageSharp;
namespace IGNIS.Models;

public class PlayerPanelImages
{
    public required string PlayerName { get; set; }
    public Image Kills { get; set; }
    public Image Accuracy { get; set; }
    public Image ShotsFired { get; set; }
    public Image ShotsHit { get; set; }
    public Image Deaths { get; set; }
    public Image StimsUsed { get; set; }
    public Image Accidentals { get; set; }
    public Image SamplesExtracted { get; set; }
    public Image StratagemsUsed { get; set; }
    public Image MeleeKills { get; set; }
    public Image TimesReinforcing { get; set; }
    public Image FriendlyFireDamage { get; set; }
}