using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookbookWin10
{
    public class Recipe
    {
        public string title;
        public int id;
        public double rating;
        public int number_of_ratings;
        public string image;

        public string getTitle()
        {
            return title;
        }
        public int getID()
        {
            return id;
        }
        public double getRating()
        {
            return rating;
        }
        public int getNumberOfRatings()
        {
            return number_of_ratings;
        }
        public string getImageString()
        {
            return image;
        }
    }

    public class RecipeExtended : Recipe
    {
        public string author;
        public string preperation;
        public string time;
        public string ingredients;
        public string actions;
        public string description;
    }
}
