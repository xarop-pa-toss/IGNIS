using IGNIS.Models;
using IGNIS.Services;
using Microsoft.AspNetCore.Components;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Tesseract;

namespace IGNIS;

public class StatsExtractor
{
    private readonly Image _image;
    private readonly TesseractEngine _tesseract;
    private readonly Dictionary<int, PanelImages> _allPanelImages = new Dictionary<int, PanelImages>();

    public StatsExtractor(TesseractEngine tesseract, Image image)
    {
        _image = image;
        _tesseract = tesseract;
    }

    private Dictionary<int, string> ExtractPlayerPositionsAndNames(List<PanelImages> imgPanels)
    {
        var playerNames = new Dictionary<int, string>();

        foreach (var panel in imgPanels)
        {
            var playerNameExtracted = OcrService.ExtractStringFromImage(_tesseract, panel.PlayerName);
            if (string.IsNullOrEmpty(playerNameExtracted))
            {
                continue;
            }

            playerNames.Add(panel.PlayerPosition, playerNameExtracted);
        }

        return playerNames;
    }

    public List<Stats> ExtractStats()
    {
        var stats = new List<Stats>();
        var allPanelImages = ImageProcessor.CreatePanelsWithPlayerInfo(_image);
        var playerPositionAndName = ExtractPlayerPositionsAndNames(allPanelImages);

        foreach (var player in playerPositionAndName)
        {
            var statsImages = ImageProcessor.GetStatsImagesForPlayer(_image, player.Key);
            
            DebugImageExporter.SaveImage(_image, $"player{player.Key}", "FULL");
            DebugImageExporter.SaveImage(statsImages.Kills, $"player{player.Key}", "kills");
            DebugImageExporter.SaveImage(statsImages.Accuracy, $"player{player.Key}", "accuracy");
            DebugImageExporter.SaveImage(statsImages.ShotsFired, $"player{player.Key}", "shots_fired");
            DebugImageExporter.SaveImage(statsImages.ShotsHit, $"player{player.Key}", "shots_hit");
            DebugImageExporter.SaveImage(statsImages.Deaths, $"player{player.Key}", "deaths");
            DebugImageExporter.SaveImage(statsImages.StimsUsed, $"player{player.Key}", "stims");
            DebugImageExporter.SaveImage(statsImages.Accidentals, $"player{player.Key}", "accidentals");
            DebugImageExporter.SaveImage(statsImages.SamplesExtracted, $"player{player.Key}", "samples");
            DebugImageExporter.SaveImage(statsImages.StratagemsUsed, $"player{player.Key}", "stratagems");
            DebugImageExporter.SaveImage(statsImages.MeleeKills, $"player{player.Key}", "melee");
            DebugImageExporter.SaveImage(statsImages.TimesReinforcing, $"player{player.Key}", "reinforces");
            DebugImageExporter.SaveImage(statsImages.FriendlyFireDamage, $"player{player.Key}", "ff_damage");

            
            stats.Add(new Stats()
            {
                PlayerPosition = player.Key,
                PlayerName = player.Value,
                Kills = (int)OcrService.ExtractFloatFromImage(_tesseract, statsImages.PlayerName),
                Accuracy = OcrService.ExtractFloatFromImage(_tesseract, statsImages.Accuracy),
                ShotsFired = (int)OcrService.ExtractFloatFromImage(_tesseract, statsImages.ShotsFired),
                ShotsHit = (int)OcrService.ExtractFloatFromImage(_tesseract, statsImages.ShotsFired),
                Deaths = (int)OcrService.ExtractFloatFromImage(_tesseract, statsImages.Deaths),
                StimsUsed = (int)OcrService.ExtractFloatFromImage(_tesseract, statsImages.StimsUsed),
                Accidentals = (int)OcrService.ExtractFloatFromImage(_tesseract, statsImages.Accidentals),
                SamplesExtracted = (int)OcrService.ExtractFloatFromImage(_tesseract, statsImages.SamplesExtracted),
                StratagemsUsed = (int)OcrService.ExtractFloatFromImage(_tesseract, statsImages.StratagemsUsed),
                MeleeKills = (int)OcrService.ExtractFloatFromImage(_tesseract, statsImages.MeleeKills),
                TimesReinforcing = (int)OcrService.ExtractFloatFromImage(_tesseract, statsImages.TimesReinforcing),
                FriendlyFireDamage = (int)OcrService.ExtractFloatFromImage(_tesseract, statsImages.FriendlyFireDamage)
            });
        }

        return stats;
    }
}