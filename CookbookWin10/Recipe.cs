using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookbookWin10
{
    public class Recipe
    {
        public int id;
        public string date;
        public string title;
        public string subtitle;
        public string category;
        public string author;
        public string types;
        public int persons;
        public string preperationTime;
        public string recipe;
        public string tip;
        public string winetip;
        public string ingredients;
        public string image;


        public int getID()
        {
            return id;
        }
        public string getTitle()
        {
            return title;
        }
        public string getSubtitle()
        {
            return subtitle;
        }
        public string getCategory()
        {
            return category;
        }
        public int getCategoryInteger()
        {
            return RecipeController.getCategory(category);
        }
        public string getAuthor()
        {
            return author;
        }
        public string getRecipeType()
        {
            StringBuilder sb = new StringBuilder();
            string[] myTypes = types.Split(',');
            for(int x = 0; x < myTypes.Length; x++)
            {
                sb.Append(RecipeController.getRecipeTypes()[int.Parse(myTypes[x])]);
                if (x != (myTypes.Length - 1))
                    sb.Append(", ");
            }
            return sb.ToString();
        }
        public int[] getRecipeTypeArray()
        {
            string[] myTypes = types.Split(',');
            int[] typesInt = new int[myTypes.Length];
            for(int x = 0; x < myTypes.Length; x++)
            {
                typesInt[x] = int.Parse(myTypes[x]);
            }
            return typesInt;
        }
        public int getPersons()
        {
            return persons;
        }
        public string getPreperationTime()
        {
            return preperationTime;
        }
        public string getRecipe()
        {
            return recipe;
        }
        public string getTip()
        {
            return tip;
        }
        public string getWineTip()
        {
            return winetip;
        }
        public string[] getIngredients()
        {
            return ingredients.Split(',');
        }
        public string getImageString()
        {
            return image;
        }
    }
}
