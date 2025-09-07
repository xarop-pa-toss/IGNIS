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

    public static List<Image> GetPlayerNameImages(Image image)
    {
        return new List<Image>()
        {
            image.Clone(img => img.Crop(CachedAreas.PlayerNamePanelRectangles[1])),
            image.Clone(img => img.Crop(CachedAreas.PlayerNamePanelRectangles[2])),
            image.Clone(img => img.Crop(CachedAreas.PlayerNamePanelRectangles[3])),
            image.Clone(img => img.Crop(CachedAreas.PlayerNamePanelRectangles[4]))    
        };
    }
    
    public static StatsImages GetStatsImagesFromStatsRectangles(Image image, int playerInd, List<StatsRectangles> statsRectangles)
    {
        try
        {
            var statsImages = new StatsImages();
            statsImages.PlayerName = image.Clone(img => img.Crop(statsRectangles[playerInd].PlayerName));
            statsImages.Kills = image.Clone(img => img.Crop(statsRectangles[playerInd].Kills));
            statsImages.Accuracy = image.Clone(img => img.Crop(statsRectangles[playerInd].Accuracy));
            statsImages.ShotsFired = image.Clone(img => img.Crop(statsRectangles[playerInd].ShotsFired));
            statsImages.ShotsHit = image.Clone(img => img.Crop(statsRectangles[playerInd].ShotsHit));
            statsImages.Deaths = image.Clone(img => img.Crop(statsRectangles[playerInd].Deaths));
            statsImages.StimsUsed = image.Clone(img => img.Crop(statsRectangles[playerInd].StimsUsed));
            statsImages.Accidentals = image.Clone(img => img.Crop(statsRectangles[playerInd].Accidentals));
            statsImages.SamplesExtracted = image.Clone(img => img.Crop(statsRectangles[playerInd].SamplesExtracted));
            statsImages.StratagemsUsed = image.Clone(img => img.Crop(statsRectangles[playerInd].StratagemsUsed));
            statsImages.MeleeKills = image.Clone(img => img.Crop(statsRectangles[playerInd].MeleeKills));
            statsImages.TimesReinforcing = image.Clone(img => img.Crop(statsRectangles[playerInd].TimesReinforcing));
            statsImages.FriendlyFireDamage = image.Clone(img => img.Crop(statsRectangles[playerInd].FriendlyFireDamage));
            
            return statsImages; 
         }
        catch (ImageProcessingException e1)
        {
            throw new ImageHandlerException("An unknown error occurred while cropping the image.", e1);
        }
    }
}



public class ImageHandlerException(string message, Exception inner) : Exception(message, inner);