using System;
using System.Collections.Generic;

namespace GoProAssistant.Shared
{
    public class Recording
    {
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public List<LocationSample> LocationSamples { get; set; }

        public bool HasFinished => EndTime != default;
        public TimeSpan Length => EndTime - StartTime;

        public Recording()
        {
            LocationSamples = new List<LocationSample>();
        }

        public Recording(string name)
        {
            Name = name;

            LocationSamples = new List<LocationSample>();
        }
    }
}
