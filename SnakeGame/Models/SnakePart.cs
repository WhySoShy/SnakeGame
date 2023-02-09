using System;
using System.Windows;

namespace SnakeGame.Models
{
    public class SnakePart
    {
        public UIElement Element { get; set; }
        public Point point { get; set; }
        public bool IsHead { get; set; }
    }
}
