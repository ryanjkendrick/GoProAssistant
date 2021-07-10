using System.ComponentModel;
using Xamarin.Forms;
using GoProAssistant.ViewModels;

namespace GoProAssistant.Views
{
    public partial class RecordingDetailPage : ContentPage
    {
        public RecordingDetailPage()
        {
            InitializeComponent();
            BindingContext = new RecordingDetailViewModel();
        }
    }
}