using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Diagnostics;

namespace App1
{
    public partial class MainPage : ContentPage, ISwipeCallBack
    {
        public void onLeftSwipe(View view)
        {
            this.left();
        }
        public void onRightSwipe(View view)
        {
            this.right();
        }
        public void onTopSwipe(View view)
        {
            this.up();
        }
        public void onBottomSwipe(View view)
        {
            this.down();
        }
        public void onNothingSwiped(View view)
        {

        }

        public Dictionary<int, string> numberToColor = new Dictionary<int, string>();
        public int BOARD_SIZE = 4;
        public Grid grid;
        private Random rnd = new Random();
        public double APP_WIDTH = 320;
        public int gameScore = 0;

        public void startNewGame()
        {
            this.gameScore = 0;
            this.updateGrid();
            this.gameBoard = this.generateBoard();
            this.previousGameBoard = this.generateBoard();
            this.addNumber(true);
            this.addNumber(true);
        }

        public int[][] generateBoard ()
        {
            var newBoard = new int[this.BOARD_SIZE][];
            for (var i = 0; i < this.BOARD_SIZE; i++)
            {
                var row = new int[this.BOARD_SIZE];
                for (var j = 0; j < this.BOARD_SIZE; j++)
                    row[j] = 0;

                newBoard[i] = row;
            }
            return newBoard;
        }
        public int[][] gameBoard;
        public int[][] previousGameBoard;

        public void saveBoardState ()
        {
            for (int i = 0; i < this.BOARD_SIZE; i++)
                for (int j = 0; j < this.BOARD_SIZE; j++)
                    this.previousGameBoard[i][j] = this.gameBoard[i][j];
        }

        public bool canAdd()
        {
            for (var i = 0; i < this.BOARD_SIZE; i++)
            {
                var summed = this.sum(this.gameBoard[i], false); // no matter if true or false
                for (var j = 0; j < this.BOARD_SIZE; j++)
                    if (summed[j] != this.gameBoard[i][j]) return true;
            }
            return false;
        }

        async public void addNumber (bool forceAdd = false)
        {
            List<int[]> emptySpots = new List<int[]>();
            for (int i = 0; i < this.BOARD_SIZE; i++)
                for (int j = 0; j < this.BOARD_SIZE; j++)
                    if (this.gameBoard[i][j] == 0)
                        emptySpots.Add(new int[] { i, j });
            
            if (emptySpots.Count == 0)
            {
                var canAddHorizontally = this.canAdd();
                this.Transpose();
                var canAddVertically = this.canAdd();
                if (!canAddHorizontally && !canAddVertically)
                {
                    var action = await DisplayAlert("GameOver", "Start a new game?", "Yes", "No");
                    if (action == true)
                        this.startNewGame();
                                       }
                else
                    this.Transpose();
            } else if (forceAdd || this.shoudlAddNumber())
            {
                
                int r = rnd.Next(emptySpots.Count);
                var randomSpot = emptySpots[r];
                var randomIntToDeterminateWhichNumberAddToBoard = rnd.Next(0, 10);
                this.gameBoard[randomSpot[0]][randomSpot[1]] = randomIntToDeterminateWhichNumberAddToBoard < 8 ? 2 : 4;
                scoreLabel.Text = "Score: " + this.gameScore.ToString();
                this.UpdateBoard();
            }
        }

        public int[][] Transpose()
        {
            int[][] result = new int[this.BOARD_SIZE][];
            for (int i = 0; i < this.BOARD_SIZE; i++)
                result[i] = new int[this.BOARD_SIZE];
            
            for (int i = 0; i < this.BOARD_SIZE; i++)
                for (int j = 0; j < this.BOARD_SIZE; j++)
                    result[i][j] = this.gameBoard[i][j];

            for (int i = 0; i < this.BOARD_SIZE; i++)
                for (int j = 0; j < this.BOARD_SIZE; j++)
                    this.gameBoard[i][j] = result[j][i];

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
                    this.gameScore += 2 * filterd[i];
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
                result.Add(0);

            if (reverse)            
                result.Reverse();

            return result.ToArray();
        }


