using System;
using System.IO;
using System.Linq;

using GoProAssistant.Shared.VideoEditing;
using GoProAssistant.Shared.VideoRecording;

using Foundation;
using AVFoundation;
using CoreAnimation;
using CoreGraphics;
using CoreFoundation;
using CoreMedia;
using UIKit;

using Xamarin.Forms;

namespace GoProAssistant.iOS.VideoEditing
{
    public class VideoEditor : IVideoEditor
    {
        public VideoEditor()
        {
        }

        public void AddTextToVideo(string inputFile, string outputFile, TextOverlay[] textOverlays)
        {
            var videoAsset = new AVUrlAsset(NSUrl.FromFilename(inputFile));
            var result = AddTextForVideo(
                videoAsset,
                textOverlays.First().Text,
                36,
                UIColor.Red,
                new CGRect(100, 100, 200, 40));

            var session = AssetExportSessionWithPreset(AVAssetExportSession.PresetHighestQuality, result.Item1, result.Item2);

            if (File.Exists(outputFile))
            {
                File.Delete(outputFile);
            }

            // If a preset that is not compatible with AVFileTypeQuickTimeMovie is used, one can use -[AVAssetExportSession supportedFileTypes] to obtain a supported file type for the output file and UTTypeCreatePreferredIdentifierForTag to obtain an appropriate path extension for the output file type.
            session.OutputUrl = NSUrl.FromFilename(outputFile);
            session.OutputFileType = AVFileType.Mpeg4;

            session.ExportAsynchronously(() => DispatchQueue.MainQueue.DispatchAsync(() => Shell.Current.DisplayAlert("Export", "Video export has finished", "OK")));
        }

        private Tuple<AVMutableComposition, AVMutableVideoComposition> AddTextForVideo(
            AVUrlAsset videoAsset,
            string text,
            nfloat fontSize,
            UIColor fontColor,
            CGRect textRect)
        {
            var videoTrack = videoAsset.TracksWithMediaType(AVMediaType.Video).First();
            var audioTrack = videoAsset.TracksWithMediaType(AVMediaType.Audio).First();

            var imageComposition = new AVMutableComposition();
            var compositionVideoTrack = imageComposition.AddMutableTrack(AVMediaType.Video, 0); // kCMPersistentTrackID_Invalid

            if (compositionVideoTrack == null)
                throw new NullReferenceException(nameof(compositionVideoTrack));

            var compositionAudioTrack = imageComposition.AddMutableTrack(AVMediaType.Audio, 0); // kCMPersistentTrackID_Invalid

            if (compositionAudioTrack == null)
                throw new NullReferenceException(nameof(compositionAudioTrack));

            compositionVideoTrack.InsertTimeRange(
                new CMTimeRange
                {
                    Start = CMTime.Zero,
                    Duration = videoAsset.Duration
                },
                videoTrack,
                CMTime.Zero,
                out NSError compVidError);

            if (compVidError != null)
                throw new Exception(compVidError.ToString());

            compositionAudioTrack.InsertTimeRange(
                new CMTimeRange
                {
                    Start = CMTime.Zero,
                    Duration = videoAsset.Duration
                },
                audioTrack,
                CMTime.Zero,
                out NSError compAudioError);

            if (compAudioError != null)
                throw new Exception(compAudioError.ToString());

            var videoComposition = new AVMutableVideoComposition();
            var mainInstruction = new AVMutableVideoCompositionInstruction();
            mainInstruction.TimeRange = new CMTimeRange
            {
                Start = CMTime.Zero,
                Duration = videoAsset.Duration
            };

            var layerInstruction = AVMutableVideoCompositionLayerInstruction.FromAssetTrack(videoTrack);
            layerInstruction.Init();
            layerInstruction.SetTransform(videoTrack.PreferredTransform, CMTime.Zero);
            mainInstruction.LayerInstructions = new AVVideoCompositionLayerInstruction[] { layerInstruction };

            var textLayer = new CATextLayer();
            textLayer.SetFont("Arial");
            textLayer.FontSize = fontSize;
            textLayer.ForegroundColor = fontColor.CGColor;
            textLayer.String = text;
            textLayer.Frame = new CGRect(
                textRect.GetMinX(),
                videoTrack.NaturalSize.Height - textRect.GetMaxY(),
                textRect.Width, textRect.Height);

            var overlayer = new CALayer();
            overlayer.Frame = new CGRect(CGPoint.Empty, videoTrack.NaturalSize);
            overlayer.MasksToBounds = true;
            overlayer.AddSublayer(textLayer);

            var parentLayer = new CALayer();
            var videoLayer = new CALayer();
            parentLayer.Frame = new CGRect(CGPoint.Empty, videoTrack.NaturalSize);
            videoLayer.Frame = new CGRect(CGPoint.Empty, videoTrack.NaturalSize);
            parentLayer.AddSublayer(videoLayer);
            parentLayer.AddSublayer(overlayer);

            videoComposition.RenderSize = videoTrack.NaturalSize;
            videoComposition.FrameDuration = new CMTime(1, 30); //videoTrack.MinFrameDuration;
            videoComposition.Instructions = new AVMutableVideoCompositionInstruction[] { mainInstruction };
            videoComposition.AnimationTool = AVVideoCompositionCoreAnimationTool.FromLayer(parentLayer, videoTrack.TrackID);

            return new Tuple<AVMutableComposition, AVMutableVideoComposition>(imageComposition, videoComposition);
        }

        private AVAssetExportSession AssetExportSessionWithPreset(
            string presetName,
            AVMutableComposition composition,
            AVMutableVideoComposition videoComposition)
        {
            return new AVAssetExportSession(composition, presetName)
            {
                VideoComposition = videoComposition
            };
        }
    }
}
