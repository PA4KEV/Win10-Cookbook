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
        public string type;
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
        public string getAuthor()
        {
            return author;
        }
        public string getRecipeType()
        {
            return type;
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
        public string getImageString()
        {
            return image;
        }
    }
}
