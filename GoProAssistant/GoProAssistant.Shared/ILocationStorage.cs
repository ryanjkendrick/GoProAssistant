using System;

namespace GoProAssistant.Shared
{
    public interface ILocationStorage
    {
        public void Init();
        public bool StartRecording(string name);
        public void StoreLocation(Location newLocation);
        public Recording FinishRecording(TimeSpan length);
    }
}
