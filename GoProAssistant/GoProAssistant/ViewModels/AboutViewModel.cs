using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;

using GoProAssistant.CameraInterface;
using GoProAssistant.Shared;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GoProAssistant.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        private Camera camera => DependencyService.Get<Camera>();
        private ILocationProvider locationProvider => DependencyService.Get<ILocationProvider>();
        private ILocationStorage locationStorage => DependencyService.Get<ILocationStorage>();

        private bool recordGps = false;
        private bool locateGoPro = false;

        public AboutViewModel()
        {
            Title = "Camera";

            locationProvider.StartLocationUpdates();
            locationProvider.LocationUpdated += HandleLocationChanged;

            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://aka.ms/xamarin-quickstart"));
            ToggleRecordCommand = new Command(async () => {
                if (recordGps) // Is recording
                {
                    RecordButtonText = "Record";

                    if (!await StopRecordingAsync())
                        RecordButtonText = "Error";
                }
                else // Is not recording
                {
                    RecordButtonText = "Stop";

                    if (!await StartRecordingAsync())
                        RecordButtonText = "Error";
                }
            });
            ToggleLocateCommand = new Command(async () => {
                if (locateGoPro) // Is locating
                {
                    LocateButtonText = "Locate On";
                    locateGoPro = false;

                    if (!await camera.LocateOffAsync())
                        LocateButtonText = "Error";
                }
                else // Is not locating
                {
                    LocateButtonText = "Locate Off";
                    locateGoPro = true;

                    if (!await camera.LocateOnAsync())
                        LocateButtonText = "Error";
                }
            });
            PowerOffCommand = new Command(async () => {
                await camera.PowerOffAsync();

                string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"{RecordNameText}.json");
                string data = File.ReadAllText(fileName);

                var obj = JsonConvert.DeserializeObject<Recording>(data);
                Console.WriteLine(obj.Name);
            });
        }

        public ICommand OpenWebCommand { get; }
        public ICommand ToggleRecordCommand { get; }
        public ICommand ToggleLocateCommand { get; }
        public ICommand PowerOffCommand { get; }

        public bool Recording
        {
            get => recordGps;
            set => SetProperty(ref recordGps, value);
        }

        public string RecordNameText
        {
            get => recordNameText;
            set => SetProperty(ref recordNameText, value);
        }

        public string RecordButtonText
        {
            get => recordButtonText;
            set => SetProperty(ref recordButtonText, value);
        }

        public string LocateButtonText
        {
            get => locateButtonText;
            set => SetProperty(ref locateButtonText, value);
        }

        public string Altitude
        {
            get => altitude;
            set => SetProperty(ref altitude, value);
        }

        public string Longitude
        {
            get => longitude;
            set => SetProperty(ref longitude, value);
        }

        public string Latitude
        {
            get => latitude;
            set => SetProperty(ref latitude, value);
        }

        public string Course
        {
            get => course;
            set => SetProperty(ref course, value);
        }

        public string Speed
        {
            get => speed;
            set => SetProperty(ref speed, value);
        }

        private string recordNameText;
        private string recordButtonText = "Record";
        private string locateButtonText = "Locate On";

        private string altitude;
        private string longitude;
        private string latitude;
        private string course;
        private string speed;

        public void HandleLocationChanged(object sender, LocationUpdatedEventArgs e)
        {
            Altitude = string.Format("{0:0.#} meters", e.Location.Altitude);
            Longitude = e.Location.Longitude.ToString();
            Latitude = e.Location.Latitude.ToString();
            Course = e.Location.Course.ToString();
            Speed = e.Location.Speed.ToString();

            if (recordGps)
                locationStorage.StoreLocation(e.Location);
        }

        private async Task<bool> StartRecordingAsync()
        {
            locationStorage.StartRecording(RecordNameText);

            if (!await camera.StartRecordingAsync())
                return false;

            Recording = true;

            return true;
        }

        private async Task<bool> StopRecordingAsync()
        {
            if (!await camera.StopRecordingAsync())
                return false;

            Recording = false;
            var recording = locationStorage.FinishRecording();

            Console.WriteLine($"{recording.Name} {recording.Length} {recording.LocationSamples.Count}");

            string json = JsonConvert.SerializeObject(recording);
            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"{recording.Name}.json");
            File.WriteAllText(fileName, json);

            return true;
        }
    }
}