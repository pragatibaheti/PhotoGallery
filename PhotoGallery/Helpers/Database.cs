using Microsoft.Data.Sqlite;
using PhotoGallery.Common;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.System;
using Windows.UI.Xaml.Media.Imaging;
using static System.Net.Mime.MediaTypeNames;

namespace PhotoGallery.Helpers
{
    public class Database
    {
        public static string DB_NAME = "Personal.db";
        SQLiteConnection SqConnection;
        Database()
        {
            SqConnection = new SQLiteConnection(Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\" + DB_NAME);
            
        }
        public static bool Register(string username,string password)
        {
           try
            {
                using (var con = new SQLiteConnection(Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\" + DB_NAME))
                {
                     
                    var query = String.Format("INSERT INTO Login VALUES('{0}','{1}')", username, password);
                    var statement = con.Prepare(query);
                    statement.Step();
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }
        public static bool Login(string username, string password)
        {
            try
            {
                using (var connection = new SQLiteConnection(Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\" + DB_NAME))
                {


                    var query = String.Format("SELECT username,password FROM Login WHERE username == '{0}' AND password =='{1}'", username, password);
                    var statement = connection.Prepare(query);
                    int count = 0;
                    while (!(SQLiteResult.DONE == statement.Step()))
                    {
                        if (statement[0] != null)
                        {
                            count++;
                        }
                    }
                    if (count == 1)
                    {
                        return true;
                    }
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }
        public static ObservableCollection<MovieData> GetAllImages()
        {
            ObservableCollection<MovieData> result = new ObservableCollection<MovieData>();
            try
            {
                using (var connection = new SQLiteConnection(Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\" + DB_NAME))
                {
                    var statement = connection.Prepare("SELECT * FROM " + "Images");
                    while (!(SQLiteResult.DONE == statement.Step()))
                    {
                        if (statement[0] != null)
                        {
                            MovieData movie = new MovieData();
                            movie.id = Convert.ToInt32(statement["Id"]);
                            movie.Title = statement["Title"].ToString();
                            movie.ImageData = (Windows.UI.Xaml.Media.Imaging.BitmapImage)statement["ImageData"];

                            result.Add(movie);
                        }
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }
        private async static Task<BitmapImage> ConvertByteToImage(byte[] imageBytes)
        {
            BitmapImage image = new BitmapImage();
            using (InMemoryRandomAccessStream randomAccessStream = new InMemoryRandomAccessStream())
            {
                using (DataWriter writer = new DataWriter(randomAccessStream.GetOutputStreamAt(0)))
                {
                    writer.WriteBytes(imageBytes);
                    await writer.StoreAsync();
                    await image.SetSourceAsync(randomAccessStream);
                }

            }
            return image;
        }
        public static bool InsertImage(string title, BitmapImage Image )
        {
            ObservableCollection<MovieData> result = new ObservableCollection<MovieData>();
            try
            {
                using (var connection = new SQLiteConnection(Windows.Storage.ApplicationData.Current.LocalFolder.Path + "\\" + DB_NAME))
                {
                    var query1 = String.Format("SELECT COUNT(id) FROM Images");
                    var count = connection.Prepare(query1);
                    Debug.WriteLine(count);
                   
                    var query = String.Format("INSERT INTO Images VALUES('{0}','{1}','{2}')",count,title,Image);
                    var statement = connection.Prepare(query);
                    statement.Step();
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }


    }
}
