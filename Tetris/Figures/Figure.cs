using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Tetris
{
    public abstract class Figure 
    {
        public Figure(int Width, int Height, PointCollection baseGeometryPoints)
        {
            this.Width = Width;
            this.Height = Height;
            BaseGeometryPoints = baseGeometryPoints;

            shape = new Polygon { 
                Points = baseGeometryPoints, 
                Width = Width, 
                Height = Height };
        }
        public PointCollection BaseGeometryPoints { get; set; }
        public CustomPoint CanvasPoint { get; set; } = new CustomPoint();

        protected Geometry line;

        public Brush Fill { get => shape.Fill; set => shape.Fill = value; }
        public Geometry IntersectLine { get => line; }
        protected Shape shape;
        public Shape Shape { get => shape; }

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
        public int Height { get; set; }
        public int Width { get; set; }
        public abstract int GetCurrentWidth();
        public abstract int GetCurrentHeight();
        public abstract int AngleCheckX();
        public abstract int AngleCheckY();
        
    }
}