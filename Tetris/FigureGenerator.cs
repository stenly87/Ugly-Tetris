using System;

namespace Tetris
{
    internal class FigureGenerator
    {
        static Random rnd = new Random();
        internal static Figure GetNext()
        {
            if (false/*rnd.Next(0, 100) > 50*/)
                return new Triangle();
            else
                return new Line();
        }
    }
}