using System;
using System.Collections.Generic;
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
using SnakeGame.Models;

namespace SnakeGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Options
        const int snakeSquareSize = 20;
        const int snakeRadius = 2;
        #endregion
        #region SnakePart
        private SolidColorBrush snakeBodyBrush = Brushes.Green;
        private SolidColorBrush snakeHeadBrush = Brushes.YellowGreen;
        private List<SnakePart> snakeParts = new List<SnakePart>();
        #endregion

        public MainWindow()
        {
            InitializeComponent();
        }

        private async Task Window_RenderedContent(Object sender, EventArgs e)
        {
            await DrawGameAsync();

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
        private async Task DrawGameAsync()
        {
            int nextX = 0, nextY = 0, rowCounter = 0;
            bool isOdd = false;
            while (nextY <= GameArea.ActualHeight)
            {
                Rectangle rectangle = new Rectangle()
                {
                    Width = snakeSquareSize,
                    Height = snakeSquareSize,
                    Fill = isOdd ? Brushes.White : Brushes.Black
                };
                GameArea.Children.Add(rectangle);
                Canvas.SetTop(rectangle, nextY);
                Canvas.SetLeft(rectangle, nextX);

                isOdd = !isOdd;
                nextX += snakeSquareSize;

                if (nextX >= GameArea.ActualWidth)
                {
                    nextX = 0;
                    nextY += snakeSquareSize;
                    rowCounter++;
                    isOdd = (rowCounter % 2 != 0);
                }
            }
        }

    }
}
