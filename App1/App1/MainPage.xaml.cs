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
        int[][] gameBoard = new int[][] {
            new int[] { 0, 0, 0, 0}, 
            new int[] { 0, 0, 0, 0},
            new int[] { 0, 0, 0, 0},
            new int[] { 0, 0, 0, 0}
        };

        public void addNumber ()
        {
            List<int[]> emptySpots = new List<int[]>();
            for (int i = 0; i < 4; i++) {
                for (int j = 0; j < 4; j++) {
                    if (this.gameBoard[i][j] == 0)
                    {
                        emptySpots.Add(new int[] { i, j });
                    }
                }
            }
            Random rnd = new Random();

            int r = rnd.Next(emptySpots.Count);
            var randomSpot = emptySpots[r];
            this.gameBoard[randomSpot[0]][randomSpot[1]] = 2;
        }

         public int[][] Transpose(int[][] matrix)
        {
            int w = 4;
            int h = 4;

            int[][] result = new int[h][];
            result[0] = new int[4];
            result[1] = new int[4];
            result[2] = new int[4];
            result[3] = new int[4];

            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    result[j][i] = matrix[i][j];
                }
            }

            return result;
        }

        public int[] sum(int[] arr, bool reverse)
        {
            int len = arr.Length;
            var filterd = arr.Where(c => c != 0).ToArray();
            List<int> result = new List<int>();
            if (reverse)
            {
                for (int i = 0; i < filterd.Length / 2; i++)
                {
                    int tmp = filterd[i];
                    filterd[i] = filterd[filterd.Length - i - 1];
                    filterd[filterd.Length - i - 1] = tmp;
                }
            }

            for (int i = 0; i < filterd.Length;)
            {
                if (i != filterd.Length - 1 && filterd[i] == filterd[i + 1])
                {
                    result.Add(2 * filterd[i]);
                    i += 2;
                }
                else
                {
                    result.Add(filterd[i]);
                    i++;
                }
            }
            int diff = len - result.Count();
            for (int i = 0; i < diff; i++)
            {
                result.Add(0);
            }
            if (reverse)
            {
                result.Reverse();
            }
            return result.ToArray();
        }


        public void down ()
        {
            var transposed = this.Transpose(this.gameBoard);
            transposed[0] = this.sum(transposed[0], true);
            transposed[1] = this.sum(transposed[1], true);
            transposed[2] = this.sum(transposed[2], true);
            transposed[3] = this.sum(transposed[3], true);
            this.gameBoard = this.Transpose(transposed);
            this.addNumber();
        }


        public void up()
        {
            var transposed = this.Transpose(this.gameBoard);
            transposed[0] = this.sum(transposed[0], false);
            transposed[1] = this.sum(transposed[1], false);
            transposed[2] = this.sum(transposed[2], false);
            transposed[3] = this.sum(transposed[3], false);
            this.gameBoard = this.Transpose(transposed);
            this.addNumber();
        }

        public void left()
        {
            this.gameBoard[0] = this.sum(this.gameBoard[0], false);
            this.gameBoard[1] = this.sum(this.gameBoard[1], false);
            this.gameBoard[2] = this.sum(this.gameBoard[2], false);
            this.gameBoard[3] = this.sum(this.gameBoard[3], false);
            this.addNumber();
        }

        public void right()
        {
            this.gameBoard[0] = this.sum(this.gameBoard[0], true);
            this.gameBoard[1] = this.sum(this.gameBoard[1], true);
            this.gameBoard[2] = this.sum(this.gameBoard[2], true);
            this.gameBoard[3] = this.sum(this.gameBoard[3], true);
            this.addNumber();
        }

        public void UpdateBoard()
        {
            var stacklayout = (StackLayout)(this.Content);
            stacklayout.Children.Clear();
            for (int i = 0; i < 4; i++)
            {
                var row = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand };
                stacklayout.Children.Add(row);
                for (int j = 0; j < 4; j++)
                {
                    row.Children.Add(new Button
                    {
                        Text = this.gameBoard[i][j] == 0 ? "" : this.gameBoard[i][j].ToString(),
                        WidthRequest = 60,
                    });
                }
            }
            var controls = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand };
            var up = new Button
            {
                Text = "Up",
                WidthRequest = 60
            };
            up.Clicked += (s, e) =>
            {
                this.up();
                this.UpdateBoard();
            };
            controls.Children.Add(up);

   
            var down = new Button
            {
                Text = "Down",
                WidthRequest = 60
            };
            down.Clicked += (s, e) =>
            {
                this.down();
                this.UpdateBoard();
            };
            controls.Children.Add(down);

            var left = new Button
            {
                Text = "Left",
                WidthRequest = 60
            };
            left.Clicked += (s, e) =>
            {
                this.left();
                this.UpdateBoard();
            };
            controls.Children.Add(left);


            var right = new Button
            {
                Text = "Right",
                WidthRequest = 60
            };
            right.Clicked += (s, e) =>
            {
                this.right();
                this.UpdateBoard();
            };
            controls.Children.Add(right);

            stacklayout.Children.Add(controls);
        }

        public MainPage()
		{
			InitializeComponent();
            this.addNumber();
            this.addNumber();
            this.UpdateBoard();
 
        }
	}
}