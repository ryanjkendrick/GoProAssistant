namespace GoProAssistant.Shared.Geospatial
{
    public class Location
    {
        public double Altitude { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public double Course { get; set; }
        public double Speed { get; set; }

        public double SpeedAccuracy { get; set; }

        public Location()
        {
        }
    }
}
