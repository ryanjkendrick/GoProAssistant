using System;

namespace GoProAssistant.Shared.Geospatial
{
    public interface ILocationProvider
    {
        public void StartLocationUpdates();
        public void StopLocationUpdates();

        public Location GetLocation();

        public event EventHandler<LocationUpdatedEventArgs> LocationUpdated;
    }
}
