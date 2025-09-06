using System.Text.Json;
using System.Security.Cryptography;
using System.Text;
using IGNIS.Models;

namespace IGNIS;

public static class StatsHasher
{
    public static string HashStats(List<PlayerStatsDTO> stats)
    {
        // Make sure the order is consistent
        var options = new JsonSerializerOptions { WriteIndented = false };
        string json = JsonSerializer.Serialize(stats, options);

        // Hash with SHA256
        using var sha = SHA256.Create();
        byte[] hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(json));
        return Convert.ToHexString(hashBytes); // e.g. "9F86D081884C7D659A2..."
    }
}
