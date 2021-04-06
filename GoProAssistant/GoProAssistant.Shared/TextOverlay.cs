using System;

namespace GoProAssistant.Shared
{
    public class TextOverlay
    {
        public string Text { get; set; }
        public string Position { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan StopTime { get; set; }

        public TextOverlay()
        {
        }
    }
}
