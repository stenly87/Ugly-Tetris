using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Tetris
{
    public class FigureGenerator
    {
        static Random rnd = new Random();
        private readonly IBrushRandomizer brushRandomizer;
        private readonly Canvas showNextFigureCanvas;
        List<Figure> figures = new List<Figure>();
        int nextIndex = 0;

        public FigureGenerator(IBrushRandomizer brushRandomizer, Canvas showNextFigureCanvas)
        {
            this.brushRandomizer = brushRandomizer;
            this.showNextFigureCanvas = showNextFigureCanvas;
            figures.Add(new Triangle());
            figures.Add(new Line());
            figures.Add(new Square());
            figures.Add(new Zigzag());
            figures.Add(new GShape());
            GenerateNextIndex();
        }

        private void GenerateNextIndex()
        {
            nextIndex = rnd.Next(0, figures.Count);
            ShowNext();
        }

        void ShowNext()
        {
            var nextFigure = figures[nextIndex];
            nextFigure.CanvasPoint = new CustomPoint();
            nextFigure.Fill = brushRandomizer.GetRandomBrush();
            showNextFigureCanvas.Children.Clear();
            showNextFigureCanvas.Children.Add(new Polygon { Points = nextFigure.BaseGeometryPoints, Fill = nextFigure.Fill });
            Canvas.SetTop(nextFigure.Shape, 0);
            Canvas.SetLeft(nextFigure.Shape, 0);
        }

        internal Figure GetNext()
        {
            var nextFigure = figures[nextIndex];
            nextFigure.CanvasPoint = new CustomPoint();
            GenerateNextIndex();
            return nextFigure;
        }
    }
}