        public void down ()
        {
            this.saveBoardState();
            this.Transpose();
            for (int i = 0; i < this.BOARD_SIZE; i++)
                this.gameBoard[i] = this.sum(this.gameBoard[i], true);

            this.Transpose();
            this.addNumber();
        }


        public void up()
        {
            this.saveBoardState();
            this.Transpose();
            for (int i = 0; i < this.BOARD_SIZE; i++)
                this.gameBoard[i] = this.sum(this.gameBoard[i], false);

            this.Transpose();
            this.addNumber();
        }

        public void left()
        {
            this.saveBoardState();
            for (int i = 0; i < this.BOARD_SIZE; i++)
                this.gameBoard[i] = this.sum(this.gameBoard[i], false);
            
            this.addNumber();
        }

        public void right()
        {
            this.saveBoardState();
            for (int i = 0; i < this.BOARD_SIZE; i++)
                this.gameBoard[i] = this.sum(this.gameBoard[i], true);

            this.addNumber();
        }

        public bool shoudlAddNumber()
        { 
            for (int i = 0; i < this.BOARD_SIZE; i++)
                for (int j = 0; j < this.BOARD_SIZE; j++)
                    if (this.previousGameBoard[i][j] != this.gameBoard[i][j])
                        return true;
            
            return false;
        }

        public int getTileFontSize()
        {
            switch(this.BOARD_SIZE)
            {
                case 5:
                    return 25;
                case 6:
                    return 18;
                case 7:
                    return 20;
                case 8:
                    return 15;
                default:
                    return 25;
            }
        }

        public void UpdateBoard()
        {
            this.grid.Children.Clear();
            for (int i = 0; i < this.BOARD_SIZE; i++)
                for (int j = 0; j < this.BOARD_SIZE; j++)
                {
                    var btn = new Button
                    {
                        Text = this.gameBoard[i][j] == 0 ? "" : this.gameBoard[i][j].ToString(),
                        FontSize = getTileFontSize(),
                        BackgroundColor = Color.FromHex(numberToColor.ContainsKey(this.gameBoard[i][j]) ? numberToColor[this.gameBoard[i][j]] : numberToColor[2048])
                    };
                    btn.SetValue(Grid.RowProperty, i);
                    btn.SetValue(Grid.ColumnProperty, j);
                    this.grid.Children.Add(btn);
                }
        }

        async public void newGameClicked(object sender, EventArgs args)
        {
            var result = await chooseBoardSize();
            var didUserChooseGameSize = false;
            switch (result)
            {
                case "3x3":
                    this.BOARD_SIZE = 3;
                    didUserChooseGameSize = true;
                    break;
                case "4x4":
                    this.BOARD_SIZE = 4;
                    didUserChooseGameSize = true;
                    break;
                case "5x5":
                    this.BOARD_SIZE = 5;
                    didUserChooseGameSize = true;
                    break;
                case "6x6":
                    this.BOARD_SIZE = 6;
                    didUserChooseGameSize = true;
                    break;
                case "7x7":
                    this.BOARD_SIZE = 7;
                    didUserChooseGameSize = true;
                    break;
                case "8x8":
                    this.BOARD_SIZE = 8;
                    didUserChooseGameSize = true;
                    break;
                default:
                    Console.WriteLine("Default case");
                    break;
            }
            if (didUserChooseGameSize)
                this.startNewGame();
        }
   
        async public Task<string> chooseBoardSize()
        {
            var choosedBoardSize = await DisplayActionSheet("Choose board size", null, null, "3x3", "4x4", "5x5", "6x6", "7x7", "8x8");
            return choosedBoardSize;
        }

        public void updateGrid ()
        {
            mainLayout.Children.Clear();

            var grid = new Grid();
            for (int i = 0; i < this.BOARD_SIZE; i++)
                grid.RowDefinitions.Add(new RowDefinition { Height = APP_WIDTH / this.BOARD_SIZE });

            for (int i = 0; i < this.BOARD_SIZE; i++)
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            mainLayout.Children.Add(grid);
            this.grid = grid;
        }

        public MainPage()
        {
            InitializeComponent();

            this.updateGrid();
            SwipeListener swipeListener = new SwipeListener(overlay, this);
            this.gameBoard = this.generateBoard();
            this.previousGameBoard = this.generateBoard();
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
        }
	}
}