using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Tetris
{
    public class Square : Figure
    {
        public Square(): base(50,50, new PointCollection(new Point[] {
            new Point(0, 0),
            new Point(0, 50),
            new Point(50, 50),
            new Point(50, 0)
        }))
        {
            line = new GeometryGroup
            {
                Children = new GeometryCollection(new[] {
                new LineGeometry(new Point(15,15), new Point(15, 45)),
                new LineGeometry(new Point(15,45), new Point(45, 45)),
                new LineGeometry(new Point(45,45), new Point(45, 15))
            })
            };
        }

        public override int Angle { get => 0; set { } }
        public override int AngleCheckX()
        {
            return 0;
        }

        public override int AngleCheckY()
        {
            return 0;
        }

        public override int GetCurrentHeight()
        {
            return 50;
        }

        public override int GetCurrentWidth()
        {
            return 50;
        }
    }
}
