using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Tetris
{
    internal class GameProcess : IDisposable
    {
        readonly int fieldWidth;
        readonly int fieldHeight;
        readonly GameTimer gameTimer;
        readonly Canvas mainCanvas;
        readonly GameFieldGeometry gameFieldGeometry;
        readonly FigureGenerator generator;

        public event EventHandler GameFail;
        public event EventHandler<int> AddScore;

        Figure CurrentFigure { get; set; }
        int step = 0;
        public GameProcess(Canvas mainCanvas, FigureGenerator generator)
        {
            this.gameTimer = new GameTimer(1000);
            fieldWidth = (int)mainCanvas.Width;
            fieldHeight = (int)mainCanvas.Height;
            gameTimer.TickEvent += GameTimer_TickEvent;
            this.mainCanvas = mainCanvas;
            this.generator = generator;

            gameFieldGeometry = new GameFieldGeometry(mainCanvas);
        }

        internal void Stop()
        {
            SetGameStatus(false);
        }

        internal void SetGameStatus(bool running)
        {
            if (running)
                gameTimer.Play();
            else
                gameTimer.Pause();
        }

        internal void Start()
        {
            SetGameStatus(true);
            CreateNewFigure();
        }

        private void GameTimer_TickEvent(object sender, System.EventArgs e)
        {
            if (gameFieldGeometry.TestIntersect(CurrentFigure, new Point(0, 25)) || TestHeight(CurrentFigure))
            {
                if (step == 0)
                    GameFail?.Invoke(this, null);
                StopCurrentFigure();
            }
            else
            {
                step++;
                CurrentFigure.CanvasPoint.Y += 25;
                CanvasPositionUpdate();
            }
        }

        private void CreateNewFigure()
        {
            step = 0;
            mainCanvas.Children.Remove(CurrentFigure?.Shape);
            CurrentFigure = generator.GetNext();
            mainCanvas.Children.Add(CurrentFigure.Shape);
            CurrentFigure.CanvasPoint.X = 50;
            CanvasPositionUpdate();
        }

        void CanvasPositionUpdate()
        {
            Canvas.SetLeft(CurrentFigure.Shape, CurrentFigure.CanvasPoint.X);
            Canvas.SetTop(CurrentFigure.Shape, CurrentFigure.CanvasPoint.Y);
        }

        internal void TryMoveDown()
        {
            GameTimer_TickEvent(this, null);
        }

        internal void TryMoveRight()
        {
            if (!gameFieldGeometry.TestIntersect(CurrentFigure, new Point(25, 0)))
            {
                if (CurrentFigure.GetCurrentWidth() + CurrentFigure.CanvasPoint.X >= fieldWidth)
                    CurrentFigure.CanvasPoint.X = fieldWidth - CurrentFigure.GetCurrentWidth();
                else
                    CurrentFigure.CanvasPoint.X += 25;
                CanvasPositionUpdate();
            }
        }

        internal void TryMoveLeft()
        {
            if (!gameFieldGeometry.TestIntersect(CurrentFigure, new Point(-25, 0)))
            {
                CurrentFigure.CanvasPoint.X -= 25;
                CanvasPositionUpdate();
            }
        }

        internal void TurnFigure()
        {
            CurrentFigure.Angle += 90;
            if (gameFieldGeometry.TestIntersect(CurrentFigure, new Point(0, 0)) || TestHeight(CurrentFigure))
            {
                CurrentFigure.Angle -= 90;
                return;
            }
            CurrentFigure.Shape.LayoutTransform = new RotateTransform(CurrentFigure.Angle, 25, 25);
            if (CurrentFigure.GetCurrentWidth() + CurrentFigure.CanvasPoint.X >= fieldWidth)
                CurrentFigure.CanvasPoint.X = fieldWidth - CurrentFigure.GetCurrentWidth();
            CanvasPositionUpdate();
        }

        private void StopCurrentFigure()
        {
            gameFieldGeometry.AddGeometry(CurrentFigure);
            int count = gameFieldGeometry.RemoveFullLines();
            if (count > 0)
            {
                AddScore?.Invoke(this, count * 50);
                gameTimer.SpeedUp();
            }
            CreateNewFigure();
        }

        private bool TestHeight(Figure currentFigure)
        {
            if (currentFigure.CanvasPoint.Y + currentFigure.GetCurrentHeight() + 25 > fieldHeight)
                return true;
            return false;
        }

        public void Dispose()
        {
            GameFail = null;
            AddScore = null;
            gameTimer?.Dispose();
        }
    }
}