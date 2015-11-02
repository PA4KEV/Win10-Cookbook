using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace CookbookWin10
{
    class RecipeController
    {
        private List<MainListboxModel> listboxItems;
        private MainListboxModel dailyRecipe;
        private List<string> categories;

        private EventArgs eventArgs = null;
        public event JsonReadyHandler jsonReady;
        public delegate void JsonReadyHandler(RecipeController rc, EventArgs e);

        public RecipeController()
        {
            retrieveJSON();        
        }

        private async void retrieveJSON()
        {
            string page = "http://www.returnoftambelon.com/koken_recepten.php?section=main";
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(page);
            HttpContent content = response.Content;

            string output = await content.ReadAsStringAsync();
            if (output != null)
            {
                this.listboxItems = JsonConvert.DeserializeObject<List<MainListboxModel>>(output);
                
                for (int x = 0; x < getListboxItems().Count; x++)
                {
                    BitmapImage img = new BitmapImage();
                    if (getListboxItems()[x].image.Length != 0)
                    {
                        try
                        {
                            page = "http://www.returnoftambelon.com/cookbook/gallery/" + getListboxItems()[x].image;
                            Stream st = await client.GetStreamAsync(page);

                            var memoryStream = new MemoryStream();
                            await st.CopyToAsync(memoryStream);
                            memoryStream.Position = 0;
                            img.SetSource(memoryStream.AsRandomAccessStream());
                        }
                        catch (Exception e)
                        {

                        }
                        getListboxItems()[x].bitmapImage = img;
                    }

                    Random random = new Random();
                    int colorIndex = random.Next(4);
                    if (getListboxItems()[x].category.Equals("Spaans"))
                    {
                        getListboxItems()[x].rectColor = (CategoryColor.sets[CategoryColor.SPANISH, colorIndex]);
                    }
                    else if (getListboxItems()[x].category.Equals("Frans"))
                    {
                        getListboxItems()[x].rectColor = (CategoryColor.sets[CategoryColor.FRENCH, colorIndex]);
                    }
                }

                this.categories = new List<string>();
                for (int x = 0; x < listboxItems.Count; x++)
                {
                    if (!categories.Contains(listboxItems[x].category))
                    {
                        categories.Add(listboxItems[x].category);
                    }
                }
                jsonReady(this, eventArgs);
            }
        }

        public List<string> getCategories()
        {
            return categories;
        }        
        public void randomDailyRecipe()
        {
            Random random = new Random();
            int idx = random.Next(listboxItems.Count);
            dailyRecipe = listboxItems[idx];
        }

        public MainListboxModel getDailyRecipe()
        {
            return dailyRecipe;
        }

        public List<MainListboxModel> getListboxItems()
        {
            return listboxItems;
        }

        public List<MainListboxModel> getListboxItems(string category)
        {
            if(category.Equals("all"))
            {
                return listboxItems;
            }

            List<MainListboxModel> list = new List<MainListboxModel>();
            for(int x = 0; x < listboxItems.Count; x++)
            {
                if(listboxItems[x].category.Equals(category))
                {
                    list.Add(listboxItems[x]);
                }
            }
            return list;
        }

        public void setListBoxColors(int catColorID)
        {
            foreach (MainListboxModel model in listboxItems)
            {
                model.rectColor = CategoryColor.sets[catColorID, 0];
            }
        }

        public void setListboxItems(List<MainListboxModel> list)
        {
            this.listboxItems = list;
        }        
    }
}
