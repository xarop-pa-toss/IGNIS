namespace IGNIS.Models;

public class StatsOutDTO
{
    public required string StatsHash { get; init; }
    public List<Stats> Stats { get; set; }
    public required DateTime CreatedOn { get; init; }
}