using System;
using System.IO;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using System.Net.Http;
using Newtonsoft.Json;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using System.IO;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CookbookWin10
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RecipePage : Page
    {        
        private RecipeExtended recipe;
        private KitchenTimer kitchenTimer;

        public RecipePage()
        {
            this.InitializeComponent();
            kitchenTimer = new KitchenTimer();
            kitchenTimer.Tick += new KitchenTimer.TickHandler(updateKitchenTimer);
            kitchenTimer.TimeDone += new KitchenTimer.TimerDoneHandler(timeDoneKitchenTimer);
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += (s, a) =>
            {
                if (Frame.CanGoBack)
                {
                    Frame.GoBack();
                    a.Handled = true;
                }
            };

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
                recipe.category = input.category;
                recipe.number_of_ratings = input.number_of_ratings;
                recipe.rating = input.rating;                              
            }
            recipeTitle.Text = recipe.title;
            recipePreperation.Text = recipe.preperation;
            recipeAuthor.Text = recipe.author;
            recipeDate.Text = recipe.time;

            recipeText.Text = "\r\nDescription:\r\n" + recipe.description + "\r\n\r\nIngredients: \r\n" + recipe.ingredients + "\r\n\r\nActions:\r\n" + recipe.actions;

            // fetch image
            try {
                page = "http://www.returnoftambelon.com/cookbook/gallery/" + input.id + "/main.jpg";
                Stream st = await client.GetStreamAsync(page);

                var memoryStream = new MemoryStream();
                await st.CopyToAsync(memoryStream);
                memoryStream.Position = 0;
                BitmapImage bitmap = new BitmapImage();
                bitmap.SetSource(memoryStream.AsRandomAccessStream());

                recipeImage.Source = bitmap;
            }
            catch (Exception e)
            {
                
            }
        }

        private void updateKitchenTimer(KitchenTimer kitchenTimer, EventArgs e)
        {
            lbl_stopwatch_minutes.Text = kitchenTimer.getMinutes().ToString("D2");
            lbl_stopwatch_seconds.Text = kitchenTimer.getSeconds().ToString("D2");            
        }
        private void timeDoneKitchenTimer(KitchenTimer kitchenTimer, EventArgs e)
        {
            beepdown.Play();
            btn_stopwatch_toggle.IsEnabled = true;
            showToast("Sushi Chikuwa");
        }

        private void btn_seconds_up_Click(object sender, RoutedEventArgs e)
        {
            beepup.Play();
            kitchenTimer.incrementSeconds();         
        }

        private void btn_minutes_up_Click(object sender, RoutedEventArgs e)
        {
            beepup.Play();
            kitchenTimer.incrementMinutes();
        }

        private void btn_seconds_down_Click(object sender, RoutedEventArgs e)
        {
            beepdown.Play();
            kitchenTimer.decrementSeconds();                      
        }

        private void btn_minutes_down_Click(object sender, RoutedEventArgs e)
        {
            beepdown.Play();
            kitchenTimer.decrementMinutes();     
        }

        private void btn_stopwatch_toggle_Click(object sender, RoutedEventArgs e)
        {
            beepup.Play();            
            btn_stopwatch_toggle.IsEnabled = false;
            kitchenTimer.startKitchenTimer();
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
