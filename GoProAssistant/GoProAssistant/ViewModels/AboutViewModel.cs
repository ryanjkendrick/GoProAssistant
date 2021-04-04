using System;
using System.Threading.Tasks;
using System.Windows.Input;

using GoProAssistant.CameraInterface;
using GoProAssistant.Shared;

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
        private DateTime recordStart;

        public AboutViewModel()
        {
            Title = "Camera";

            locationProvider.StartLocationUpdates();
            locationProvider.LocationUpdated += HandleLocationChanged;

            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://aka.ms/xamarin-quickstart"));
            ToggleRecordCommand = new Command(async () => {
                if (recordGps) // Is recording
                {
                    if (await StopRecordingAsync())
                        RecordButtonText = "Stop";
                }
                else // Is not recording
                {
                    if (await StartRecordingAsync())
                        RecordButtonText = "Record";
                }
            });
        }

        public ICommand OpenWebCommand { get; }
        public ICommand ToggleRecordCommand { get; }

        public string RecordButtonText
        {
            get => recordButtonText;
            set => SetProperty(ref recordButtonText, value);
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


        private string recordButtonText = "Record";

        private string altitude;
        private string longitude;
        private string latitude;
        private string course;
        private string speed;

        public void HandleLocationChanged(object sender, LocationUpdatedEventArgs e)
        {
            Altitude = e.Location.Altitude + " meters";
            Longitude = e.Location.Longitude.ToString();
            Latitude = e.Location.Latitude.ToString();
            Course = e.Location.Course.ToString();
            Speed = e.Location.Speed.ToString();

            if (recordGps)
                locationStorage.StoreLocation(e.Location);
        }

        private async Task<bool> StartRecordingAsync()
        {
            locationStorage.StartRecording("Test Recording");

            if (!await camera.StartRecordingAsync())
                return false;

            recordGps = true;
            recordStart = DateTime.Now;

            return true;
        }

        private async Task<bool> StopRecordingAsync()
        {
            if (!await camera.StopRecordingAsync())
                return false;

            recordGps = false;
            var recordLength = DateTime.Now - recordStart;

            var recording = locationStorage.FinishRecording(recordLength);
            Console.WriteLine($"{recording.Name} {recording.Length} {recording.Locations.Count}");

            return true;
        }
    }
}