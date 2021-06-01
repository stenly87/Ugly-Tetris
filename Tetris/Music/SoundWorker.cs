using System;
using System.Collections.Generic;
using System.Media;
using System.Text;

namespace Tetris
{
    public class SoundWorker
    {
        SoundPlayer player = new SoundPlayer();

        internal void PlayCongratulations()
        {
            player.SoundLocation = Environment.CurrentDirectory + "/Music/аплодисменты.wav";
            player.Play();
        }
    }
}
