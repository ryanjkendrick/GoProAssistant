using System.Collections.Generic;
using System.Threading.Tasks;
using FFMediaToolkit.Decoding;
using FFMediaToolkit.Encoding;
using FFMediaToolkit.Graphics;
using GoProAssistant.VideoEdit.Extensions;
using SixLabors.ImageSharp;
using SixLabors.Fonts;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Linq;
using FFmpeg.AutoGen;
using System;
using SixLabors.ImageSharp.Formats;
//using VisioForge.Controls.VideoEdit;
//using VisioForge.Types;
//using VisioForge.Types.OutputFormat;
//using VisioForge.Types.VideoEffects;

namespace GoProAssistant.VideoEdit
{
    public class VidEdit
    {
        //public async Task AddTextToVideoAsync(string inputFile, string outputFile, TextOverlay[] textOverlays)
        //{
        //    var core = new VideoEditCore();

        //    core.Input_AddVideoFile(inputFile);

        //    core.Output_Format = new VFMP4v11Output();
        //    core.Output_Filename = outputFile;

        //    core.Video_Renderer.VideoRendererInternal = VFVideoRendererInternal.None;
        //    core.Video_Effects_Enabled = true;

        //    foreach (var overlay in textOverlays)
        //    {
        //        var textOverlay = new VFVideoEffectTextLogo(true, overlay.Text, overlay.StartTime, overlay.StopTime);

        //        core.Video_Effects_Add(textOverlay);
        //    }

        //    await core.StartAsync();
        //}

        //public void AddTextToVideoAsync(string inputFile, string outputFile, TextOverlay[] textOverlays)
        //{
        //    var file = MediaFile.Open(inputFile);

        //    int i = 0;
        //    TextOverlay currentOverlay = textOverlays[i++];
        //    List<VideoSection> videoSections = new List<VideoSection>();
        //    List<Image<Bgr24>> frames = new List<Image<Bgr24>>();


        //    while (file.Video.TryGetNextFrame(out ImageData imageData))
        //    {
        //        Font font = SystemFonts.CreateFont("Arial", 10);
        //        var img = imageData.ToBitmap();

        //        var newFrame = img.Clone(ctx => ctx.DrawText(currentOverlay.Text, font, Color.Black, new PointF(5, 5)));

        //        frames.Add(newFrame);

        //        if (file.Video.Position == currentOverlay.StopTime)
        //        {
        //            videoSections.Add(new VideoSection
        //            {
        //                Text = currentOverlay.Text,
        //                Frames = frames.ToArray()
        //            });
        //            frames = new List<Image<Bgr24>>();

        //            currentOverlay = textOverlays[i++];
        //        }
        //    }

        //    var settings = new VideoEncoderSettings(width: 1920, height: 1080, framerate: 30, codec: VideoCodec.H264);
        //    settings.EncoderPreset = EncoderPreset.Fast;
        //    settings.CRF = 17;

        //    using (var newFile = MediaBuilder.CreateContainer(outputFile).WithVideo(settings).Create())
        //    {
        //        foreach (var frame in videoSections.SelectMany(x => x.Frames))
        //        {
        //            newFile.Video.AddFrame(frame, 0);
        //        }
        //    }
        //}

        //private ImageData ConvertToImageData(Image<Bgr24> img)
        //{
        //    byte[] data = Convert.FromBase64String(img.ToBase64String());
        //    ImageData.FromArray(data, ImagePixelFormat.Bgr24, img.Size().);

        //    var imgData = new ImageData
        //    {
        //        Da;
        //    img.TryGetSinglePixelSpan(out Span<byte> data);

        //    imgData.Data = data;
        //}
    }
}
