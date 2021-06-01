using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Tetris
{
    public class Line : Figure
    {
        public Line(): base(100,25, new PointCollection (new Point[] { 
            new Point(0, 0),
            new Point(0, 25),
            new Point(100, 25),
            new Point(100, 0)
        }))
        {
            line = new LineGeometry
            {
                StartPoint = new Point(10, 10),
                EndPoint = new Point(90, 10)
            };
        }

        public override int Angle
        { 
            get => base.Angle; 
            set => base.Angle = value == 90 ? value : 0;
        }

        public override int AngleCheckX()
        {
            if (Angle == 90)
                return -25;
            return 0;
        }

        public override int AngleCheckY()
        {
            return 0;
        }

        public override int GetCurrentHeight()
        {
            if (Angle == 90)
                return 100;
            return 25;
        }

        public override int GetCurrentWidth()
        {
            if (Angle == 90)
                return 25;
            return 100;
        }
    }
}
