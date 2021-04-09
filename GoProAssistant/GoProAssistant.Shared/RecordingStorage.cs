using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace GoProAssistant.Shared
{
    public class RecordingStorage : IRecordingStorage
    {
        private Recording currentRecording;

        private readonly string recordingFileDirectory;

        public RecordingStorage()
        {
            recordingFileDirectory = Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData, Environment.SpecialFolderOption.Create) + "/RecordingData";
        }

        public Recording FinishRecording()
        {
            currentRecording.EndTime = DateTime.Now;
            SaveRecording();

            return currentRecording;
        }

        public bool StartRecording(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || File.Exists(GenerateRecordingFilename(name, "json")))
                return false;

            currentRecording = new Recording(name);
            currentRecording.StartTime = DateTime.Now;

            return true;
        }

        public void StoreLocation(Location newLocation)
        {
            var sample = new LocationSample(newLocation);

            if (newLocation != null && !currentRecording.HasFinished)
                currentRecording.LocationSamples.Add(sample);
        }

        private void SaveRecording(Recording recording = null)
        {
            Recording recordingToChange = recording ?? currentRecording;

            if (!Directory.Exists(recordingFileDirectory))
                Directory.CreateDirectory(recordingFileDirectory);

            string json = JsonConvert.SerializeObject(recordingToChange);

            File.WriteAllText(GenerateRecordingFilename(recordingToChange.Name, "json"), json);
        }

        private string GenerateRecordingFilename(string recName, string extension) => Path.Combine(recordingFileDirectory, $"{recName}.{extension}");

        public Recording GetRecording(string name)
        {
            if (!Directory.Exists(recordingFileDirectory))
                return null;

            string fileName = GenerateRecordingFilename(name, "json");

            return GetRecordingFromFile(fileName);
        }

        public Recording[] GetAllRecordings()
        {
            if (!Directory.Exists(recordingFileDirectory))
                return new Recording[0];

            List<Recording> recordings = new List<Recording>();

            foreach (string filename in Directory.EnumerateFiles(recordingFileDirectory, "*.json"))
            {
                var rec = GetRecordingFromFile(filename);

                if (rec != null)
                    recordings.Add(rec);
            }

            return recordings.OrderBy(x => x.StartTime).ToArray();
        }

        private Recording GetRecordingFromFile(string filename)
        {
            if (!File.Exists(filename))
                return null;

            string json = File.ReadAllText(filename);
            var rec = JsonConvert.DeserializeObject<Recording>(json);

            return rec;
        }

        public void DeleteRecording(string name)
        {
            string filename = GenerateRecordingFilename(name, "json");
            DeleteFile(filename);

            filename = GetVideoPath(name);
            DeleteFile(filename);

            filename = GetEditedVideoPath(name);
            DeleteFile(filename);
        }

        public async Task<bool> StoreVideoAsync(string name, Stream video)
        {
            string filename = GenerateRecordingFilename(name, "mp4");
            Recording recordingToChange = GetRecording(name);

            using (var fileStream = File.Create(filename))
            {
                video.Seek(0, SeekOrigin.Begin);
                await video.CopyToAsync(fileStream);
            }

            recordingToChange.VideoSaved = true;
            SaveRecording(recordingToChange);

            return true;
        }

        public string GetVideoPath(string name)
        {
            string filename = GenerateRecordingFilename(name, "mp4");

            return filename;
        }

        public string GetEditedVideoPath(string name)
        {
            string filename = GenerateRecordingFilename(name + "-Edited", "mp4");

            return filename;
        }

        public void StoreEditedVideo(string name)
        {
            Recording recordingToChange = GetRecording(name);

            recordingToChange.EditedVideoSaved = true;

            SaveRecording(recordingToChange);
        }

        private void DeleteFile(string filename)
        {
            if (File.Exists(filename))
                File.Delete(filename);
        }
    }
}
