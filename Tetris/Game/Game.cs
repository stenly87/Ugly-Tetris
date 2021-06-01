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
        private readonly Canvas mainCanvas;
        private readonly SoundWorker soundWorker;
        private readonly FigureGenerator figureGenerator;

        private int scores;
        internal int Scores
        {
            get => scores;
            set
            {
                scores = value;
                ScoresChanged?.Invoke(this, null);
            }
        }

        internal event EventHandler ScoresChanged;
        bool running = false;
        GameProcess gameProcess;

        public Game(Canvas mainCanvas, int fieldWidth, int fieldHeight, SoundWorker worker, FigureGenerator figureGenerator)
        {
            mainCanvas.Width = fieldWidth;
            mainCanvas.Height = fieldHeight;
            this.mainCanvas = mainCanvas;
            this.soundWorker = worker;
            this.figureGenerator = figureGenerator;

            Restart();
        }

        internal void KeyUp(Key key)
        {
            switch (key)
            {
                case Key.Up:
                    gameProcess.TurnFigure();
                    break;
                case Key.Left:
                    gameProcess.TryMoveLeft();
                    break;
                case Key.Right:
                    gameProcess.TryMoveRight();
                    break;
                case Key.Down:
                    gameProcess.TryMoveDown();
                    break;
                case Key.Space:
                    running = !running;
                    gameProcess.SetGameStatus(running);
                    break;
            }
        }

        private void Restart()
        {
            Scores = 0;
            gameProcess?.Dispose();// удаляем прошлые подписки и таймеры
            gameProcess = new GameProcess(mainCanvas, figureGenerator);
            gameProcess.GameFail += FigureProcess_GameFail;
            gameProcess.AddScore += FigureProcess_AddScore;
            gameProcess.Start();
        }

        private void FigureProcess_AddScore(object sender, int count)
        {
            Scores += count;
        }

        private void FigureProcess_GameFail(object sender, EventArgs e)
        {
            gameProcess.Stop();
            soundWorker.PlayCongratulations();
            if (MessageBox.Show("Вы проиграли. Хотите еще раз?", "Игра окончена", MessageBoxButton.YesNo) ==
                 MessageBoxResult.Yes)
                Restart();
            else
                Application.Current.Shutdown();
        }




    }
}
