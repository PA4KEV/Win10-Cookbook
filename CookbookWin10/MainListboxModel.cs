using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace CookbookWin10
{
    class MainListboxModel
    {
        public int id { get; set; }
        public string date { get; set; }
        public string title { get; set; }
        public string subtitle { get; set; }
        public string category { get; set; }
        public string image { get; set; }
        public BitmapImage bitmapImage { get; set; }        

        public void setBitmapImage(BitmapImage bitmap)
        {
            this.bitmapImage = bitmap;
        }
    }
}
