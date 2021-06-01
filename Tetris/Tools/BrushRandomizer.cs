using System;
using System.Windows.Media;

namespace Tetris
{
    public class BrushRandomizer : IBrushRandomizer
    {
        static Random rnd = new Random();
        public Brush GetRandomBrush()
        {
            SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(255,
                (byte)rnd.Next(0, 255),
                (byte)rnd.Next(0, 255),
                (byte)rnd.Next(0, 255)));
            return brush;
        }
    }
}