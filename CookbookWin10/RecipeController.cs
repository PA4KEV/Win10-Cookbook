using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookbookWin10
{
    class RecipeController
    {
        private List<Recipe> recipes;
        private Recipe dailyRecipe;

        private EventArgs eventArgs = null;
        public RecipeController(List<Recipe> recipes)
        {
            this.recipes = recipes;
            randomDailyRecipe();
        }

        public List<String> getRecipeTitles()
        {
            if(recipes.Count <= 0) { throw new Exception();  }
            List<String> sideList = new List<string>();
            for (int x = 0; x < recipes.Count; x++)
            {
                sideList.Add(recipes[x].getTitle() + " " + recipes[x].getSubtitle());
            }
            return sideList;            
        }

        public List<Recipe> getRecipes()
        {
            return recipes;
        }
        public void setRecipes(List<Recipe> recipes)
        {
            this.recipes = recipes;
        }
        public Recipe getDailyRecipe()
        {
            return dailyRecipe;
        }
        public void randomDailyRecipe()
        {
            Random random = new Random();
            int idx = random.Next(recipes.Count);
            dailyRecipe = recipes[idx];
        }
    }
}
