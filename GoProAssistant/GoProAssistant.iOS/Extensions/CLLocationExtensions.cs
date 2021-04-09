using CoreLocation;
using GoProAssistant.Shared;

namespace GoProAssistant.iOS.Extensions
{
    public static class CLLocationExtensions
    {
        public static Location ConvertToLocation(this CLLocation loc)
        {
            return new Location
            {
                Altitude = loc.Altitude,
                Longitude = loc.Coordinate.Longitude,
                Latitude = loc.Coordinate.Latitude,
                Course = loc.Course,
                Speed = loc.Speed,
                SpeedAccuracy = loc.SpeedAccuracy
            };
        }
    }
}
