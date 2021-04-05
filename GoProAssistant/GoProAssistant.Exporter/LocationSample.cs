using System;
namespace GoProAssistant.Exporter
{
    public class LocationSample
    {
        public DateTime SampledAt { get; set; }
        public Location Location { get; set; }

        public LocationSample()
        {
            SampledAt = DateTime.Now;
        }

        public LocationSample(Location location)
        {
            SampledAt = DateTime.Now;
            Location = location;
        }
    }
}
