using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Tetris
{
    internal class GameFieldGeometry
    {
        private Canvas mainCanvas;
        readonly LineGeometry checkLine;
        Geometry geometryGroup;
        Path parentGeometryGroup;

        public GameFieldGeometry(Canvas mainCanvas)
        {
            this.mainCanvas = mainCanvas;
            mainCanvas.Children.Clear();
            checkLine = new LineGeometry
            {
                StartPoint = new Point(10, 10),
                EndPoint = new Point(mainCanvas.Width - 20, 10)
            };
            geometryGroup = new PathGeometry();
            parentGeometryGroup = new Path { Data = geometryGroup };
            var brush = new LinearGradientBrush
            {
                GradientStops = new GradientStopCollection(new GradientStop[] {
                    new GradientStop(Colors.Red, 0),
                    new GradientStop(Colors.Green, 0.5),
                    new GradientStop(Colors.Blue, 1)
                })
            };
            parentGeometryGroup.Fill = brush;
            parentGeometryGroup.Height = mainCanvas.Height;
            parentGeometryGroup.Width = mainCanvas.Width;
            mainCanvas.Children.Add(parentGeometryGroup);
        }

        internal bool TestIntersect(Figure currentFigure, Point point)
        {
            if (geometryGroup.FillContainsWithDetail(
                 currentFigure.GetCurrentIntersectGeometry(point)) != IntersectionDetail.Empty)
                return true;
            return false;
        }

        internal void AddGeometry(Figure currentFigure)
        {
            geometryGroup = Geometry.Combine(geometryGroup, currentFigure.GetCurrentGeometry(), GeometryCombineMode.Union, null);
            parentGeometryGroup.Data = geometryGroup;
        }

        internal int RemoveFullLines()
        {
            int count = 0;
            int fieldHeight = (int)mainCanvas.Height;
            int fieldWidth = (int)mainCanvas.Width;
            var checkline = checkLine.Clone();
            for (int i = 0; i < fieldHeight; i += 25)
            {
                checkline.Transform = new TranslateTransform(0, i);
                if (geometryGroup.FillContains(checkline))
                {
                    count++;
                    var up = Geometry.Combine(geometryGroup, new RectangleGeometry(new Rect(0, 0, fieldWidth, i)), GeometryCombineMode.Intersect, null);
                    var down = Geometry.Combine(geometryGroup, new RectangleGeometry(new Rect(0, i + 25, fieldWidth, fieldHeight - i)), GeometryCombineMode.Intersect, null);
                    up.Transform = new TranslateTransform(0, 25);
                    geometryGroup = Geometry.Combine(up, down, GeometryCombineMode.Union, null);
                }
            }
            parentGeometryGroup.Data = geometryGroup;
            return count;
        }
    }
}