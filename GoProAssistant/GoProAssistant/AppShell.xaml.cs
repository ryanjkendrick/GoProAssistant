using GoProAssistant.Views;

using Xamarin.Forms;

namespace GoProAssistant
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(RecordingDetailPage), typeof(RecordingDetailPage));
        }

    }
}
