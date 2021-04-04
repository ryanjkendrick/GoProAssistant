using System.ComponentModel;
using Xamarin.Forms;
using GoProAssistant.ViewModels;

namespace GoProAssistant.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}