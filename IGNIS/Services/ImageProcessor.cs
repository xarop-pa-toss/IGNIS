using IGNIS.Constants;
using IGNIS.Helpers;
using IGNIS.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
namespace IGNIS.Services;

public static class ImageProcessor
{
    #region Load, Resize, Prepare Image
    public static Image LoadAndPrepare(Stream imgStream, int finWidth = 1980)
    {
        var image = LoadImage(imgStream);
        image.Prepare();

        return image;
    }

    private static Image LoadImage(Stream imageStream)
    {
        try
        {
            var image = Image.Load(imageStream);
            return image;
        }
        catch (InvalidImageContentException e1)
        {
            throw new ImageHelper.ImageHandlerException("The file content is not a valid image.", e1);
        }
        catch (UnknownImageFormatException e2)
        {
            throw new ImageHelper.ImageHandlerException("The file format is invalid.", e2);
        }
        catch (Exception ex)
        {
            throw new ImageHelper.ImageHandlerException("An unknown error occurred while loading the image.", ex);
        }
    }

    private static void Prepare(this Image image)
    {
        try
        {
            // int targetHeight = (int)(image.Height * (targetWidth / (float)image.Width));
            image.Mutate(x =>
            {
                x.Grayscale();
                x.GaussianBlur(0.0f);
                x.BinaryThreshold(0.5f);
                x.Contrast(1.2f);
            });
        }
        catch (ObjectDisposedException e1)
        {
            throw new ImageHelper.ImageHandlerException("Image object was disposed of before resizing could be performed.", e1);
        }
        catch (ImageProcessingException e2)
        {
            throw new ImageHelper.ImageHandlerException("Image resizing failed.", e2);
        }
        catch (Exception ex)
        {
            throw new ImageHelper.ImageHandlerException("An unknown error occurred while resizing the image.", ex);
        }
    }
    #endregion

    public static Dictionary<int, PanelImages> ExtractAllPlayerPanels(Image image)
    {
        int imgWidth = image.Width;
        int imgHeight = image.Height;

        // Get cached rectangles
        var rects = CachedPanelRects.GetCachedRectsForDimensions(imgWidth, imgHeight);
        if (rects == null)
        {
            rects = RectangleHelper.CreateFinalRects(imgWidth, imgHeight);
        }

        var panels = new Dictionary<int, PanelImages>();

        // Create panels for all 4 player positions
        for (int i = 1; i <= 4; i++)
        {
            var panelRect = rects[i];
            panels[i] = new PanelImages
            {
                PlayerPosition = i,
                FullPanel = ImageHelper.GetImageFromRectangle(image, panelRect.FullPanelRect),
                PlayerName = ImageHelper.GetImageFromRectangle(image, panelRect.PlayerNameRect)
            };
        }

        return panels;
    }


    public static void FillStatsImages(PanelImages panelImage, Dictionary<int, PanelRects> rects)
    {
        var panelRectangles = rects[panelImage.PlayerPosition].SplitStatsRect;

        try
        {
            panelImage.Kills = ImageHelper.GetImageFromRectangle(panelImage.FullPanel, panelRectangles.Kills);;
            panelImage.Accuracy = ImageHelper.GetImageFromRectangle(panelImage.FullPanel, panelRectangles.Accuracy);
            panelImage.ShotsFired = ImageHelper.GetImageFromRectangle(panelImage.FullPanel, panelRectangles.ShotsFired);
            panelImage.ShotsHit = ImageHelper.GetImageFromRectangle(panelImage.FullPanel, panelRectangles.ShotsHit);
            panelImage.Deaths = ImageHelper.GetImageFromRectangle(panelImage.FullPanel, panelRectangles.Deaths);
            panelImage.StimsUsed = ImageHelper.GetImageFromRectangle(panelImage.FullPanel, panelRectangles.StimsUsed);
            panelImage.Accidentals = ImageHelper.GetImageFromRectangle(panelImage.FullPanel, panelRectangles.Accidentals);
            panelImage.SamplesExtracted = ImageHelper.GetImageFromRectangle(panelImage.FullPanel, panelRectangles.SamplesExtracted);
            panelImage.StratagemsUsed = ImageHelper.GetImageFromRectangle(panelImage.FullPanel, panelRectangles.StratagemsUsed);
            panelImage.MeleeKills = ImageHelper.GetImageFromRectangle(panelImage.FullPanel, panelRectangles.MeleeKills);
            panelImage.TimesReinforcing = ImageHelper.GetImageFromRectangle(panelImage.FullPanel, panelRectangles.TimesReinforcing);
            panelImage.FriendlyFireDamage = ImageHelper.GetImageFromRectangle(panelImage.FullPanel, panelRectangles.FriendlyFireDamage);
        }
        catch (ImageProcessingException e1)
        {
            throw new ImageHelper.ImageHandlerException("An unknown error occurred while cropping the image.", e1);
        }
    }
}