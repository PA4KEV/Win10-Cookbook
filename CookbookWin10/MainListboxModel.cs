using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
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
        public string types { get; set; }
        public string image { get; set; }
        public BitmapImage bitmapImage { get; set; }
        public SolidColorBrush rectColor { get; set; }

        public int[] getTypesArray()
        {
            string[] myTypes = types.Split(',');
            int[] typesInt = new int[myTypes.Length];
            for (int x = 0; x < myTypes.Length; x++)
            {
                typesInt[x] = int.Parse(myTypes[x]);
            }
            return typesInt;
        }
    }


}
