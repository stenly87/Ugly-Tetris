using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;
namespace Tetris
{
    public class GShape : Figure
    {
        public GShape(): base(75,50, new PointCollection(new Point[] {
                new Point(0, 25),
                new Point(0, 50),
                new Point(75, 50),
                new Point(75, 0),
                new Point(50, 0),
                new Point(50, 25)
            }))
        {
            line = new GeometryGroup
            {
                Children = new GeometryCollection(new[] {
                new LineGeometry(new Point(15,45), new Point(65, 45)),
                new LineGeometry(new Point(65,45), new Point(65,15))
            })
            };
        }
        public override int AngleCheckX()
        {
            if (Angle == 180)
                return 25;
            return 0;
        }

        public override int AngleCheckY()
        {
            if (Angle == 270)
                return 25;
            return 0;
        }

        public override int GetCurrentHeight()
        {
            switch (Angle)
            {
                case 90:
                case 270:
                    return 75;
                default:
                    return 50;
            }
        }

        public override int GetCurrentWidth()
        {
            switch (Angle)
            {
                case 90:
                case 270:
                    return 50;
                default:
                    return 75;
            }
        }
    }
}
