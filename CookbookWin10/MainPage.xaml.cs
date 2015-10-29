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
using Windows.UI.Core;
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CookbookWin10
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {        
        private RecipeController recipeController;
        private string category = "all";     

        public MainPage()
        {
            this.InitializeComponent();
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            retrieveJSON();            
        }

        private async void newDailyRecipe(object sender, RoutedEventArgs e)
        {
            //recipeController.randomDailyRecipe();
            //lblDailyTitle.Text = recipeController.getDailyRecipe().title;
            //lblDailyCategory.Text = recipeController.getDailyRecipe().category;

            HttpClient client = new HttpClient();
            try
            {
                int id = 0;
                string page = "http://www.returnoftambelon.com/cookbook/gallery/" + id + "/main.jpg";
                Stream st = await client.GetStreamAsync(page);

                var memoryStream = new MemoryStream();
                await st.CopyToAsync(memoryStream);
                memoryStream.Position = 0;
                BitmapImage bitmap = new BitmapImage();
                bitmap.SetSource(memoryStream.AsRandomAccessStream());

                //imgDailyImage.Source = bitmap;
            }
            catch (Exception ex)
            {

            }
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
                recipeController = new RecipeController(JsonConvert.DeserializeObject<List<MainListboxModel>>(output));

                for (int x = 0; x < recipeController.getListboxItems().Count; x++)
                {
                    BitmapImage img = new BitmapImage();
                    if (recipeController.getListboxItems()[x].image.Length != 0)
                    {
                        try
                        {
                            page = "http://www.returnoftambelon.com/cookbook/gallery/" + recipeController.getListboxItems()[x].image;
                            Stream st = await client.GetStreamAsync(page);

                            var memoryStream = new MemoryStream();
                            await st.CopyToAsync(memoryStream);
                            memoryStream.Position = 0;
                            img.SetSource(memoryStream.AsRandomAccessStream());
                        }
                        catch (Exception e)
                        {

                        }
                        recipeController.getListboxItems()[x].setBitmapImage(img);
                    }                    
                }
            }
        }

        private void updateMainListboxes(string category)
        {
            lbox_main_0.ItemsSource = recipeController.getListboxItems(category);
        }
        private void updateMainMenuColors(string category)
        {
            if(category.Equals("Spaans"))
            {

            }
            else if(category.Equals("Frans"))
            {

            }
        }

        // Click events

        private void navigateToRecipePage(object sender, SelectionChangedEventArgs e)
        {
            this.Frame.Navigate(typeof(RecipePageImproved), recipeController.getListboxItems(this.category)[lbox_main_0.SelectedIndex]);        
        }        

        private void btn_category_Click(object sender, RoutedEventArgs e)
        {
            MenuFlyout menuFlyout = new MenuFlyout();

            for(int x = 0; x < recipeController.getCategories().Count; x++)
            {
                MenuFlyoutItem flyItem = new MenuFlyoutItem();
                flyItem.Text = recipeController.getCategories()[x];
                flyItem.Click += FlyItem_Click;
                menuFlyout.Items.Add(flyItem);
            }            
            menuFlyout.ShowAt((FrameworkElement)sender);
        }

        private void FlyItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender.GetType() == typeof(MenuFlyoutItem))
            {                
                MenuFlyoutItem item = (MenuFlyoutItem)sender;
                this.category = item.Text;
                updateMainListboxes(item.Text);
                updateMainMenuColors(item.Text);    
            }            
        }
    }
}
