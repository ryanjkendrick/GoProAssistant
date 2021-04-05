using FFMediaToolkit.Graphics;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace GoProAssistant.VideoEdit.Extensions
{
    public static class ImageDataExtensions
    {
        public static Image<Bgr24> ToBitmap(this ImageData imageData)
        {
            return Image.LoadPixelData<Bgr24>(imageData.Data, imageData.ImageSize.Width, imageData.ImageSize.Height);
        }
    }
}
