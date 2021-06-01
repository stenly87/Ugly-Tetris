using System;

namespace Tetris
{
    internal class FigureGenerator
    {
        static Random rnd = new Random();
        private readonly IBrushRandomizer brushRandomizer;

        public FigureGenerator(IBrushRandomizer brushRandomizer)
        {
            this.brushRandomizer = brushRandomizer;
        }
        internal Figure GetNext()
        {
            int value = rnd.Next(0, 4);
            switch (value)
            {
                case 0: return new Triangle(brushRandomizer);
                case 1: return new Line(brushRandomizer);
                case 2: return new Square(brushRandomizer);
                case 3: return new Zigzag(brushRandomizer);
                default: return new GShape(brushRandomizer);
            }
        }
    }
}