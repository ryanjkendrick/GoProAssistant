using System.IO;
using System.Threading.Tasks;

using GoProAssistant.Shared.Geospatial;

namespace GoProAssistant.Shared.VideoRecording
{
    public interface IRecordingStorage
    {
        public bool StartRecording(string name);
        public void StoreLocation(Location newLocation);
        public Task<bool> StoreVideoAsync(string name, Stream video);
        public void StoreEditedVideo(string name);
        public Recording FinishRecording();

        public Recording GetRecording(string name);
        public Recording[] GetAllRecordings();
        public void DeleteRecording(string name);
        public string GetVideoPath(string name);
        public string GetEditedVideoPath(string name);
    }
}
