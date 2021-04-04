using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using GoProAssistant.Services;
using GoProAssistant.Views;
using GoProAssistant.iOS;
using System.Collections.Generic;
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
            DependencyService.Register<ILocationStorage, LocationStorage>();

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
