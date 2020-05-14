using PhotoGallery.Common;
using PhotoGallery.Helpers;
using RestSharp.Extensions;
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
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Windows.Storage.Streams;
using System.Threading.Tasks;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PhotoGallery
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //List<MovieData> lst;
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "EGyO3sEGNsULCzKvXiQKIHFlyJAj5jUxO57jPXoU",
            BasePath = "https://photogallery-81868.firebaseio.com/"
        };
        IFirebaseClient client;
        private ObservableCollection<MovieData> lst;
        public MainPage()
        {

            lst = new ObservableCollection<MovieData>();
            //lst.Add(new MovieData { Title = "Movie 1", ImageData = LoadImage("StoreLogo.png") });

            InitializeComponent();
            // lst = Database.GetAllImages();

            client = new FireSharp.FirebaseClient(config);
            if (client != null)
                Debug.WriteLine("Connection established");
            else
                Debug.WriteLine("Connection not established");

           // LoadImages();

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
                //Image img1 = new BitmapImage(file.Name);
                StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                Debug.WriteLine(localFolder.Path + "10000000");
                StorageFile copiedFile = await file.CopyAsync(localFolder, file.Name, NameCollisionOption.ReplaceExisting);//copy to localstate
                //lst.Add(new MovieData { Title = copiedFile.Name, ImageData = LoadImage("ms-appdata:///local/" + copiedFile.Name)});
                //BitmapImage img =  LoadImage(copiedFile.Path);
                Windows.Storage.Streams.IRandomAccessStream random = await Windows.Storage.Streams.RandomAccessStreamReference.CreateFromUri(new Uri("ms-appdata:///local/" + copiedFile.Name)).OpenReadAsync();//ms-appdata:///local/
                Windows.Graphics.Imaging.BitmapDecoder decoder = await Windows.Graphics.Imaging.BitmapDecoder.CreateAsync(random);

                Windows.Graphics.Imaging.PixelDataProvider pixelData = await decoder.GetPixelDataAsync();
                byte[] bytes = pixelData.DetachPixelData();
                string output = Convert.ToBase64String(bytes);
                DateTime dd = DateTime.Now;
                var data = new Image_Model
                {

                    Img = output,
                    Title = file.Name,
                    DateAdded = dd.ToString("dd/MM/yyyy")
                };
                SetResponse request = await client.SetTaskAsync("Image/", data);
                Image_Model result = request.ResultAs<Image_Model>();
                Debug.WriteLine("Success");
               // LoadImages();
                //var x = Database.InsertImage(copiedFile.Name, img);
                //lst = Database.GetAllImages();
                //Debug.WriteLine(x);

            }

            else
            {

            }
            //lst.Add(new MovieData { Title = "Movie 100", ImageData = LoadImage("StoreLogo.png") });
            //LoadImages();
        }
        public async void LoadImages()
        {
            FirebaseResponse response = await client.GetTaskAsync("Image/");
            //ObservableCollection<Image_Model> imgs = response.ResultAs<ObservableCollection<Image_Model>>();
            Image_Model img = response.ResultAs<Image_Model>();
            //foreach (Image_Model img in imgs)
            //{
            byte[] bytes = Convert.FromBase64String(img.Img);
            Debug.WriteLine(bytes.Length);
            BitmapImage image = new BitmapImage();
            using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
            {
                using (DataWriter writer = new DataWriter(stream.GetOutputStreamAt(0)))
                {
                    writer.WriteBytes(bytes);
                    await writer.StoreAsync();
                }

                await image.SetSourceAsync(stream);
            }

            lst.Add(new MovieData { Title = img.Title, ImageData = image });
            //}

        }

    }
}
