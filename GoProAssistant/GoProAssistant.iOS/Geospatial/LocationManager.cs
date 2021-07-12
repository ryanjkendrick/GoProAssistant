using System;
using CoreLocation;
using GoProAssistant.iOS.Extensions;
using GoProAssistant.Shared.Geospatial;
using UIKit;

namespace GoProAssistant.iOS.Geospatial
{
    public class LocationManager : ILocationProvider
    {
        protected CLLocationManager locMgr;

        public event EventHandler<LocationUpdatedEventArgs> LocationUpdated = delegate { };

        public LocationManager()
        {
            this.locMgr = new CLLocationManager();
            this.locMgr.PausesLocationUpdatesAutomatically = false;

            // iOS 8 has additional permissions requirements
            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                locMgr.RequestAlwaysAuthorization(); // Works in background
                locMgr.RequestWhenInUseAuthorization(); // Only in foreground
            }

            if (UIDevice.CurrentDevice.CheckSystemVersion(9, 0))
            {
                locMgr.AllowsBackgroundLocationUpdates = true;
            }
        }


        public CLLocationManager LocMgr
        {
            get { return this.locMgr; }
        }

        public void StartLocationUpdates()
        {
            if (CLLocationManager.LocationServicesEnabled)
            {
                // Set the desired accuracy, in meters
                LocMgr.DesiredAccuracy = 1;
                LocMgr.LocationsUpdated += (object sender, CLLocationsUpdatedEventArgs e) =>
                {
                    var location = e.Locations[e.Locations.Length - 1];

                    // Fire our custom Location Updated event
                    LocationUpdated(this, new LocationUpdatedEventArgs(location?.ConvertToLocation()));
                };
                LocMgr.StartUpdatingLocation();
            }
        }

        public void StopLocationUpdates()
        {
            if (CLLocationManager.LocationServicesEnabled)
            {
                LocMgr.StopUpdatingLocation();
            }
        }

        public Location GetLocation()
        {
            var loc = LocMgr.Location;

            return loc?.ConvertToLocation();
        }
    }
}
