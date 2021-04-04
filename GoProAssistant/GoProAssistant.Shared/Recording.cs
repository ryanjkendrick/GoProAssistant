using System;
using System.Collections.Generic;

namespace GoProAssistant.Shared
{
    public class Recording
    {
        public string Name { get; set; }
        public TimeSpan Length { get; set; }

        public List<Location> Locations { get; set; }

        public Recording()
        {
            Locations = new List<Location>();
        }

        public Recording(string name)
        {
            Name = name;

            Locations = new List<Location>();
        }
    }
}
