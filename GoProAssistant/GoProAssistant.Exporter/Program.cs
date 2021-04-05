using System;
using System.IO;
using System.Threading.Tasks;
using GoProAssistant.Exporter.Extensions;
using Newtonsoft.Json;
using VisioForge.Controls.VideoEdit;
using VisioForge.Types;
using VisioForge.Types.OutputFormat;
using VisioForge.Types.VideoEffects;

namespace GoProAssistant.Exporter
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the GoPro Assistant Exporter!");

            Console.WriteLine("Enter video file path:");
            string inputFile = Console.ReadLine();
            Console.WriteLine("Enter recording file path:");
            string recFile = Console.ReadLine();

            Recording recording = JsonConvert.DeserializeObject<Recording>(File.ReadAllText(recFile));
            TextOverlay[] textOverlays = recording.ConvertToTextOverlayArray();

            Console.WriteLine("Processing video...");

            AddTextToVideo(inputFile, "./vid.mp4", textOverlays);

            Console.WriteLine("Finished");
        }

        public static void AddTextToVideo(string inputFile, string outputFile, TextOverlay[] textOverlays)
        {
            var core = new VideoEditCore();

            core.Input_AddVideoFile(inputFile);

            core.Output_Format = new VFMP4v11Output();
            core.Output_Filename = outputFile;

            core.Video_Renderer.VideoRendererInternal = VFVideoRendererInternal.None;
            core.Video_Effects_Enabled = true;

            foreach (var overlay in textOverlays)
            {
                var textOverlay = new VFVideoEffectTextLogo(true, overlay.Text, overlay.StartTime, overlay.StopTime);

                core.Video_Effects_Add(textOverlay);
            }

            core.Start();
        }
    }
}
