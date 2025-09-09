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
            var playerNameExtracted = OcrService.GetStringFromImage(_tesseract, panel.PlayerName);
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

            stats.Add(new Stats()
            {
                PlayerPosition = player.Key,
                PlayerName = player.Value,
                Kills = (int)OcrService.GetFloatFromImage(_tesseract, statsImages.PlayerName),
                Accuracy = OcrService.GetFloatFromImage(_tesseract, statsImages.Accuracy),
                ShotsFired = (int)OcrService.GetFloatFromImage(_tesseract, statsImages.ShotsFired),
                ShotsHit = (int)OcrService.GetFloatFromImage(_tesseract, statsImages.ShotsFired),
                Deaths = (int)OcrService.GetFloatFromImage(_tesseract, statsImages.Deaths),
                StimsUsed = (int)OcrService.GetFloatFromImage(_tesseract, statsImages.StimsUsed),
                Accidentals = (int)OcrService.GetFloatFromImage(_tesseract, statsImages.Accidentals),
                SamplesExtracted = (int)OcrService.GetFloatFromImage(_tesseract, statsImages.SamplesExtracted),
                StratagemsUsed = (int)OcrService.GetFloatFromImage(_tesseract, statsImages.StratagemsUsed),
                MeleeKills = (int)OcrService.GetFloatFromImage(_tesseract, statsImages.MeleeKills),
                TimesReinforcing = (int)OcrService.GetFloatFromImage(_tesseract, statsImages.TimesReinforcing),
                FriendlyFireDamage = (int)OcrService.GetFloatFromImage(_tesseract, statsImages.FriendlyFireDamage)
            });
        }

        return stats;
    }
}