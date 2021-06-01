using System;
using System.Collections.Generic;

namespace Tetris
{
    public class FigureGenerator
    {
        static Random rnd = new Random();
        private readonly IBrushRandomizer brushRandomizer;
        List<Figure> figures = new List<Figure>();
        int nextIndex = 0;

        public FigureGenerator(IBrushRandomizer brushRandomizer)
        {
            this.brushRandomizer = brushRandomizer;
            figures.Add(new Triangle());
            figures.Add(new Line());
            figures.Add(new Square());
            figures.Add(new Zigzag());
            figures.Add(new GShape());
            nextIndex = GenerateNextIndex();

        }

        private int GenerateNextIndex()
        {
            return rnd.Next(0, 5);
        }

        internal Figure GetNext()
        {
            var nextFigure = figures[nextIndex];
            nextFigure.CanvasPoint = new CustomPoint();
            nextFigure.Fill = brushRandomizer.GetRandomBrush();
            nextIndex = GenerateNextIndex();
            return nextFigure;
        }
    }
}