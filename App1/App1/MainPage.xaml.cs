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
        public Dictionary<int, string> numberToColor = new Dictionary<int, string>();
        // 2 #eee4da
        // 4 #ede0c8
        // 8 #f2b179
        // 16 #f59563 
        // 32 #f67c5f
        // 64 #f65e3b
        // 128 #edcf72
        // 256 #edcc61
        // 512 #edc850
        // 1024 #edc53f
        // 2048 #edc22e                          
 
        public int[][] gameBoard = new int[][] {
            new int[] { 0, 0, 0, 0}, 
            new int[] { 0, 0, 0, 0},
            new int[] { 0, 0, 0, 0},
            new int[] { 0, 0, 0, 0 }
        };
        public int[][] previousGameBoard = new int[][] {
            new int[] { 0, 0, 0, 0},
            new int[] { 0, 0, 0, 0},
            new int[] { 0, 0, 0, 0},
            new int[] { 0, 0, 0, 0 }
        };

        public void saveBoardState ()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    this.previousGameBoard[i][j] = this.gameBoard[i][j];
                }
            }
        }

        public void addNumber (bool forceAdd = false)
        {
            if (forceAdd || this.shoudlAddNumber())
            {
                List<int[]> emptySpots = new List<int[]>();
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (this.gameBoard[i][j] == 0)
                        {
                            emptySpots.Add(new int[] { i, j });
                        }
                    }
                }
                if (emptySpots.Count > 0)
                {
                    Random rnd = new Random();
                    int r = rnd.Next(emptySpots.Count);
                    var randomSpot = emptySpots[r];
                    this.gameBoard[randomSpot[0]][randomSpot[1]] = 2;
                }
            }
        }

        public int[][] Transpose(/*int[][] matrix*/)
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
                    result[i][j] = this.gameBoard[i][j];
                }
            }
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    this.gameBoard[i][j] = result[j][i];
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
            this.saveBoardState();
            this.Transpose();
            for (int i = 0; i < 4; i++)
            {
                this.gameBoard[i] = this.sum(this.gameBoard[i], true);
            }
            this.Transpose();
            this.addNumber();
        }


        public void up()
        {
            this.saveBoardState();
            this.Transpose();
            for (int i = 0; i < 4; i++)
            {
                this.gameBoard[i] = this.sum(this.gameBoard[i], false);
            }
            this.Transpose();
            this.addNumber();
        }

        public void left()
        {
            this.saveBoardState();
            for (int i = 0; i < 4; i++)
            {
                this.gameBoard[i] = this.sum(this.gameBoard[i], false);
            }
            
            this.addNumber();
        }

        public void right()
        {
            this.saveBoardState();
            for (int i = 0; i < 4; i++)
            {
                this.gameBoard[i] = this.sum(this.gameBoard[i], true);
            }
            this.addNumber();
        }

        public bool shoudlAddNumber()
        { 
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (this.previousGameBoard[i][j] != this.gameBoard[i][j])
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void UpdateBoard()
        {
            var stacklayout = (StackLayout)(this.Content);
            stacklayout.Children.Clear();
            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = 100 });
            grid.RowDefinitions.Add(new RowDefinition { Height = 100 });
            grid.RowDefinitions.Add(new RowDefinition { Height = 100 });
            grid.RowDefinitions.Add(new RowDefinition { Height = 100 });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            stacklayout.Children.Add(grid);
            for (int i = 0; i < 4; i++)
            {
                
                //var row = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand };
                //stacklayout.Children.Add(row);
                for (int j = 0; j < 4; j++)
                {
                    var btn = new Button
                    {
                        Text = this.gameBoard[i][j] == 0 ? "" : this.gameBoard[i][j].ToString(),
                        FontSize = 30,
                        BackgroundColor = Color.FromHex(this.numberToColor[this.gameBoard[i][j]])
                    };
                    btn.SetValue(Grid.RowProperty, i);
                    btn.SetValue(Grid.ColumnProperty, j);
                    grid.Children.Add(btn);
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
            this.numberToColor.Add(0, "bbada0");
            this.numberToColor.Add(2, "eee4da");
            this.numberToColor.Add(4, "ede0c8");
            this.numberToColor.Add(8, "f2b179");
            this.numberToColor.Add(16, "f59563");
            this.numberToColor.Add(32, "f67c5f");
            this.numberToColor.Add(64, "f65e3b");
            this.numberToColor.Add(128, "edcf72");
            this.numberToColor.Add(256, "edcc61");
            this.numberToColor.Add(512, "edc850");
            this.numberToColor.Add(1024, "edc53f");
            this.numberToColor.Add(2048, "edc22e");
            this.addNumber(true);
            this.addNumber(true);
            this.UpdateBoard();
 
        }
	}
}