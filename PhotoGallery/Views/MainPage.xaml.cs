using PhotoGallery.Common;
using PhotoGallery.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PhotoGallery
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //List<MovieData> lst;
        private ObservableCollection<MovieData> lst;
        public MainPage()
        {

            lst = new ObservableCollection<MovieData>();
            //lst.Add(new MovieData { Title = "Movie 1", ImageData = LoadImage("StoreLogo.png") });
            
            InitializeComponent();
            lst = Database.GetAllImages();




        }
        
        private BitmapImage LoadImage(string filename)
        {
            return new BitmapImage(new Uri(filename));
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");
            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                Debug.WriteLine(file.Name);
                StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                Debug.WriteLine(localFolder.Path + "10000000");
                StorageFile copiedFile = await file.CopyAsync(localFolder, file.Name, NameCollisionOption.ReplaceExisting);
                //lst.Add(new MovieData { Title = copiedFile.Name, ImageData = LoadImage(copiedFile.Path});
                BitmapImage img =  LoadImage(copiedFile.Path);
          
                var x = Database.InsertImage(copiedFile.Name, img);
                lst = Database.GetAllImages();
                Debug.WriteLine(x);

            }

            else
            {

            }
            //lst.Add(new MovieData { Title = "Movie 100", ImageData = LoadImage("StoreLogo.png") });
            //LoadImages();

        }
        public void LoadImages()
        { 
            //Debug.WriteLine(lst.Count());
            foreach(var item in lst)
            {
                Debug.WriteLine(item.Title+" "+item.ImageData);
            }

            //lstImage.ItemsSource = lst;
        }
    }
}
