using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
namespace IGNIS.Helpers;

public abstract class ImageHelper
{
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
    
    public class ImageHandlerException(string message, Exception inner) : Exception(message, inner);
}