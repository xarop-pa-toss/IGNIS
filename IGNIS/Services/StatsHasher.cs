using System.Text.Json;
using System.Security.Cryptography;
using System.Text;
using IGNIS.Models;

namespace IGNIS.Services;

public static class StatsHasher
{
    public static string HashStats(IEnumerable<Stats> statsList)
    {
        var options = new JsonSerializerOptions { WriteIndented = false };
        string json = JsonSerializer.Serialize(statsList, options);

        using var sha = SHA256.Create();
        byte[] hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(json));
        return Convert.ToHexString(hashBytes);
    }
}
