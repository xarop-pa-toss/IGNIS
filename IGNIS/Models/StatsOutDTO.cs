namespace IGNIS.Models;

public class StatsOutDTO
{
    public string StatsHash { get; init; }
    public Stats? Player1Stats { get; set; }
    public Stats? Player2Stats { get; set; }
    public Stats? Player3Stats { get; set; }
    public Stats? Player4Stats { get; set; }
    public DateTime CreatedOn { get; }

    public StatsOutDTO()
    {
        CreatedOn = DateTime.Now;
    }
}