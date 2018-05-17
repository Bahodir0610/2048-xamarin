using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1
{
    public partial class MainPage : ContentPage
    {

        //StackLayout parent;

        static string bomb = new Random().Next(1, 4).ToString();
        public string generateText ()
        {
            return new Random().Next(1, 4).ToString();
        }

        public void AddButton (object sender, EventArgs e)
        {
            Button button = new Button
            {
                Text = "My button"
            };
            var stacklayout = (StackLayout)(this.Content);
            stacklayout.Children.Add(button);

        }

        public MainPage()
		{
			InitializeComponent();
            var stacklayout = (StackLayout)(this.Content);

            int[,] array2D = new int[,] { { 2, 4, 8, 16}, { 2, 2, 2, 4 }, { 4, 8, 64, 2 }, { 8, 8, 32, 32 } };
            for (int i = 0; i < 4; i++)
            {
                var row = new StackLayout { Orientation= StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand};
                stacklayout.Children.Add(row);
                for (int j = 0; j < 4; j++)
                {

                    row.Children.Add(new Button
                    {
                        Text = array2D[i, j].ToString(),
                        WidthRequest = 60
                    });
                }
            }
            var controls = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand };
            controls.Children.Add(new Button
            {
                Text = "Up",
                WidthRequest = 60
            });

            controls.Children.Add(new Button
            {
                Text = "Down",
                WidthRequest = 60
            });
            controls.Children.Add(new Button
            {
                Text = "Left",
                WidthRequest = 60
            });
            controls.Children.Add(new Button
            {
                Text = "Right",
                WidthRequest = 60
            });
            stacklayout.Children.Add(controls);
            //Button button = new Button
            //{
            //    Text = "My button"
            //};

        }

        async void ButtonClickced(object sender, EventArgs e)
        {
            Button button = sender as Button;
            await DisplayAlert(button.Text, "GAME OVER", "RETRY");
            // button.Text = new Random().Next(1, 400).ToString();
        }
	}
}