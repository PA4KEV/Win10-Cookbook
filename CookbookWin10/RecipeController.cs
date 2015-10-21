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

        public List<String> getRecipeTitles()
        {
            if(recipes.Count <= 0) { throw new Exception();  }
            List<String> sideList = new List<string>();
            for (int x = 0; x < recipes.Count; x++)
            {
                sideList.Add(recipes[x].title);
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
    }
}
