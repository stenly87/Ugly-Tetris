using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Tetris
{
    internal class FigureProcess
    {
        private readonly int fieldWidth;
        private readonly int fieldHeight;
        private GameTimer gameTimer;
        private Canvas mainCanvas;
        private Geometry geometryGroup;
        private Path parentGeometryGroup;
        private readonly LineGeometry checkLine;
        IBrushRandomizer brushRandomizer;
        FigureGenerator generator;

        public event EventHandler GameFail;

        Figure CurrentFigure { get; set; }
        int step = 0;
        public FigureProcess(GameTimer gameTimer, Canvas mainCanvas, Path parentGeometryGroup, LineGeometry checkLine)
        {
            this.gameTimer = gameTimer;
            fieldWidth = (int)mainCanvas.Width;
            fieldHeight = (int)mainCanvas.Height;
            gameTimer.TickEvent += GameTimer_TickEvent;
            this.mainCanvas = mainCanvas;
            this.parentGeometryGroup = parentGeometryGroup;
            this.geometryGroup = parentGeometryGroup.Data;
            this.checkLine = checkLine;
            brushRandomizer = new BrushRandomizer();
            generator = new FigureGenerator(brushRandomizer);
            CreateNewFigure();
        }

        private void GameTimer_TickEvent(object sender, System.EventArgs e)
        {
            if (TestIntersect(new Point(0, 25)) || TestHeight(CurrentFigure))
            {
                if (step == 0)
                    GameFail?.Invoke(this, null);
                StopCurrentFigure();
            }
            else
            {
                step++;
                CurrentFigure.CanvasPoint.Y += 25;
                Canvas.SetTop(CurrentFigure.Shape, CurrentFigure.CanvasPoint.Y);
            }
        }

        private void CreateNewFigure()
        {
            step = 0;
            mainCanvas.Children.Remove(CurrentFigure?.Shape);
            CurrentFigure = generator.GetNext();
            mainCanvas.Children.Add(CurrentFigure.Shape);
            CurrentFigure.CanvasPoint.X = 125;
            Canvas.SetLeft(CurrentFigure.Shape, CurrentFigure.CanvasPoint.X);
        }

        internal void TryMoveDown()
        {
            GameTimer_TickEvent(this, null);
        }

        internal void TryMoveRight()
        {
            if (!TestIntersect(new Point(25, 0)))
            {
                if (CurrentFigure.GetCurrentWidth() + CurrentFigure.CanvasPoint.X >= fieldWidth)
                    CurrentFigure.CanvasPoint.X = fieldWidth - CurrentFigure.GetCurrentWidth();
                else
                    CurrentFigure.CanvasPoint.X += 25;
                Canvas.SetLeft(CurrentFigure.Shape, CurrentFigure.CanvasPoint.X);
            }
        }

        internal void TryMoveLeft()
        {
            if (!TestIntersect(new Point(-25, 0)))
            {
                CurrentFigure.CanvasPoint.X -= 25;
                Canvas.SetLeft(CurrentFigure.Shape, CurrentFigure.CanvasPoint.X);
            }
        }

        internal void TurnFigure()
        {
            CurrentFigure.Angle += 90;
            if (TestIntersect(new Point(0, 0)) || TestHeight(CurrentFigure))
            {
                CurrentFigure.Angle -= 90;
                return;
            }
            CurrentFigure.Shape.LayoutTransform = new RotateTransform(CurrentFigure.Angle, 25, 25);
            if (CurrentFigure.GetCurrentWidth() + CurrentFigure.CanvasPoint.X >= fieldWidth)
                CurrentFigure.CanvasPoint.X = fieldWidth - CurrentFigure.GetCurrentWidth();
            Canvas.SetLeft(CurrentFigure.Shape, CurrentFigure.CanvasPoint.X);
        }

        private bool TestIntersect(Point point)
        {
            if (geometryGroup.FillContainsWithDetail(
                CurrentFigure.GetCurrentIntersectGeometry(point)) != IntersectionDetail.Empty)
                return true;
            return false;
        }

        private void StopCurrentFigure()
        {
            geometryGroup = Geometry.Combine(geometryGroup, CurrentFigure.GetCurrentGeometry(), GeometryCombineMode.Union, null);
            var checkline = checkLine.Clone();
            for (int i = 0; i < fieldHeight; i += 25)
            {
                checkline.Transform = new TranslateTransform(0, i);
                if (geometryGroup.FillContains(checkline))
                {
                    var up = Geometry.Combine(geometryGroup, new RectangleGeometry(new Rect(0, 0, fieldWidth, i)), GeometryCombineMode.Intersect, null);
                    var down = Geometry.Combine(geometryGroup, new RectangleGeometry(new Rect(0, i + 25, fieldWidth, fieldHeight - i)), GeometryCombineMode.Intersect, null);
                    up.Transform = new TranslateTransform(0, 25);
                    geometryGroup = Geometry.Combine(up, down, GeometryCombineMode.Union, null);
                }
            }
            parentGeometryGroup.Data = geometryGroup;
            CreateNewFigure();
        }

        private bool TestHeight(Figure currentFigure)
        {
            if (currentFigure.CanvasPoint.Y + currentFigure.GetCurrentHeight() + 25 > fieldHeight)
                return true;
            return false;
        }
    }
}