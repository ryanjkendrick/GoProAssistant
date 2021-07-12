using GoProAssistant.Shared.VideoRecording;

namespace GoProAssistant.Shared.VideoEditing
{
    public interface IVideoEditor
    {
        public void AddTextToVideo(string inputFile, string outputFile, TextOverlay[] textOverlays);

        //public event EventHandler<LocationUpdatedEventArgs> LocationUpdated;
    }
}
