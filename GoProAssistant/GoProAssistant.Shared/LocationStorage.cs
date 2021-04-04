using System;

namespace GoProAssistant.Shared
{
    public class LocationStorage : ILocationStorage
    {
        private Recording currentRecording;

        public LocationStorage()
        {
        }

        public Recording FinishRecording()
        {
            currentRecording.EndTime = DateTime.Now;

            return currentRecording;
        }

        public void Init()
        {
            throw new NotImplementedException();
        }

        public bool StartRecording(string name)
        {
            currentRecording = new Recording(name);
            currentRecording.StartTime = DateTime.Now;

            return true;
        }

        public void StoreLocation(Location newLocation)
        {
            if (newLocation != null && !currentRecording.HasFinished)
                currentRecording.LocationSamples.Add(new LocationSample(newLocation));
        }
    }
}
