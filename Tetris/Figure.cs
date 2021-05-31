using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Tetris
{
    public abstract class Figure
    {
        public CustomPoint CanvasPoint { get; set; } = new CustomPoint();
        protected Shape shape;
        protected Geometry line;
        private int angle;

        public virtual int Angle
        {
            get => angle;
            set
            {
                if (value == 360)
                    value = 0;
                angle = value;
            }
        }

        public Geometry GetIntersectLine() => line;

        public Shape Shape { get => shape; }
        public int Height { get; set; }
        public int Width { get; set; }
        public abstract int GetCurrentWidth();
        public abstract int GetCurrentHeight();

        public abstract int AngleCheckX();
        public abstract int AngleCheckY();
        
        public Geometry GetCurrentGeometry()
        {
            var currentGeometry = Shape.RenderedGeometry.Clone();
            ApplyCurrentTransform(currentGeometry);
            return currentGeometry;
        }
        public Geometry GetCurrentIntersectGeometry(Point point)
        {
            var currentGeometry = line.Clone();
            currentGeometry.Transform = new TransformGroup
            {
                Children = new TransformCollection(new Transform[] {
                new RotateTransform(Angle, 25, 25),
                new TranslateTransform(CanvasPoint.X + AngleCheckX() + point.X,
                    CanvasPoint.Y + AngleCheckY() + point.Y)
            })
            };
            return currentGeometry;
        }
        private void ApplyCurrentTransform(Geometry currentGeometry)
        {
            currentGeometry.Transform = new TransformGroup
            {
                Children = new TransformCollection(new Transform[] {
                    new RotateTransform(Angle, 25, 25),
                    new TranslateTransform(CanvasPoint.X + AngleCheckX(),
                        CanvasPoint.Y + AngleCheckY())
                })
            };
        }

    }
}