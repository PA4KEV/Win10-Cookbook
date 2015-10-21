using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Net.Http;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CookbookWin10
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {        
        private RecipeController recipeController;

        public MainPage()
        {
            this.InitializeComponent();
            recipeController = new RecipeController();
            retrieveJSON();            
        }

        private void newDailyRecipe(object sender, RoutedEventArgs e)
        {
            recipeController.randomDailyRecipe();
            lblDailyTitle.Text = recipeController.getDailyRecipe().title;
            lblDailyCategory.Text = recipeController.getDailyRecipe().category;
        }

        private async void retrieveJSON()
        {
            string page = "http://www.returnoftambelon.com/cookbook_titles.php";
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(page);
            HttpContent content = response.Content;

            string output = await content.ReadAsStringAsync();
            if(output != null)
            {
                //lblDailyRecipe.Text = output;
            }
            recipeController.setRecipes(JsonConvert.DeserializeObject<List<Recipe>>(output));
            recipeController.randomDailyRecipe();
            listbox_mainlist.ItemsSource = recipeController.getRecipeTitles();
        }

        private void navigateToRecipePage(object sender, SelectionChangedEventArgs e)
        {
            string title = listbox_mainlist.SelectedItem.ToString();
            
            this.Frame.Navigate(typeof(RecipePage), recipeController.getRecipes()[listbox_mainlist.SelectedIndex]);
        }
    }
}
