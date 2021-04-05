using System;
namespace GoProAssistant.Exporter
{
    public class TextOverlay
    {
        public string Text { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan StopTime { get; set; }

        public TextOverlay()
        {
        }
    }
}
