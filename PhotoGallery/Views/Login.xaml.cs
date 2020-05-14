using PhotoGallery.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238
//C:\Users\praga\AppData\Local\Packages\43164a0c-6895-42c0-80d8-8eb93791a857_vwghp0w08xty0\LocalState
namespace PhotoGallery
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Login : Page
    {
        public Login()
        {
            this.InitializeComponent();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            string username = txtusername.Text;
            string password = txtpassword.Text;
            bool result;
            result = Database.Register(username, password);
            if (result)
            {
                txtpassword.Text = "";
                txtusername.Text = "";
               
            }
            else
                Debug.WriteLine("Try Again!");
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtusername.Text;
            string password = txtpassword.Text;
            bool result;
            result = Database.Login(username, password);
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            //setting value 
            localSettings.Values["userName"] = username;

            //Getting the setting value 
            string value = localSettings.Values["userName"].ToString();
            Debug.WriteLine(value);
            if (result)
                Frame.Navigate(typeof(MainPage));
            else
                Debug.WriteLine("Wrong Credentials!");
        }
    }
}
