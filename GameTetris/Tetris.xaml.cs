using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace GameTetris
{
    /// <summary>
    /// Interaction logic for Tetris.xaml
    /// </summary>
    public partial class Tetris : Window
    {
        private readonly ImageSource[] tileImages = new ImageSource[]
        {
            new BitmapImage(new Uri("Assets/TileEmpty.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileCyan.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileBlue.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileOrange.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileYellow.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileGreen.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TilePurple.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileRed.png", UriKind.Relative))
        };

        private readonly ImageSource[] blockImages = new ImageSource[]
        {
            new BitmapImage(new Uri("Assets/Block-Empty.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-I.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-J.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-L.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-O.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-S.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-T.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-Z.png", UriKind.Relative))
        };

        private readonly Image[,] imageControls;
        private readonly int maxDelay = 1000;
        private readonly int minDelay = 75;
        private readonly int delayDecrease = 25;
        private GameState gameState = new GameState();
        private bool flag_pause = false;
        private int finalScore = 0;

        public Tetris()
        {
            InitializeComponent();
            imageControls = SetupGameCanvas(gameState.GameGrid);
            this.WindowStyle = WindowStyle.None;
        }

        private Image[,] SetupGameCanvas(GameGrid grid)
        {
            Image[,] imageControls = new Image[grid.Rows, grid.Columns];
            int cellSize = 25;

            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    Image imageControl = new Image
                    {
                        Width = cellSize,
                        Height = cellSize
                    };

                    Canvas.SetTop(imageControl, (r - 2) * cellSize + 10);
                    Canvas.SetLeft(imageControl, c * cellSize);
                    GameCanvas.Children.Add(imageControl);
                    imageControls[r, c] = imageControl;
                }
            }
            return imageControls;
        }

        private void DrawGrid(GameGrid grid)
        {
            for (int r = 0; r < grid.Rows; r++)
            {
                for (int c = 0; c < grid.Columns; c++)
                {
                    int id = grid[r, c];
                    imageControls[r, c].Opacity = 1;
                    imageControls[r, c].Source = tileImages[id];
                }
            }
        }

        private void DrawBlock(Block block)
        {
            foreach (Position p in block.TilePositions())
            {
                imageControls[p.Row, p.Column].Opacity = 1;
                imageControls[p.Row, p.Column].Source = tileImages[block.Id];
            }
        }

        private void DrawNextBlock(BlockQueue blockQueue)
        {
            Block next = blockQueue.NextBlock;
            NextImage.Source = blockImages[next.Id];
        }

        private void DrawHeldBlock(Block heldBlock)
        {
            if (heldBlock == null)
            {
                HoldImage.Source = blockImages[0];
            }
            else
            {
                HoldImage.Source = blockImages[heldBlock.Id];
            }
        }

        private void DrawGhostBlock(Block block)
        {
            int dropDistance = gameState.BlockDropDistance();
            foreach (Position p in block.TilePositions())
            {
                imageControls[p.Row + dropDistance, p.Column].Opacity = 0.25;
                imageControls[p.Row + dropDistance, p.Column].Source = tileImages[block.Id];
            }
        }

        private void Draw(GameState gameState)
        {
            DrawGrid(gameState.GameGrid);
            DrawGhostBlock(gameState.CurrentBlock);
            DrawBlock(gameState.CurrentBlock);
            DrawNextBlock(gameState.BlockQueue);
            DrawHeldBlock(gameState.HeldBlock);
            //ScoreText.Text = $"Score: {gameState.Score}";
        } 

        private async Task GameLoop()
        {
            Draw(gameState);
            //this.WindowStyle = WindowStyle.None;
            while (!gameState.GameOver)
            {
                int delay = Math.Max(minDelay, maxDelay - (gameState.Score * delayDecrease));
                await Task.Delay(500);
                if (flag_pause == true)
                {
                    ScoreText.Text = $"Игра на паузе";
                    gameState.StopBlock();
                }
                else
                {
                    ScoreText.Text = $"Счет: {gameState.Score}";
                    gameState.MoveBlockDown();
                    Draw(gameState);
                }
            }
            //this.WindowStyle = WindowStyle.SingleBorderWindow;
            finalScore = gameState.Score;
            string path = @"C:\Users\USer\source\repos\GameTetris";
            using (StreamWriter w = new StreamWriter(path, true, Encoding.GetEncoding(1251)))
            {
                 w.WriteLine(finalScore);
            }
            GameOverMenu.Visibility = Visibility.Visible;
            FinalScoreText.Text = $"Счет: {gameState.Score}";
            Quit_Tetris.Visibility = Visibility.Visible;

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (gameState.GameOver)
            {
                return;
            }

            switch (e.Key)
            {
                case Key.Left:
                    if (!flag_pause)
                        gameState.MoveBlockLeft();
                    break;
                case Key.Right:
                    if (!flag_pause)
                        gameState.MoveBlockRight();
                    break;
                case Key.Down:
                    if (!flag_pause)
                        gameState.MoveBlockDown();
                    break;
                case Key.Up:
                    if (!flag_pause)
                        gameState.RotateBlockCW();
                    break;
                case Key.Z:
                    if (!flag_pause)
                        gameState.RotateBlockCCW();
                    break;
                case Key.C:
                    if (!flag_pause)
                        gameState.HoldBlock();
                    break;
                case Key.Space:
                    if (!flag_pause)
                        gameState.DropBlock();
                    break;
                case Key.P:
                    flag_pause = true;
                    break;
                case Key.U:
                    flag_pause = false;
                    break;
                default:
                    return;
            }
            Draw(gameState);
            return;
        }

        private async void GameCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            await GameLoop();
        }

        private void btn_QuitTetrisClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

