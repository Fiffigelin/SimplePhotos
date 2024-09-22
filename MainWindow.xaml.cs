using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.Storage.Search;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SimplePhotos
{
    public sealed partial class MainWindow : Window
    {

        public ObservableCollection<ImageFileInfo> Images { get; } = [];
        public MainWindow()
        {
            this.InitializeComponent();
            _ = GetItemsAsync();
        }

        private async Task GetItemsAsync()
        {
            StorageFolder appInstalledFolder = Package.Current.InstalledLocation;
            StorageFolder picturesFolder = await appInstalledFolder.GetFolderAsync("Assets\\Samples");

            var result = picturesFolder.CreateFileQueryWithOptions(new QueryOptions());

            IReadOnlyList<StorageFile> imageFiles = await result.GetFilesAsync();
            foreach (StorageFile file in imageFiles)
            {
                Images.Add(await LoadImageInfoAsync(file));
            }

            ImageGridView.ItemsSource = Images;
        }

        public async static Task<ImageFileInfo> LoadImageInfoAsync(StorageFile file)
        {
            var properties = await file.Properties.GetImagePropertiesAsync();
            ImageFileInfo info = new(properties,
                                     file, file.DisplayName, file.DisplayType);

            return info;
        }
    }
}
