using System.Threading.Tasks;
using VisioForge.Controls.VideoEdit;
using VisioForge.Types;
using VisioForge.Types.OutputFormat;
using VisioForge.Types.VideoEffects;

namespace GoProAssistant.VideoEdit
{
    public class VidEdit
    {
        public async Task AddTextToVideoAsync(string inputFile, string outputFile, TextOverlay[] textOverlays)
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

            await core.StartAsync();
        }
    }
}
