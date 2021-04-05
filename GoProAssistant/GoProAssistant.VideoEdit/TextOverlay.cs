using System;
namespace GoProAssistant.VideoEdit
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
