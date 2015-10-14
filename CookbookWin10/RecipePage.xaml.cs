using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;
using System.Net.Http;
using Newtonsoft.Json;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CookbookWin10
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RecipePage : Page
    {
        DispatcherTimer stopwatch;
        private RecipeExtended recipe;

        public RecipePage()
        {
            this.InitializeComponent();
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;

            stopwatch = new DispatcherTimer();
            stopwatch.Tick += Stopwatch_Tick;
            stopwatch.Interval = new TimeSpan(0, 0, 0, 1);

            

            Image img = new Image();
            img.Source = new BitmapImage(new Uri("ms-appx:///Assets/Logo.png"));
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Recipe recipe = e.Parameter as Recipe;
            //recipeTitle.Text = recipe.title;
            loadRecipe(recipe);

        }

        private async void loadRecipe(Recipe input)
        {
            string page = "http://www.returnoftambelon.com/cookbook_recipes.php?id=" + input.id;
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(page);
            HttpContent content = response.Content;

            string output = await content.ReadAsStringAsync();
            if (output != null)
            {
                recipe = JsonConvert.DeserializeObject<RecipeExtended>(output);
                recipe.image = input.image;
                recipe.number_of_ratings = input.number_of_ratings;
                recipe.rating = input.rating;                              
            }
            recipeTitle.Text = recipe.title;
            recipeAuthor.Text = recipe.author;
            recipeDate.Text = recipe.time;

            recipeText.Text = "\r\nDescription:\r\n" + recipe.description + "\r\n\r\nIngredients: \r\n" + recipe.ingredients + "\r\n\r\nActions:\r\n" + recipe.actions;
        }

        private void btn_seconds_up_Click(object sender, RoutedEventArgs e)
        {
            beepup.Play();
            lbl_stopwatch_seconds.Text = (Int32.Parse(lbl_stopwatch_seconds.Text) + 1 > 60) ? "00" : (Int32.Parse(lbl_stopwatch_seconds.Text) + 1).ToString("D2");            
        }

        private void btn_minutes_up_Click(object sender, RoutedEventArgs e)
        {
            beepup.Play();
            lbl_stopwatch_minutes.Text = (Int32.Parse(lbl_stopwatch_minutes.Text) + 1 > 99) ? "00" : (Int32.Parse(lbl_stopwatch_minutes.Text) + 1).ToString("D2");            
        }

        private void btn_seconds_down_Click(object sender, RoutedEventArgs e)
        {
            beepdown.Play();
            lbl_stopwatch_seconds.Text = (Int32.Parse(lbl_stopwatch_seconds.Text) - 1).ToString("D2");
            if (Int32.Parse(lbl_stopwatch_seconds.Text) < 0)
                lbl_stopwatch_seconds.Text = "59";            
        }

        private void btn_minutes_down_Click(object sender, RoutedEventArgs e)
        {
            beepdown.Play();
            lbl_stopwatch_minutes.Text = (Int32.Parse(lbl_stopwatch_minutes.Text) - 1).ToString("D2");
            if (Int32.Parse(lbl_stopwatch_minutes.Text) < 0)
                lbl_stopwatch_minutes.Text = "99";            
        }

        private void btn_stopwatch_toggle_Click(object sender, RoutedEventArgs e)
        {
            beepup.Play();
            stopwatch.Start();
            btn_stopwatch_toggle.IsEnabled = false;
        }

        private void Stopwatch_Tick(object sender, object e)
        {
            int seconds = Int32.Parse(lbl_stopwatch_seconds.Text);
            int minutes = Int32.Parse(lbl_stopwatch_minutes.Text);

            if ((seconds - 1) < 0)  {
                seconds = 59;
                if ((minutes - 1) >= 0) {
                    minutes--;
                }                
            }
            else {
                seconds--;
            }

            if (minutes <= 0 && seconds <= 0) {
                seconds = minutes = 0;
                beepdown.Play();
                stopwatch.Stop();
                btn_stopwatch_toggle.IsEnabled = true;
                showToast("Sushi Chikuwa");
            }
            lbl_stopwatch_seconds.Text = seconds.ToString("D2");
            lbl_stopwatch_minutes.Text = minutes.ToString("D2");
        }
        private void showToast(string recipeName)
        {
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText04);

            // Fill in the text elements
            XmlNodeList stringElements = toastXml.GetElementsByTagName("text");

            stringElements[0].AppendChild(toastXml.CreateTextNode("Alarm Finished!"));
            stringElements[1].AppendChild(toastXml.CreateTextNode(recipeName));

            // Specify the absolute path to an image
            String imagePath = "file:///" + Path.GetFullPath("toastImageAndText.png");
            XmlNodeList imageElements = toastXml.GetElementsByTagName("image");

            ToastNotification toast = new ToastNotification(toastXml);

            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }
    }
}
