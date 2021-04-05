using System;

using GoProAssistant.Shared;
using GoProAssistant.VideoEdit;

namespace GoProAssistant.Extensions
{
    public static class RecordingExtensions
    {
        public static TextOverlay[] ConvertToTextOverlayArray(this Recording recording)
        {
            TextOverlay[] textOverlays = new TextOverlay[recording.LocationSamples.Count];
            DateTime lastSampleTime = recording.StartTime;
            TimeSpan duration = default;

            for (int i = 0; i < recording.LocationSamples.Count; i++)
            {
                var sample = recording.LocationSamples[i];

                textOverlays[i] = new TextOverlay();
                textOverlays[i].Text = string.Format("{0:0.#} m/s", sample.Location.Speed);
                textOverlays[i].StartTime = duration;

                if (i < recording.LocationSamples.Count - 1) // Is not the last sample
                {
                    duration += sample.SampledAt - lastSampleTime;
                    lastSampleTime = sample.SampledAt;

                    textOverlays[i].StopTime = duration;
                }
            }

            return textOverlays;
        }
    }
}
