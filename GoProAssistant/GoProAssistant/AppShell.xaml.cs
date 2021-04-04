using System;
using System.Collections.Generic;
using GoProAssistant.ViewModels;
using GoProAssistant.Views;
using Xamarin.Forms;

namespace GoProAssistant
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

    }
}
