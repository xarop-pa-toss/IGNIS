namespace IGNIS.Models;

public class Stats
{
    public int PlayerPosition { get; set; } = -1;
    public string? PlayerName { get; set; } = string.Empty;
    public int? Kills { get; set; } = -1;
    public float? Accuracy { get; set; } = -1;
    public int? ShotsFired { get; set; } = -1;
    public int? ShotsHit { get; set; } = -1;
    public int? Deaths { get; set; } = -1;
    public int? StimsUsed { get; set; } = -1;
    public int? Accidentals { get; set; } = -1;
    public int? SamplesExtracted { get; set; } = -1;
    public int? StratagemsUsed { get; set; } = -1;
    public int? MeleeKills { get; set; } = -1;
    public int? TimesReinforcing { get; set; } = -1;
    public int? FriendlyFireDamage { get; set; } = -1;
}