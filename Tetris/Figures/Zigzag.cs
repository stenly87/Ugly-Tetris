using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Tetris
{
    public class Zigzag : Figure
    {
        public Zigzag() : base(75, 50, new PointCollection(new Point[] {
            new Point(0, 0),
                new Point(0, 25),
                new Point(25, 25),
                new Point(25, 50),
                new Point(75, 50),
                new Point(75, 25),
                new Point(50, 25),
                new Point(50, 0)
        }))
        {
            line = new GeometryGroup
            {
                Children = new GeometryCollection(new[] {
                new LineGeometry(new Point(15,15), new Point(45, 15)),
                new LineGeometry(new Point(45, 15), new Point(45, 45)),
                new LineGeometry(new Point(45, 45), new Point(65, 45))
            })
            };
        }

        public override int Angle
        {
            get => base.Angle;
            set => base.Angle = value == 90 ? value : 0;
        }

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
            if (Angle == 0)
                return 50;
            return 75;
        }

        public override int GetCurrentWidth()
        {
            if (Angle == 0)
                return 75;
            return 50;
        }
    }
}
