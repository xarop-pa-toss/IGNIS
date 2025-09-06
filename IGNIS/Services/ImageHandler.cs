using IGNIS.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
namespace IGNIS;

public class ImageHandler
{
    private readonly Image _image;

    public ImageHandler(Stream imageStream)
    {
        _image = LoadImage(imageStream);
    }
    
    var playerStats = new StatsExtractor(_image);
    
    internal Image LoadImage(Stream imageStream)
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
            throw new ImageHandlerException("An unknown error occurred while loading the image file.", ex);
        }
    }
    
    internal void Resize(int finWidth = 1500)
    {
        // Resize to intended width while maintaining original aspect ratio
        int finHeight = (int)(_image.Height * (finWidth / (float)_image.Width));

        try
        {
            _image.Mutate(s => s.Resize(finWidth, finHeight));
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

    internal void PrepareForProcessing()
    {
        _image.Mutate(s =>
        {
            s.Grayscale();
            s.GaussianBlur(0.5f);
            s.BinaryThreshold(0.5f);
            s.Contrast(1.1f);
            s.Resize(_image.Width * 2, _image.Height * 2);
        });
    }

    internal int GetPlayerCount()
    {
        return StatsExtractor.GetPlayerCount(_image);
    }
    internal List<Image> SplitPerPlayer ()
    {
        Resize();
        
        
        
        
        //TODO: Split image into the expected chunks
    }

}

public class ImageHandlerException : Exception
{
    public ImageHandlerException(string message, Exception inner) : base(message, inner) { }
}