﻿using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

using Xamarin.Forms;

using GoProAssistant.Models;
using GoProAssistant.Views;

namespace GoProAssistant.ViewModels
{
    public class RecordingsViewModel : BaseViewModel
    {
        private RecordingMeta _selectedItem;

        public ObservableCollection<RecordingMeta> Items { get; }
        public Command LoadItemsCommand { get; }
        public Command<RecordingMeta> ItemTapped { get; }

        public RecordingsViewModel()
        {
            Title = "Browse";
            Items = new ObservableCollection<RecordingMeta>();
            LoadItemsCommand = new Command(() => ExecuteLoadItemsCommand());

            ItemTapped = new Command<RecordingMeta>(OnItemSelected);
        }

        void ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                var items = DataStore.GetAllRecordings();

                foreach (var item in items)
                {
                    Items.Add(new RecordingMeta
                    {
                        Name = item.Name,
                        StartTime = item.StartTime.ToString("dd/MM/yyyy HH:mm")
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
        }

        public RecordingMeta SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        private async void OnAddItem(object obj)
        {
            await Shell.Current.DisplayAlert("Error", "Not yet implemented", "OK");
        }

        async void OnItemSelected(RecordingMeta item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(RecordingDetailPage)}?{nameof(RecordingDetailViewModel.RecordingName)}={item.Name}");
        }
    }
}