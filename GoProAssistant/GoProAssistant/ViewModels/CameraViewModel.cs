using System;
using System.Threading.Tasks;
using System.Windows.Input;

using GoProAssistant.CameraInterface;
using GoProAssistant.Shared;

using Xamarin.Forms;

namespace GoProAssistant.ViewModels
{
    public class CameraViewModel : BaseViewModel
    {
        private Camera camera => DependencyService.Get<Camera>();
        private ILocationProvider locationProvider => DependencyService.Get<ILocationProvider>();
        private IRecordingStorage recordingStorage => DependencyService.Get<IRecordingStorage>();

        private bool canPerformOperation = true;
        private bool locateGoPro = false;

        public CameraViewModel()
        {
            Title = "Camera";

            locationProvider.StartLocationUpdates();
            locationProvider.LocationUpdated += HandleLocationChanged;

            ToggleRecordCommand = new Command(async () => {
                if (IsRecording) // Is recording
                {
                    CanPerformOperation = false;
                    RecordButtonText = "Wait";
                    RecordNameText = string.Empty;

                    if (!await StopRecordingAsync())
                    {
                        await Shell.Current.DisplayAlert("Alert", "Stopping failed", "OK");
                        RecordButtonText = "Stop";
                    }
                    else
                        RecordButtonText = "Record";

                    CanPerformOperation = true;
                }
                else // Is not recording
                {
                    CanPerformOperation = false;
                    RecordButtonText = "Wait";

                    if (!await StartRecordingAsync())
                    {
                        IsRecording = false;

                        await Shell.Current.DisplayAlert("Alert", "Recording failed", "OK");
                        RecordButtonText = "Record";
                    }
                    else
                        RecordButtonText = "Stop";

                    CanPerformOperation = true;
                }
            });

            ToggleLocateCommand = new Command(async () => {
                if (locateGoPro) // Is locating
                {
                    CanPerformOperation = false;
                    LocateButtonText = "Wait";
                    locateGoPro = false;

                    if (!await camera.LocateOffAsync())
                    {
                        await Shell.Current.DisplayAlert("Alert", "Locate failed", "OK");
                        LocateButtonText = "Locate Off";
                    }    
                    else
                        LocateButtonText = "Locate On";

                    CanPerformOperation = true;
                }
                else // Is not locating
                {
                    CanPerformOperation = false;
                    LocateButtonText = "Wait";
                    locateGoPro = true;

                    if (!await camera.LocateOnAsync())
                    {
                        await Shell.Current.DisplayAlert("Alert", "Locate failed", "OK");
                        LocateButtonText = "Locate On";
                    }
                    else
                        LocateButtonText = "Locate Off";

                    CanPerformOperation = true;
                }
            });

            PowerOffCommand = new Command(async () => {
                CanPerformOperation = false;
                
                await camera.PowerOffAsync();

                CanPerformOperation = true;
            });
        }

        public ICommand ToggleRecordCommand { get; }
        public ICommand ToggleLocateCommand { get; }
        public ICommand PowerOffCommand { get; }

        public bool CanPerformOperation
        {
            get => canPerformOperation;
            set => SetProperty(ref canPerformOperation, value);
        }

        public bool IsRecording
        {
            get => GoProAssistant.IsInRecordingMode;
            set => GoProAssistant.IsInRecordingMode = value;
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
            if (IsRecording)
                recordingStorage.StoreLocation(e.Location);

            Altitude = string.Format("{0:0.#} meters", e.Location.Altitude);
            Longitude = e.Location.Longitude.ToString();
            Latitude = e.Location.Latitude.ToString();
            Course = e.Location.Course.ToString();
            Speed = string.Format("{0:0.#} meters per second", e.Location.Speed);
        }

        private async Task<bool> StartRecordingAsync()
        {
            if (string.IsNullOrWhiteSpace(RecordNameText) || !recordingStorage.StartRecording(RecordNameText.Trim()))
                return false;

            if (!await camera.StartRecordingAsync())
                return false;

            IsRecording = true;

            return true;
        }

        private async Task<bool> StopRecordingAsync()
        {
            if (!await camera.StopRecordingAsync())
                return false;

            IsRecording = false;
            var recording = recordingStorage.FinishRecording();

            return true;
        }
    }
}