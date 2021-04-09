﻿using System;
using System.Collections.Generic;

namespace GoProAssistant.Shared.Extensions
{
    public static class RecordingExtensions
    {
        public static TextOverlay[] ConvertToTextOverlayArray(this Recording recording)
        {
            List<TextOverlay> textOverlays = new List<TextOverlay>();
            DateTime lastSampleTime = recording.StartTime;
            TimeSpan duration = default;

            for (int i = 0; i < recording.LocationSamples.Count; i++)
            {
                var sample = recording.LocationSamples[i];

                var speedOverlay = new TextOverlay();
                speedOverlay.Text = sample.Location.SpeedAccuracy < 0 ? "N/A" :
                    string.Format("±{0:0.#}mph {1:0.#}mph",
                        MetresPerSecondToMPH(sample.Location.SpeedAccuracy),
                        MetresPerSecondToMPH(sample.Location.Speed)); 
                speedOverlay.Position = "right";
                speedOverlay.StartTime = duration;

                var altitudeOverlay = new TextOverlay();
                altitudeOverlay.Text = string.Format("{0:0.#}m", sample.Location.Altitude);
                altitudeOverlay.Position = "left";
                altitudeOverlay.StartTime = duration;

                if (i < recording.LocationSamples.Count - 1) // Is not the last sample
                {
                    duration += sample.SampledAt - lastSampleTime;
                    lastSampleTime = sample.SampledAt;

                    speedOverlay.StopTime = duration;
                    altitudeOverlay.StopTime = duration;
                }

                textOverlays.Add(speedOverlay);
                textOverlays.Add(altitudeOverlay);
            }

            return textOverlays.ToArray();
        }

        private static double MetresPerSecondToMPH(double value) => value * 2.2369; // m/s -> mph
    }
}
