using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;

using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.CommunityToolkit.Core;

using GoProAssistant.Shared;
using GoProAssistant.Shared.Extensions;

using Newtonsoft.Json;

namespace GoProAssistant.ViewModels
{
    [QueryProperty(nameof(RecordingName), nameof(RecordingName))]
    public class ItemDetailViewModel : BaseViewModel
    {
        private IVideoEditor vidEdit => DependencyService.Get<IVideoEditor>();

        private bool canPerformOperation = true;
        private bool showOriginalVid, showEditedVid;
        private string name;
        private string recordingName;
        private string startTime;
        private string length;
        private FileMediaSource originalVideoSource, editedVideoSource;

        public bool CanPerformOperation
        {
            get => canPerformOperation;
            set => SetProperty(ref canPerformOperation, value);
        }

        public bool ShowOriginalVid
        {
            get => showOriginalVid;
            set => SetProperty(ref showOriginalVid, value);
        }

        public bool ShowEditedVid
        {
            get => showEditedVid;
            set => SetProperty(ref showEditedVid, value);
        }

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public string StartTime
        {
            get => startTime;
            set => SetProperty(ref startTime, value);
        }

        public string Length
        {
            get => length;
            set => SetProperty(ref length, value);
        }

        public FileMediaSource OriginalVideoSource
        {
            get => originalVideoSource;
            set => SetProperty(ref originalVideoSource, value);
        }

        public FileMediaSource EditedVideoSource
        {
            get => editedVideoSource;
            set => SetProperty(ref editedVideoSource, value);
        }

        public string RecordingName
        {
            get
            {
                return recordingName;
            }
            set
            {
                recordingName = value;
                LoadRecording(value);
            }
        }

        public ICommand AddVidCommand { get; }
        public ICommand ProcessVidCommand { get; }
        public ICommand DeleteCommand { get; }

        public ItemDetailViewModel()
        {
            AddVidCommand = new Command(async () => {
                CanPerformOperation = false;

                var file = await MediaPicker.PickVideoAsync();

                if (file != null)
                {
                    using (var vidStream = await file.OpenReadAsync())
                    {
                        await DataStore.StoreVideoAsync(Name, vidStream);
                    }
                }

                CanPerformOperation = true;
            });

            ProcessVidCommand = new Command(async () => {
                CanPerformOperation = false;

                var rec = DataStore.GetRecording(Name);
                var textOverlays = rec.ConvertToTextOverlayArray();
                string json = JsonConvert.SerializeObject(textOverlays);

                string fn = "TextOverlays.json";
                string file = Path.Combine(FileSystem.CacheDirectory, fn);
                File.WriteAllText(file, json);

                await Share.RequestAsync(new ShareFileRequest
                {
                    Title = Title,
                    File = new ShareFile(file)
                });

                if (rec.VideoSaved)
                {
                    string inputFile = DataStore.GetVideoPath(Name);
                    string outputfile = DataStore.GetEditedVideoPath(Name);

                    vidEdit.AddTextToVideo(inputFile, outputfile, textOverlays);

                    EditedVideoSource = new FileMediaSource
                    {
                        File = outputfile
                    };
                    ShowEditedVid = true;

                    DataStore.StoreEditedVideo(Name);
                }

                CanPerformOperation = true;
            });

            DeleteCommand = new Command(async () => {
                CanPerformOperation = false;

                DataStore.DeleteRecording(Name);
                await Shell.Current.Navigation.PopAsync();

                CanPerformOperation = true;
            });
        }

        public void LoadRecording(string name)
        {
            try
            {
                var rec = DataStore.GetRecording(name);

                Name = rec.Name;
                StartTime = rec.StartTime.ToString("dd/MM/yyyy HH:mm");
                Length = rec.Length.ToString(@"hh\:mm\:ss");

                if (rec.VideoSaved)
                {
                    string vidPath = DataStore.GetVideoPath(name);

                    OriginalVideoSource = new FileMediaSource
                    {
                        File = vidPath
                    };
                    ShowOriginalVid = true;
                }

                if (rec.EditedVideoSaved)
                {
                    string vidPath = DataStore.GetEditedVideoPath(name);

                    EditedVideoSource = new FileMediaSource
                    {
                        File = vidPath
                    };
                    ShowEditedVid = true;
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Recording");
            }
        }
    }
}
