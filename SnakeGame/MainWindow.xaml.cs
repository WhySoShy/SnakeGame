﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Controls;
using SnakeGame.Models;
namespace SnakeGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Options
        const int snakeSquareSize = 30;
        const int snakeRadius = 2;

        const int snakeStartLength = 3;
        const int snakeStartSpeed = 150;
        const int snakeSpeedThreshold = 100;

        private int snakeLength = 3;
        private int currentScore = 0;
        private bool gameState = true;

        private DispatcherTimer timer = new DispatcherTimer();
        #endregion
        #region Basic properties
        private Random rnd = new();
        #endregion
        #region Apple
        private UIElement snakeFood = null;
        private SolidColorBrush foodColor = Brushes.Red;
        #endregion
        #region SnakePart
        private SolidColorBrush snakeBodyBrush = Brushes.Black;
        private SolidColorBrush snakeHeadBrush = Brushes.HotPink;
        private List<SnakePart> snakeParts = new List<SnakePart>();

        private enum SnakeDirection { Left, Right, Up, Down };
        private SnakeDirection snakeDirection = SnakeDirection.Right;
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            timer.Tick += Timer;
            for (int i = 0; i < 10; i++)
                DrawSnakeFood();
        }
        private void Timer(Object sender, EventArgs e)
        {
            MoveSnake();
            Score_Label.Content = $"Score: {currentScore}";
            Speed_Label.Content = $"Speed: {Math.Max(snakeSpeedThreshold, (int)timer.Interval.TotalMilliseconds - (currentScore * 2))}";
        }
        private void Window_RenderedContent(Object sender, EventArgs e)
        {
            DrawGame();
            StartNewGame();
        }
        private void Window_KeyUp(Object sender, KeyEventArgs e)
        {
            SnakeDirection _ = snakeDirection;
            switch(e.Key)
            {
                case Key.Up or Key.W:
                    if (snakeDirection != SnakeDirection.Down)
                        snakeDirection = SnakeDirection.Up;
                    break;
                case Key.Down or Key.S:
                    if (snakeDirection != SnakeDirection.Up)
                        snakeDirection = SnakeDirection.Down;
                    break;
                case Key.Left or Key.A:
                    if (snakeDirection != SnakeDirection.Right)
                        snakeDirection = SnakeDirection.Left;
                    break;
                case Key.Right or Key.D:
                    if (snakeDirection != SnakeDirection.Left)
                        snakeDirection = SnakeDirection.Right;
                    break;
            }
            if (snakeDirection != _)
                MoveSnake();   
        }
        private void StartNewGame()
        {
            snakeLength = snakeStartLength;
            snakeDirection = SnakeDirection.Right;
            snakeParts.Add(new SnakePart() { point = new Point(snakeSquareSize * 5, snakeSquareSize * 5) });
            timer.Interval = TimeSpan.FromMilliseconds(snakeStartSpeed);

            DrawSnake();
            DrawSnakeFood();

            timer.IsEnabled = true;
            

        }
        private void MoveSnake()
        {
            if (!gameState)
                return;

            while(snakeParts.Count() >= snakeLength)
            {
                GameArea.Children.Remove(snakeParts[0].Element);
                snakeParts.RemoveAt(0);
            }
            foreach(SnakePart part in snakeParts)
            {
                (part.Element as Rectangle).Fill = snakeBodyBrush;
                part.IsHead = false;
            }
            SnakePart head = snakeParts[snakeParts.Count - 1];
            double nextX = head.point.X, nextY = head.point.Y;
            switch(snakeDirection)
            {
                case SnakeDirection.Left:
                    nextX -= snakeSquareSize;
                    break;
                case SnakeDirection.Right:
                    nextX += snakeSquareSize;
                    break;
                case SnakeDirection.Up:
                    nextY -= snakeSquareSize;
                    break;
                case SnakeDirection.Down:
                    nextY += snakeSquareSize;
                    break;
            }
            snakeParts.Add(new SnakePart()
            {
                point = new Point(nextX, nextY),
                IsHead = true
            });
            DrawSnake();
            DoCollisionCheck();
        }
        private void DoCollisionCheck()
        {
            SnakePart head = snakeParts[snakeParts.Count-1];
            if (head.point.X == Canvas.GetLeft(snakeFood) && head.point.Y == Canvas.GetTop(snakeFood))
            {
                EatSnakeFood();
                return;
            }

            if (head.point.X < 0 || head.point.X >= GameArea.ActualWidth ||
                head.point.Y < 0 || head.point.Y >= GameArea.ActualHeight)
            {
                gameState = false;
                EndGame();
            }

            foreach (SnakePart part in snakeParts.Take(snakeParts.Count - 1))
                if (head.point.X == part.point.X && head.point.Y == part.point.Y)
                {
                    gameState = false;
                    EndGame();
                }
        }
        private void EatSnakeFood()
        {
            snakeLength++;
            currentScore++;
            // Sætter hastigheden op
            int timerInterval = Math.Max(snakeSpeedThreshold, (int)timer.Interval.TotalMilliseconds - (currentScore * 2));
            timer.Interval = TimeSpan.FromMilliseconds(timerInterval);
            GameArea.Children.Remove(snakeFood);
            DrawSnakeFood();
            UpdateGameStatus();
        }
        private void UpdateGameStatus()
        {
            this.Title = $"Snake score: {currentScore}";
        }
        private void DrawSnake()
        {
            foreach(SnakePart part in snakeParts)
            {
                if (part.Element == null)
                {
                    part.Element = new Rectangle()
                    {
                        Width = snakeSquareSize,
                        Height = snakeSquareSize,
                        RadiusX = snakeRadius,
                        RadiusY = snakeRadius,
                        Fill = (part.IsHead ? snakeHeadBrush : snakeBodyBrush)
                    };
                    GameArea.Children.Add(part.Element);
                    Canvas.SetTop(part.Element, part.point.Y);
                    Canvas.SetLeft(part.Element, part.point.X);
                }
            }
        }
        private void DrawGame()
        {
            int nextX = 0, nextY = 0, rowCounter = 0;
            bool isOdd = false;
            Debug.WriteLine(GameArea.ActualHeight);
            while (nextY <= GameArea.ActualHeight)
            {
                Rectangle rectangle = new Rectangle()
                {
                    Width = snakeSquareSize,
                    Height = snakeSquareSize,
                    Fill = isOdd ? Brushes.Green : Brushes.LawnGreen
                };

                isOdd = !isOdd;
                nextX += snakeSquareSize;

                GameArea.Children.Add(rectangle);
                Canvas.SetTop(rectangle, nextY);
                Canvas.SetLeft(rectangle, nextX - snakeSquareSize);

                if (nextX >= GameArea.ActualWidth)
                {
                    nextX = 0;
                    nextY += snakeSquareSize;
                    rowCounter++;
                    isOdd = (rowCounter % 2 != 0);
                }
            }
        }

        private Point GetNextFoodPos()
        {
            int maxX = (int)(GameArea.ActualWidth / snakeSquareSize);
            int maxY = ((int)GameArea.ActualHeight / snakeSquareSize);
            int foodX = rnd.Next(0, maxX) * snakeSquareSize;
            int foodY = rnd.Next(0, maxY) * snakeSquareSize;

            foreach (SnakePart part in snakeParts)
                if ((part.point.X == foodX) && (part.point.Y == foodY))
                    return GetNextFoodPos();

            return new Point(foodX, foodY);
        }
        private void DrawSnakeFood()
        {
            Point foodPosition = GetNextFoodPos();
            snakeFood = new Ellipse()
            {
                Width = snakeSquareSize,
                Height = snakeSquareSize,
                Fill = foodColor
            };
            GameArea.Children.Add(snakeFood);
            Canvas.SetTop(snakeFood, foodPosition.Y);
            Canvas.SetLeft(snakeFood, foodPosition.X);
        }
        private void EndGame()
        {
            //Debug.WriteLine($"X: {snakeParts[0].point.X} \n Y: {snakeParts[0].point.Y}");
            timer.IsEnabled = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Menu menu = new();
            menu.Show();
            this.Close();
        }
    }
}
