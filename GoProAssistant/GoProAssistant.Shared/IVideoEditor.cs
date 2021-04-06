using System;
using System.Threading.Tasks;

namespace GoProAssistant.Shared
{
    public interface IVideoEditor
    {
        public void AddTextToVideo(string inputFile, string outputFile, TextOverlay[] textOverlays);

        //public event EventHandler<LocationUpdatedEventArgs> LocationUpdated;
    }
}
