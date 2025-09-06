namespace IGNIS.Models;

public class StatsOutDTO
{
    public string StatsHash { get; init; }
    public required List<PlayerStats> PlayerStats { get; set; }
    public DateTime CreatedOn { get; }

    public StatsOutDTO()
    {
        CreatedOn = DateTime.Now;
    }
}