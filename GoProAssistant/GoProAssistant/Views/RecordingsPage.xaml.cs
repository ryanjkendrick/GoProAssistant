using Xamarin.Forms;

using GoProAssistant.ViewModels;

namespace GoProAssistant.Views
{
    public partial class RecordingsPage : ContentPage
    {
        RecordingsViewModel _viewModel;

        public RecordingsPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new RecordingsViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}