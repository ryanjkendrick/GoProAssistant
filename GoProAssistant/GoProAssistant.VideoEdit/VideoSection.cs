using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace GoProAssistant.VideoEdit
{
    public class VideoSection
    {
        public string Text { get; set; }
        public Image<Bgr24>[] Frames { get; set; }
    }
}
