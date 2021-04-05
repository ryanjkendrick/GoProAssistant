using Xamarin.Forms;

using GoProAssistant.Services;
using GoProAssistant.iOS;
using GoProAssistant.CameraInterface;
using GoProAssistant.Shared;

namespace GoProAssistant
{
    public partial class App : Xamarin.Forms.Application
    {
        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            DependencyService.Register<Camera>();
            DependencyService.Register<ILocationProvider, LocationManager>();
            DependencyService.Register<IRecordingStorage, RecordingStorage>();

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
