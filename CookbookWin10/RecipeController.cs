using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookbookWin10
{
    class RecipeController
    {
        private List<MainListboxModel> listboxItems;
        //private Recipe dailyRecipe;
        private List<string> categories;

        private EventArgs eventArgs = null;
        public RecipeController(List<MainListboxModel> listboxItems)
        {
            this.listboxItems = listboxItems;
            //randomDailyRecipe();
            this.categories = new List<string>();
            for(int x = 0; x < listboxItems.Count; x++)
            {
                if(!categories.Contains(listboxItems[x].category))
                {
                    categories.Add(listboxItems[x].category);
                }
            }
        }

        public List<string> getCategories()
        {
            return categories;
        }        
        public void randomDailyRecipe()
        {
            Random random = new Random();
            //int idx = random.Next(recipes.Count);
            //dailyRecipe = recipes[idx];
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

        public void setListboxItems(List<MainListboxModel> list)
        {
            this.listboxItems = list;
        }        
    }
}
