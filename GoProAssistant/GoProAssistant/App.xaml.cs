using Xamarin.Forms;

using GoProAssistant.iOS.Geospatial;
using GoProAssistant.iOS.VideoEditing;

using GoProAssistant.CameraInterface;

using GoProAssistant.Shared.Geospatial;
using GoProAssistant.Shared.VideoRecording;
using GoProAssistant.Shared.VideoEditing;

namespace GoProAssistant
{
    public partial class App : Xamarin.Forms.Application
    {
        private ILocationProvider locationProvider => DependencyService.Get<ILocationProvider>();

        public App()
        {
            InitializeComponent();

            DependencyService.Register<Camera>();
            DependencyService.Register<ILocationProvider, LocationManager>();
            DependencyService.Register<IRecordingStorage, RecordingStorage>();

            MainPage = new AppShell();

            DependencyService.Register<IVideoEditor, VideoEditor>();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
            if (!GoProAssistant.IsInRecordingMode)
                locationProvider.StopLocationUpdates();
        }

        protected override void OnResume()
        {
            if (!GoProAssistant.IsInRecordingMode)
                locationProvider.StartLocationUpdates();
        }
    }
}
