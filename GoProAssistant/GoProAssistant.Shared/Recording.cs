using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace GoProAssistant.Shared
{
    public class Recording
    {
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool VideoSaved { get; set; }
        public bool EditedVideoSaved { get; set; }

        public List<LocationSample> LocationSamples { get; set; }

        [JsonIgnore]
        public bool HasFinished => EndTime != default;
        public TimeSpan Length => HasFinished ? EndTime - StartTime : default;

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
