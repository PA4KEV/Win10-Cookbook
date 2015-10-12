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
        public MainPage()
        {
            this.InitializeComponent();
            test();                            
        }

        private void buttonClick(object sender, RoutedEventArgs e)
        {
            //greetingOutput.Text = "Hello " + nameInput.Text + "!";
            this.Frame.Navigate(typeof(RecipePage), null);          
        }

        private async void test()
        {
            string page = "http://www.returnoftambelon.com/cookbook_titles.php";
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(page);
            HttpContent content = response.Content;

            string output = await content.ReadAsStringAsync();
            if(output != null)
            {
                Titlebar.Text = output;
            }

            string json = @"{
              'Name': 'Bad Boys',
              'ReleaseDate': '1995-4-7T00:00:00',
              'Genres': [
                'Action',
                'Comedy'
              ]
            }";
            string json2 = "{'title':'Maguro Sushi','id':1,'image':'maguro_sushi.jpg','rating':7.8,'number_of_ratings':30}";
            //Recipe recipe = JsonConvert.DeserializeObject<Recipe>(output);
            List<Recipe> recipes = JsonConvert.DeserializeObject<List<Recipe>>(output);

            List<String> sideList = new List<string>();
            for(int x = 0; x<recipes.Count; x++)
            {
                sideList.Add(recipes[x].title);
            }
            listbox_mainlist.ItemsSource = sideList;
        }
    }
}
