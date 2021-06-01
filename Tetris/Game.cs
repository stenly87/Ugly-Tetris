using System;
using System.Collections.Generic;
using System.Media;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace Tetris
{
    public class Game
    {
        public int Scores { get; set; }
        public bool Running { get; set; } = false;

        private Canvas mainCanvas;
        private readonly int fieldWidth;
        private readonly int fieldHeight;


        PathGeometry geometryGroup;
        Path parentGeometryGroup;
        SoundPlayer pl = new SoundPlayer(Environment.CurrentDirectory + "/Music/аплодисменты.wav"); // .wav
        

        FigureProcess figureProcess;
        GameTimer gameTimer;

        LineGeometry checkLine;
        public Game(Canvas mainCanvas, int fieldWidth, int fieldHeight)
        {
            mainCanvas.Width = fieldWidth;
            mainCanvas.Height = fieldHeight;
            checkLine = new LineGeometry
            {
                StartPoint = new Point(10, 10),
                EndPoint = new Point(fieldWidth - 20, 10)
            };
            this.mainCanvas = mainCanvas;
            this.fieldWidth = fieldWidth;
            this.fieldHeight = fieldHeight;
            gameTimer = new GameTimer();
            Restart();
        }

        internal void KeyUp(Key key)
        {
            switch (key)
            {
                case Key.Up:
                    figureProcess.TurnFigure();
                    break;
                case Key.Left:
                    figureProcess.TryMoveLeft();
                    break;
                case Key.Right:
                    figureProcess.TryMoveRight();
                    break;
                case Key.Down:
                    figureProcess.TryMoveDown();                  
                    break;
                case Key.Space:
                    Running = !Running;
                    if (Running)
                        gameTimer.Play();
                    else
                        gameTimer.Pause();
                    break;
            }
        }

        private void Restart()
        {
            Scores = 0;
            InitCanvas();
            figureProcess = new FigureProcess(gameTimer, mainCanvas, parentGeometryGroup, checkLine);
            figureProcess.GameFail += FigureProcess_GameFail;
        }

        private void FigureProcess_GameFail(object sender, EventArgs e)
        {
            gameTimer.Pause();
            pl.Play();
            if (MessageBox.Show("Вы проиграли. Хотите еще раз?", "Игра окончена", MessageBoxButton.YesNo) ==
                 MessageBoxResult.Yes)
                Restart();
            else
                Application.Current.Shutdown();
        }

        private void InitCanvas()
        {
            mainCanvas.Children.Clear();
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
            parentGeometryGroup.Height = fieldHeight;
            parentGeometryGroup.Width = fieldWidth;
            mainCanvas.Children.Add(parentGeometryGroup);
        }

        
    }
}
