using System;
namespace GoProAssistant.Shared
{
    public class LocationStorage : ILocationStorage
    {
        private Recording currentRecording;

        public LocationStorage()
        {
        }

        public Recording FinishRecording(TimeSpan length)
        {
            currentRecording.Length = length;

            return currentRecording;
        }

        public void Init()
        {
            throw new NotImplementedException();
        }

        public bool StartRecording(string name)
        {
            currentRecording = new Recording(name);

            return true;
        }

        public void StoreLocation(Location newLocation)
        {
            if (newLocation != null)
                currentRecording.Locations.Add(newLocation);
        }
    }
}
