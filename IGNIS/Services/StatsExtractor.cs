using IGNIS.Constants;
using IGNIS.Helpers;
using IGNIS.Models;
using SixLabors.ImageSharp;
using Tesseract;

namespace IGNIS.Services;

public class StatsExtractor
{
    private readonly Image _image;
    private readonly TesseractEngine _tesseract;

    public StatsExtractor(TesseractEngine tesseract, Image image)
    {
        _image = image;
        _tesseract = tesseract;
    }

    public List<Stats> ExtractStats()
    {
        var stats = new List<Stats>();
 
        // Create all player panel images
        var allPlayerPanelImages = ImageProcessor.ExtractAllPlayerPanels(_image);

        // Filter panels to only include active players
        var activePlayerPanelImages = FilterActivePlayerPanels(allPlayerPanelImages);
        
        // Get rectangles for split stats panels
        int imgWidth = _image.Width;
        int imgHeight = _image.Height;
        var rects = CachedPanelRects.GetCachedRectsForDimensions(imgWidth, imgHeight);
        if (rects == null)
        {
            rects = RectangleHelper.CreateFinalRects(imgWidth, imgHeight);
        }

        foreach (var playerPanel in activePlayerPanelImages)
        {
            // Fill stats images for this player
            ImageProcessor.FillStatsImages(playerPanel.Value, rects);
            
            // Debug: Save images for troubleshooting
            SaveDebugImages(playerPanel.Value);
            
            // Extract stats using OCR
            var playerStats = new Stats()
            {
                PlayerPosition = playerPanel.Value.PlayerPosition,
                PlayerName = OcrService.ExtractStringFromImage(_tesseract, playerPanel.Value.PlayerName),
                Kills = (int)OcrService.ExtractFloatFromImage(_tesseract, playerPanel.Value.Kills),
                Accuracy = OcrService.ExtractFloatFromImage(_tesseract, playerPanel.Value.Accuracy),
                ShotsFired = (int)OcrService.ExtractFloatFromImage(_tesseract, playerPanel.Value.ShotsFired),
                ShotsHit = (int)OcrService.ExtractFloatFromImage(_tesseract, playerPanel.Value.ShotsHit),
                Deaths = (int)OcrService.ExtractFloatFromImage(_tesseract, playerPanel.Value.Deaths),
                StimsUsed = (int)OcrService.ExtractFloatFromImage(_tesseract, playerPanel.Value.StimsUsed),
                Accidentals = (int)OcrService.ExtractFloatFromImage(_tesseract, playerPanel.Value.Accidentals),
                SamplesExtracted = (int)OcrService.ExtractFloatFromImage(_tesseract, playerPanel.Value.SamplesExtracted),
                StratagemsUsed = (int)OcrService.ExtractFloatFromImage(_tesseract, playerPanel.Value.StratagemsUsed),
                MeleeKills = (int)OcrService.ExtractFloatFromImage(_tesseract, playerPanel.Value.MeleeKills),
                TimesReinforcing = (int)OcrService.ExtractFloatFromImage(_tesseract, playerPanel.Value.TimesReinforcing),
                FriendlyFireDamage = (int)OcrService.ExtractFloatFromImage(_tesseract, playerPanel.Value.FriendlyFireDamage)
            };

            stats.Add(playerStats);
        }

        return stats;
    }
    
    private Dictionary<int, PanelImages> FilterActivePlayerPanels(Dictionary<int, PanelImages> allPanels)
    {
        var activePlayers = new Dictionary<int, PanelImages>();
        
        foreach (var panel in allPanels)
        {
            var playerNameText = OcrService.ExtractStringFromImage(_tesseract, panel.Value.PlayerName);
            
            if (!string.IsNullOrWhiteSpace(playerNameText))
            {
                activePlayers[panel.Key] = panel.Value;
            }
        }

        return activePlayers;
    }

    
    private void SaveDebugImages(PanelImages playerImages)
    {
        var playerKey = playerImages.PlayerPosition;
        
        DebugImageExporter.SaveImage(playerImages.FullPanel, $"player{playerKey}", "FULL");
        DebugImageExporter.SaveImage(playerImages.PlayerName, $"player{playerKey}", "player_name");
        DebugImageExporter.SaveImage(playerImages.Kills, $"player{playerKey}", "kills");
        DebugImageExporter.SaveImage(playerImages.Accuracy, $"player{playerKey}", "accuracy");
        DebugImageExporter.SaveImage(playerImages.ShotsFired, $"player{playerKey}", "shots_fired");
        DebugImageExporter.SaveImage(playerImages.ShotsHit, $"player{playerKey}", "shots_hit");
        DebugImageExporter.SaveImage(playerImages.Deaths, $"player{playerKey}", "deaths");
        DebugImageExporter.SaveImage(playerImages.StimsUsed, $"player{playerKey}", "stims");
        DebugImageExporter.SaveImage(playerImages.Accidentals, $"player{playerKey}", "accidentals");
        DebugImageExporter.SaveImage(playerImages.SamplesExtracted, $"player{playerKey}", "samples");
        DebugImageExporter.SaveImage(playerImages.StratagemsUsed, $"player{playerKey}", "stratagems");
        DebugImageExporter.SaveImage(playerImages.MeleeKills, $"player{playerKey}", "melee");
        DebugImageExporter.SaveImage(playerImages.TimesReinforcing, $"player{playerKey}", "reinforces");
        DebugImageExporter.SaveImage(playerImages.FriendlyFireDamage, $"player{playerKey}", "ff_damage");
    }

}