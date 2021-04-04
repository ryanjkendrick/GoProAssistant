using System;

namespace GoProAssistant.Shared
{
    public interface ILocationProvider
    {
        public void StartLocationUpdates();
        public void StopLocationUpdates();

        public Location GetLocation();

        public event EventHandler<LocationUpdatedEventArgs> LocationUpdated;
    }
}
