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


namespace CookbookWin10
{
    public sealed partial class RecipeEditor : Page
    {
        private RecipeController recipeController;
        public RecipeEditor()
        {
            this.InitializeComponent();

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
            SystemNavigationManager.GetForCurrentView().BackRequested += (s, a) =>
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
            if ((e.Parameter).GetType() == typeof(RecipeController))
            {
                this.recipeController = (RecipeController)e.Parameter;
            }            
        }

        private void btn_editor_category_Click(object sender, RoutedEventArgs e)
        {
            buildFlyout(sender);
        }

        public async void buildFlyout(object sender)
        {
            // make this await, this wont run async now
            MenuFlyout menuFlyout = new MenuFlyout();

            for (int x = 0; x < recipeController.getCategories().Count; x++)
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
                
                setCategory(item.Text);                                
            }
        }

        private void setCategory(string category)
        {
            changeButtonText(category);
            fillRectanglesWithColors(RecipeController.getCategory(category));
        }

        private void fillRectanglesWithColors(int category)
        {
            int[] idx = { 0, 1, 2, 3 };
            MainPage.FisherYatesShuffle(idx);

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

        private void changeButtonText(string category)
        {
            btn_editor_category.Content = category;
        }
    }
}