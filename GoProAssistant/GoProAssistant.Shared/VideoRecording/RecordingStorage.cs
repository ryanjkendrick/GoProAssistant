using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using GoProAssistant.Shared.Geospatial;

using Newtonsoft.Json;

namespace GoProAssistant.Shared.VideoRecording
{
    public class RecordingStorage : IRecordingStorage
    {
        private Recording _currentRecording;

        private readonly string _recordingFileDirectory;

        public RecordingStorage()
        {
            _recordingFileDirectory = Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData, Environment.SpecialFolderOption.Create) + "/RecordingData";
        }

        public Recording FinishRecording()
        {
            _currentRecording.EndTime = DateTime.Now;
            SaveRecording();

            return _currentRecording;
        }

        public bool StartRecording(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || File.Exists(GenerateRecordingFilename(name, "json")))
                return false;

            _currentRecording = new Recording(name);
            _currentRecording.StartTime = DateTime.Now;

            return true;
        }

        public void StoreLocation(Location newLocation)
        {
            var sample = new LocationSample(newLocation);

            if (newLocation != null && !_currentRecording.HasFinished)
                _currentRecording.LocationSamples.Add(sample);
        }

        public Recording GetRecording(string name)
        {
            if (!Directory.Exists(_recordingFileDirectory))
                return null;

            string fileName = GenerateRecordingFilename(name, "json");

            return GetRecordingFromFile(fileName);
        }

        public Recording[] GetAllRecordings()
        {
            if (!Directory.Exists(_recordingFileDirectory))
                return new Recording[0];

            List<Recording> recordings = new List<Recording>();

            foreach (string filename in Directory.EnumerateFiles(_recordingFileDirectory, "*.json"))
            {
                var rec = GetRecordingFromFile(filename);

                if (rec != null)
                    recordings.Add(rec);
            }

            return recordings.OrderBy(x => x.StartTime).ToArray();
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

        private Recording GetRecordingFromFile(string filename)
        {
            if (!File.Exists(filename))
                return null;

            string json = File.ReadAllText(filename);
            var rec = JsonConvert.DeserializeObject<Recording>(json);

            return rec;
        }

        private void DeleteFile(string filename)
        {
            if (File.Exists(filename))
                File.Delete(filename);
        }

        private void SaveRecording(Recording recording = null)
        {
            Recording recordingToChange = recording ?? _currentRecording;

            if (!Directory.Exists(_recordingFileDirectory))
                Directory.CreateDirectory(_recordingFileDirectory);

            string json = JsonConvert.SerializeObject(recordingToChange);

            File.WriteAllText(GenerateRecordingFilename(recordingToChange.Name, "json"), json);
        }

        private string GenerateRecordingFilename(string recName, string extension) => Path.Combine(_recordingFileDirectory, $"{recName}.{extension}");
    }
}
