using IGNIS.Constants;
using IGNIS.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
namespace IGNIS;

public static class ImageProcessor
{
    public static Image LoadAndPrepare(Stream imgStream, int finWidth = 1500)
    {
        var image = LoadImage(imgStream);
        ResizeAndPrepare(image, finWidth);

        return image;
    }

    private static Image LoadImage(Stream imageStream)
    {
        try
        {
            return Image.Load(imageStream);
        }
        catch (InvalidImageContentException e1)
        {
            throw new ImageHandlerException("The file content is not a valid image.", e1);
        }
        catch (UnknownImageFormatException e2)
        {
            throw new ImageHandlerException("The file format is invalid.", e2);
        }
        catch (Exception ex)
        {
            throw new ImageHandlerException("An unknown error occurred while loading the image.", ex);
        }
    }

    private static void ResizeAndPrepare(Image image, int targetWidth)
    {
        try
        {
            int targetHeight = (int)(image.Height * (targetWidth / (float)image.Width));
            image.Mutate(x =>
            {
                x.Resize(targetWidth, targetHeight);
                x.Grayscale();
                x.GaussianBlur(0.5f);
                x.BinaryThreshold(0.5f);
                x.Contrast(1.1f);
                x.Resize(image.Width * 2, image.Height * 2);
            });
        }
        catch (ObjectDisposedException e1)
        {
            throw new ImageHandlerException("Image object was disposed of before resizing could be performed.", e1);
        }
        catch (ImageProcessingException e2)
        {
            throw new ImageHandlerException("Image resizing failed.", e2);
        }
        catch (Exception ex)
        {
            throw new ImageHandlerException("An unknown error occurred while resizing the image.", ex);
        }
    }

    public static Image GetImageFromRectangle(Image image, Rectangle cropRectangle)
    {
        try
        {
            return image.Clone(img => img.Crop(cropRectangle));
        }
        catch (ImageProcessingException e1)
        {
            throw new ImageHandlerException("An unknown error occurred while cropping the image.", e1);
        }
    }
    
    public static Dictionary<int, Image> GetPlayerNameImagesDict(Image image)
    {
        return new Dictionary<int, Image>()
        {
            {1, GetImageFromRectangle(image, CachedAreas.AllPlayersStatsRectangles[1].PlayerName) },
            {2, GetImageFromRectangle(image, CachedAreas.AllPlayersStatsRectangles[2].PlayerName) },
            {3, GetImageFromRectangle(image, CachedAreas.AllPlayersStatsRectangles[3].PlayerName) },
            {4, GetImageFromRectangle(image, CachedAreas.AllPlayersStatsRectangles[4].PlayerName) }
        };
    }
    
    public static void GetStatsImagesFromPanelImage(Image image, PanelImages panelImage, int playerNum)
    {
        var panelRectangles = CachedAreas.AllPlayersStatsRectangles[playerNum];
        try
        {
            panelImage.Kills = GetImageFromRectangle(image, panelRectangles.Kills);
            panelImage.Accuracy = GetImageFromRectangle(image, panelRectangles.Accuracy);
            panelImage.ShotsFired = GetImageFromRectangle(image, panelRectangles.ShotsFired);
            panelImage.ShotsHit = GetImageFromRectangle(image, panelRectangles.ShotsHit);
            panelImage.Deaths = GetImageFromRectangle(image, panelRectangles.Deaths);
            panelImage.StimsUsed = GetImageFromRectangle(image, panelRectangles.StimsUsed);
            panelImage.Accidentals = GetImageFromRectangle(image, panelRectangles.Accidentals);
            panelImage.SamplesExtracted = GetImageFromRectangle(image, panelRectangles.SamplesExtracted);
            panelImage.StratagemsUsed = GetImageFromRectangle(image, panelRectangles.StratagemsUsed);
            panelImage.MeleeKills = GetImageFromRectangle(image, panelRectangles.MeleeKills);
            panelImage.TimesReinforcing = GetImageFromRectangle(image, panelRectangles.TimesReinforcing);
            panelImage.FriendlyFireDamage = GetImageFromRectangle(image, panelRectangles.FriendlyFireDamage);
         }
        catch (ImageProcessingException e1)
        {
            throw new ImageHandlerException("An unknown error occurred while cropping the image.", e1);
        }
    }
}

public class ImageHandlerException(string message, Exception inner) : Exception(message, inner);