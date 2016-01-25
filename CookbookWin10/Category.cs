using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace CookbookWin10
{
    class Category
    {
        public static int DEFAULT = 0;
        public static int SPANISH = 1;
        public static int FRENCH = 2;
        public static int AMERICAN = 3;
        public static int ITALIAN = 4;
        public static int JAPANESE = 5;

        public static Color accentColor = (Color)App.Current.Resources["SystemAccentColor"];
                
        private static byte convertColor(byte input, int mutation)
        {
            if (input + mutation > 255)
                return 255;
            else if (input + mutation < 0)
                return 0;
            else
                return (byte)(input + mutation);
        }

        public static SolidColorBrush[,] colorSets = {
            //Default colors            
            { (new SolidColorBrush(accentColor)),
                (new SolidColorBrush(Color.FromArgb(255, convertColor(accentColor.R, 25), convertColor(accentColor.G, 25), convertColor(accentColor.B, 25)))),
                (new SolidColorBrush(Color.FromArgb(255, convertColor(accentColor.R, -15), convertColor(accentColor.G, -15), convertColor(accentColor.B, -15)))),
                (new SolidColorBrush(Color.FromArgb(255, convertColor(accentColor.R, 15), convertColor(accentColor.G, 15), convertColor(accentColor.B, 15))))
            },
            //Spanish
            { (new SolidColorBrush(Color.FromArgb(255, 214, 78, 32))), (new SolidColorBrush(Color.FromArgb(255, 255, 119, 8))), (new SolidColorBrush(Color.FromArgb(255, 247, 5, 5))), (new SolidColorBrush(Color.FromArgb(255, 255, 193, 8))) },
            //French
            { (new SolidColorBrush(Color.FromArgb(255, 58, 14, 232))), (new SolidColorBrush(Color.FromArgb(255, 149, 64, 201))), (new SolidColorBrush(Color.FromArgb(255, 227, 9, 9))), (new SolidColorBrush(Color.FromArgb(255, 105, 31, 0))) },
            //American
            { (new SolidColorBrush(Color.FromArgb(255, 12, 84, 199))), (new SolidColorBrush(Color.FromArgb(255, 15, 60, 133))), (new SolidColorBrush(Color.FromArgb(255, 247, 5, 5))), (new SolidColorBrush(Color.FromArgb(255, 105, 179, 219))) },
            //Italian
            { (new SolidColorBrush(Color.FromArgb(255, 224, 7, 7))), (new SolidColorBrush(Color.FromArgb(255, 222, 215, 20))), (new SolidColorBrush(Color.FromArgb(255, 102, 230, 67))), (new SolidColorBrush(Color.FromArgb(255, 26, 153, 49))) },
            //Japanese
            { (new SolidColorBrush(Color.FromArgb(255, 224, 7, 7))), (new SolidColorBrush(Color.FromArgb(255, 222, 215, 20))), (new SolidColorBrush(Color.FromArgb(255, 102, 230, 67))), (new SolidColorBrush(Color.FromArgb(255, 26, 153, 49))) }
        };

        public static string[] categoryNames =
        {
            "Default", "Spaans", "Frans", "Amerikaans", "Italiaans", "Japans"
        };
    }
}
