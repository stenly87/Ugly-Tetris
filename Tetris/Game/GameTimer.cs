using System;
using System.Windows.Threading;

namespace Tetris
{
    internal class GameTimer : IDisposable
    {
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        private int interval;

        public event EventHandler TickEvent;

        public GameTimer(int interval)
        {
            SetGameSpeed(interval);
            dispatcherTimer.Start();
            dispatcherTimer.Tick += Tick;
        }

        private void Tick(object sender, EventArgs e)
        {
            TickEvent?.Invoke(this, null);
        }

        void SetGameSpeed(int interval)
        {
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(interval);
            this.interval = interval;
        }

        internal void SpeedUp()
        {
            if (interval > 100)
                interval -= 10;
        }

        internal void Play()
        {
            dispatcherTimer.Start();
        }

        internal void Pause()
        {
            dispatcherTimer.Stop();
        }

        public void Dispose()
        {
            Pause();
            TickEvent = null;
        }
    }
}