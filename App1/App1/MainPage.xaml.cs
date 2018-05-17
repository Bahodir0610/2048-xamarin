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
        static string bomb = new Random().Next(1, 4).ToString();

        public MainPage()
		{
			InitializeComponent();
		}

        async void ButtonClickced(object sender, EventArgs e)
        {
            await DisplayAlert("Bomb Exploded", "GAME OVER", "RETRY");
            Button button = sender as Button;
            button.Text = new Random().Next(1, 400).ToString();
        }
	}
}
