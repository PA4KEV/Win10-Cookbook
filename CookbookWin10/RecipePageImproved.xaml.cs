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
    public sealed partial class RecipePageImproved : Page
    {
        private Recipe recipe;
        private KitchenTimer kitchenTimer;
        public RecipePageImproved()
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
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Recipe recipe = e.Parameter as Recipe;
            //recipeTitle.Text = recipe.title;
            loadRecipe(recipe);
        }

        private async void loadRecipe(Recipe input)
        {
            string page = "http://www.returnoftambelon.com/koken_recepten.php?id=" + input.id;
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(page);
            HttpContent content = response.Content;

            string output = await content.ReadAsStringAsync();
            if (output != null)
            {
                recipe = JsonConvert.DeserializeObject<Recipe>(output);
            }            
            
            fillRectanglesWithColors(recipe.getCategoryInteger());
            fillXamlElements(recipe);

            if (recipe.getImageString().Length != 0)
            {
                try
                {
                    page = "http://www.returnoftambelon.com/cookbook/gallery/" + recipe.getImageString();
                    Stream st = await client.GetStreamAsync(page);

                    var memoryStream = new MemoryStream();
                    await st.CopyToAsync(memoryStream);
                    memoryStream.Position = 0;
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.SetSource(memoryStream.AsRandomAccessStream());

                    img_main.Source = bitmap;
                }
                catch (Exception e)
                {

                }
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

        private void fillXamlElements(Recipe recipe)
        {
            lbl_title.Text = recipe.getTitle();
            lbl_subtitle.Text = recipe.getSubtitle();
            lbl_recipe.Text = recipe.getRecipe();
            lbl_type.Text = recipe.getRecipeType();
            lbl_rect_left.Text = recipe.getPersons() + " personen";
            lbl_rect_middle.Text = recipe.getPreperationTime();
            lbl_rect_right.Text = recipe.getAuthor();            

            string[] ingredients = recipe.getIngredients();
            string output_left = "";
            string output_right = "";
            for (int x = 0; x < 10 && x < ingredients.Length; x++)
            {
                if (x < 5)
                    output_left += ingredients[x].Trim() + "\n";
                else
                    output_right += ingredients[x].Trim() + "\n";
            }
            lbl_ingredients_left.Text = output_left;
            lbl_ingredients_right.Text = output_right;

            if (recipe.getTip().Length != 0)
            {
                lbl_tip_title_low.Text = "Tip:";
                lbl_tip_low.Text = recipe.getTip();
                img_tip_low.Source = new BitmapImage(new Uri("ms-appx:///Assets/tip.png"));
                if (recipe.getWineTip().Length != 0)
                {
                    lbl_tip_title_top.Text = "Wijntip:";
                    lbl_tip_top.Text = recipe.getWineTip();
                    img_tip_top.Source = new BitmapImage(new Uri("ms-appx:///Assets/wine.png"));
                }
            }
            else if (recipe.getWineTip().Length != 0)
            {
                lbl_tip_title_low.Text = "Wijntip:";
                lbl_tip_low.Text = recipe.getWineTip();
                img_tip_low.Source = new BitmapImage(new Uri("ms-appx:///Assets/wine.png"));
            }            
        }

        private void fillRectanglesWithColors(int category)
        {
            int[] idx = { 0, 1, 2, 3 };
            FisherYatesShuffle(idx);

            rect_main.Fill = CategoryColor.sets[category, idx[0]];
            rect_ingredients.Fill = CategoryColor.sets[category, idx[1]];
            rect_left_low.Fill = CategoryColor.sets[category, idx[2]];
            rect_left_top.Fill = CategoryColor.sets[category, idx[3]];
            rect_sub_left.Fill = CategoryColor.sets[category, 0];
            rect_sub_middle.Fill = CategoryColor.sets[category, 1];
            rect_sub_right.Fill = CategoryColor.sets[category, 2];

            lbl_title.Foreground = CategoryColor.sets[category, idx[1]];
            lbl_subtitle.Foreground = CategoryColor.sets[category, idx[2]];
        }

        private static void FisherYatesShuffle<T>(T[] array)
        {
            Random _random = new Random();
            int n = array.Length;
            for (int i = 0; i < n; i++)
            {
                // NextDouble returns a random number between 0 and 1.
                // ... It is equivalent to Math.random() in Java.
                int r = i + (int)(_random.NextDouble() * (n - i));
                T t = array[r];
                array[r] = array[i];
                array[i] = t;
            }
        }

    }
}
