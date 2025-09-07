namespace IGNIS.Models;

public class StatsOutDTO
{
    public string StatsHash { get; init; }
    public required List<Stats> PlayerStats { get; set; }
    public DateTime CreatedOn { get; }

    public StatsOutDTO()
    {
        CreatedOn = DateTime.Now;
    }
}