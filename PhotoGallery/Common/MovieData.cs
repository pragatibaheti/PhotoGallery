using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace PhotoGallery.Common
{
    public class MovieData
    {
        private string _Title;
        private int _id;

        public int id { get; set; }
        public string Title
        {
            get { return this._Title; }
            set { this._Title = value; }
        }

        private BitmapImage _ImageData;
        public BitmapImage ImageData
        {
            get { return this._ImageData; }
            set { this._ImageData = value; }
        }
    }
    public class Image_Model
    {
        public string Img { get; set; }
        public string Title { get; set; }
        public string DateAdded { get; set; }

    }
}
