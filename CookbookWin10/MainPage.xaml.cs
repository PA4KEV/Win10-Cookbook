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
using Windows.UI.Xaml.Shapes;
using Windows.UI;
using Windows.UI.Notifications;
using Windows.ApplicationModel.Background;
using Windows.UI.Popups;
using Windows.Data.Xml.Dom;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CookbookWin10
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {        
        private RecipeController recipeController;
        public static string category = "all";     

        public MainPage()
        {
            this.InitializeComponent();
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            //registerBackgroundTask(); 
                       
            
            recipeController = new RecipeController();
            recipeController.jsonReady += RecipeController_jsonReady;            
        }

        private void RecipeController_jsonReady(RecipeController rc, EventArgs e)
        {
            btn_category.Visibility = Visibility.Visible;
            btn_daily.Visibility = Visibility.Visible;
            if (!MainPage.category.Equals("all"))
            {
                updateMainListboxes(MainPage.category);
                updateMainMenuColors(MainPage.category);
            }
        }

        private async void registerBackgroundTask()
        {
            string myTaskName = "TileUpdateTask";
            // check if task is already registered

            foreach (var cur in BackgroundTaskRegistration.AllTasks)
                if (cur.Value.Name == myTaskName)
                {
                    await (new MessageDialog("Task already registered")).ShowAsync();
                    return;
                }
            
            // Windows Phone app must call this to use trigger types (see MSDN)
            await BackgroundExecutionManager.RequestAccessAsync();

            // register a new task
            BackgroundTaskBuilder taskBuilder = new BackgroundTaskBuilder { Name = "TileUpdate Task", TaskEntryPoint = "BackgroundTasks.TileUpdateTask" };
            taskBuilder.SetTrigger(new TimeTrigger(15, true));
            BackgroundTaskRegistration myTileUpdateTask = taskBuilder.Register();
            await (new MessageDialog("Task registered")).ShowAsync();
        }


        private void newDailyRecipe(object sender, RoutedEventArgs e)
        {
            recipeController.randomDailyRecipe();
            UpdateTile(recipeController.getDailyRecipe());

            //HttpClient client = new HttpClient();
            //try
            //{
            //    int id = 0;
            //    string page = "http://www.returnoftambelon.com/cookbook/gallery/" + id + "/main.jpg";
            //    Stream st = await client.GetStreamAsync(page);

            //    var memoryStream = new MemoryStream();
            //    await st.CopyToAsync(memoryStream);
            //    memoryStream.Position = 0;
            //    BitmapImage bitmap = new BitmapImage();
            //    bitmap.SetSource(memoryStream.AsRandomAccessStream());

            //    //imgDailyImage.Source = bitmap;
            //}
            //catch (Exception ex)
            //{

            //}
        }        

        

        private void UpdateTile(MainListboxModel item)
        {
            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.EnableNotificationQueue(true);
            updater.Clear();
            Windows.Data.Xml.Dom.XmlDocument xml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquare150x150PeekImageAndText01);

            xml.GetElementsByTagName("text")[0].InnerText = item.title;
            xml.GetElementsByTagName("text")[1].InnerText = item.subtitle;            
            xml.GetElementsByTagName("text")[3].InnerText = "Heerlijk " + item.category;
            
            XmlNodeList squareImageElements = xml.GetElementsByTagName("image");
            XmlElement squareImageElement = (XmlElement)squareImageElements.Item(0);
            squareImageElement.SetAttribute("src", "http://www.returnoftambelon.com/cookbook/gallery/" + item.image); //http://www.returnoftambelon.com/cookbook/gallery/paella.jpg


            updater.Update(new TileNotification(xml));
        }

        private async void updateMainListboxes(string category)
        {
            MainListboxModel[] recipes;

            if (category.Equals("Favorites"))
            {
                List<MainListboxModel> list = await recipeController.getFavorites();
                recipes = list.ToArray();
            } 
            else
            {
                recipes = recipeController.getListboxItems(category).ToArray();
            }

            FisherYatesShuffle(recipes);
            ListView[] lists = { lbox_main_0, lbox_main_1, lbox_main_2, lbox_main_3, lbox_main_4 };

            // knullige zooi...
            List<MainListboxModel>[] subLists = { new List<MainListboxModel>(), new List<MainListboxModel>(), new List<MainListboxModel>(), new List<MainListboxModel>(), new List<MainListboxModel>() };

            for (int x = 0; x < recipes.Length; x++)
            {
                subLists[(x % subLists.Length)].Add(recipes[x]);
            }           
            for (int y = 0; y < lists.Length; y++)
            {
                lists[y].ItemsSource = subLists[y];
            }                    
        }              

        private void updateMainMenuColors(string category)
        {
            int catColor = 0;
            if (category.Equals("Favorites"))
            {
                catColor = CategoryColor.ITALIAN;
            }
            else if (category.Equals("Spaans"))
            {
                catColor = CategoryColor.SPANISH;
            }
            else if (category.Equals("Frans"))
            {
                catColor = CategoryColor.FRENCH;
            }
            else if (category.Equals("Amerikaans"))
            {
                catColor = CategoryColor.AMERICAN;
            }
            else if (category.Equals("Italiaans"))
            {
                catColor = CategoryColor.ITALIAN;
            }
            colorRectangles(catColor);
            lbl_main_menu_welcome.Text = (category.Equals("Favorites")) ? "Heerlijk Mijn Favorieten Koken" : "Heerlijk " + category + " Koken";            
        }

        // Click events

        private void navigateToRecipePage(object sender, SelectionChangedEventArgs e)
        {
            if(sender.GetType() == typeof(ListView))
            {
                ListView listView = (ListView)sender;

                MainListboxModel model = (MainListboxModel)listView.Items[listView.SelectedIndex];

                this.Frame.Navigate(typeof(RecipePageImproved), model);
            }            
        }        

        private void btn_category_Click(object sender, RoutedEventArgs e)
        {
            buildFlyout(sender);
        }

        private async void buildFlyout(object sender)
        {
            MenuFlyout menuFlyout = new MenuFlyout();

            for (int x = 0; x < recipeController.getCategories().Count; x++)
            {
                MenuFlyoutItem flyItem = new MenuFlyoutItem();
                flyItem.Text = recipeController.getCategories()[x];
                flyItem.Click += FlyItem_Click;
                menuFlyout.Items.Add(flyItem);
            }
            StorageManager storageManager = new StorageManager();

            bool fileExists = await storageManager.fileExists();
            if (fileExists)
            {
                MenuFlyoutItem favItem = new MenuFlyoutItem();
                favItem.Text = "Mijn Favorieten";
                favItem.Click += FlyItem_Click_Favorites;
                menuFlyout.Items.Add(favItem);
            }

            menuFlyout.ShowAt((FrameworkElement)sender);
        }

        private void btn_daily_Click(object sender, RoutedEventArgs e)
        {
            recipeController.randomDailyRecipe();
            UpdateTile(recipeController.getDailyRecipe());
        }

        private void FlyItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender.GetType() == typeof(MenuFlyoutItem))
            {                
                MenuFlyoutItem item = (MenuFlyoutItem)sender;
                MainPage.category = item.Text;
                
                updateMainMenuColors(item.Text);
                updateMainListboxes(item.Text);
            }            
        }

        private void FlyItem_Click_Favorites(object sender, RoutedEventArgs e)
        {
            if (sender.GetType() == typeof(MenuFlyoutItem))
            {
                MenuFlyoutItem item = (MenuFlyoutItem)sender;
                
                updateMainMenuColors("Favorites");
                updateMainListboxes("Favorites");
            }
        }

        // Colors

        private void colorRectangles(int catColorID)
        {
            // deal with when 2 colors are next to eachother
            int[] keys = { 0, 0, 1, 2, 3 };
            FisherYatesShuffle(keys);

            Rectangle[] rectangles = { rect_main_0, rect_main_1, rect_main_2, rect_main_3, rect_main_4 };
            ListView[] lists = { lbox_main_0, lbox_main_1, lbox_main_2, lbox_main_3, lbox_main_4 };

            for (int x = 0; x < keys.Length; x++)
            {
                rectangles[x].Fill = CategoryColor.sets[catColorID, keys[x]];
                lists[x].Background = CategoryColor.sets[catColorID, keys[x]];
            }
            Random random = new Random();            
            rect_main_menu.Fill = CategoryColor.sets[catColorID, keys[random.Next(keys.Length)]];
                        
            //recipeController.setListBoxColors(catColorID);            
        }

        public static void FisherYatesShuffle<T>(T[] array)
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
