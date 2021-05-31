using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Tetris
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        //монет получился достойниый
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        Figure CurrentFigure { get; set; }

        PathGeometry GeometryGroup = new PathGeometry();
        Path parentGeometryGroup;
        public MainWindow()
        {
            InitializeComponent();
            

            parentGeometryGroup = new Path { Data = GeometryGroup };
            parentGeometryGroup.Fill = Brushes.Blue;
            parentGeometryGroup.Height = 500;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            mainCanvas.Children.Add(parentGeometryGroup);

            CreateNewFigure();
        }


        private void CreateNewFigure()
        {
            step = 0;
            mainCanvas.Children.Remove(CurrentFigure?.Shape);
            CurrentFigure = FigureGenerator.GetNext();
            mainCanvas.Children.Add(CurrentFigure.Shape);
            CurrentFigure.CanvasPoint.X = 125;
            Canvas.SetLeft(CurrentFigure.Shape, CurrentFigure.CanvasPoint.X);
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (TestIntersect(new Point(0, 25)) || TestHeight(CurrentFigure))
            {
                if (step == 0)
                {
                    dispatcherTimer.Stop();
                    MessageBox.Show("Игра окончена");
                }
                StopCurrentFigure();
            }
            else
            {
                step++;
                CurrentFigure.CanvasPoint.Y += 25;
                Canvas.SetTop(CurrentFigure.Shape, CurrentFigure.CanvasPoint.Y);
            }
        }

        private bool TestIntersect(Point point)
        {
            if (GeometryGroup.FillContainsWithDetail(
                CurrentFigure.GetCurrentIntersectGeometry(point)) != IntersectionDetail.Empty)
                return true;
            return false;
        }

        int step = 0;
        LineGeometry checkLine = new LineGeometry { StartPoint = new Point(10, 10), EndPoint = new Point(280, 10) };
        private void StopCurrentFigure()
        {
            GeometryGroup = Geometry.Combine(GeometryGroup, CurrentFigure.GetCurrentGeometry(), GeometryCombineMode.Union, null);
            var checkline = checkLine.Clone();
            for (int i = 0; i < 500; i += 25)
            {
                checkline.Transform = new TranslateTransform(0, i);
                if (GeometryGroup.FillContains(checkline))
                {
                    var up = Geometry.Combine(GeometryGroup, new RectangleGeometry(new Rect(0, 0, 300, i)), GeometryCombineMode.Intersect, null);
                    var down = Geometry.Combine(GeometryGroup, new RectangleGeometry(new Rect(0, i + 25, 300, 500-i)), GeometryCombineMode.Intersect, null);
                    up.Transform = new TranslateTransform(0, 25);
                    GeometryGroup = Geometry.Combine(up, down, GeometryCombineMode.Union, null);
                }
            }
            parentGeometryGroup.Data = GeometryGroup;
            CreateNewFigure();
        }

        private bool TestHeight(Figure currentFigure)
        {
            if (currentFigure.CanvasPoint.Y + currentFigure.GetCurrentHeight() + 25 > 500)
                return true;
            return false;
        }

        private void keyUpMethod(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    CurrentFigure.Angle += 90;
                    if (TestIntersect(new Point(0, 0)) || TestHeight(CurrentFigure))
                    {
                        CurrentFigure.Angle -= 90;
                        break;
                    }
                    CurrentFigure.Shape.LayoutTransform = new RotateTransform(CurrentFigure.Angle, 25, 25);
                    if (CurrentFigure.GetCurrentWidth() + CurrentFigure.CanvasPoint.X >= 300)
                        CurrentFigure.CanvasPoint.X = 300 - CurrentFigure.GetCurrentWidth();
                    Canvas.SetLeft(CurrentFigure.Shape, CurrentFigure.CanvasPoint.X);
                    break;
                case Key.Left:
                    if (!TestIntersect(new Point(-25,0)))
                    {
                        CurrentFigure.CanvasPoint.X -= 25;
                        Canvas.SetLeft(CurrentFigure.Shape, CurrentFigure.CanvasPoint.X);
                    }
                    break;
                case Key.Right:
                    if (!TestIntersect(new Point(25, 0)))
                    {
                        if (CurrentFigure.GetCurrentWidth() + CurrentFigure.CanvasPoint.X >= 300)
                            CurrentFigure.CanvasPoint.X = 300 - CurrentFigure.GetCurrentWidth();
                        else
                            CurrentFigure.CanvasPoint.X += 25;
                        Canvas.SetLeft(CurrentFigure.Shape, CurrentFigure.CanvasPoint.X);
                    }
                    break;
                case Key.Down:
                    DispatcherTimer_Tick(this, new EventArgs());
                    break;
            }
        }

    }
}
