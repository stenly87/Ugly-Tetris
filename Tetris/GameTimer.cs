using System;
using System.Windows.Threading;

namespace Tetris
{
    internal class GameTimer
    {
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        public event EventHandler TickEvent;

        public GameTimer()
        {
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
            dispatcherTimer.Tick += Tick;
        }

        private void Tick(object sender, EventArgs e)
        {
            TickEvent?.Invoke(this, null);
        }

        internal void Play()
        {
            dispatcherTimer.Start();
        }

        internal void Pause()
        {
            dispatcherTimer.Stop();
        }
    }
}