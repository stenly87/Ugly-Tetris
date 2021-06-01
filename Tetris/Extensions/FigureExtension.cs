using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Tetris
{
    public static class FigureExtension
    {
        public static Geometry GetCurrentGeometry(this Figure figure)
        {
            return ApplyTransform(figure, figure.Shape.RenderedGeometry.Clone());
        }
        public static Geometry GetCurrentIntersectGeometry(this Figure figure, Point shift)
        {
            return ApplyTransform(figure, figure.IntersectLine.Clone(), shift);
        }
        static Geometry ApplyTransform(Figure figure, Geometry currentGeometry, Point shift = new Point())
        {
            currentGeometry.Transform = new TransformGroup
            {
                Children = new TransformCollection(new Transform[] {
                new RotateTransform(figure.Angle, 25, 25),
                new TranslateTransform(figure.CanvasPoint.X + figure.AngleCheckX() + shift.X,
                    figure.CanvasPoint.Y + figure.AngleCheckY() + shift.Y)
            })
            };
            return currentGeometry;
        }

    }
}
