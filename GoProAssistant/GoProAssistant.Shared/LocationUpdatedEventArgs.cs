using System;

namespace GoProAssistant.Shared
{
    public class LocationUpdatedEventArgs : EventArgs
    {
        Location location;

        public LocationUpdatedEventArgs(Location location)
        {
            this.location = location;
        }

        public Location Location
        {
            get { return location; }
        }
    }
}
