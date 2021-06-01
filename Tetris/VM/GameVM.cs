using System.ComponentModel;

namespace Tetris
{
    internal class GameVM : INotifyPropertyChanged
    {
        public int Scores { get => game.Scores; }

        private Game game;

        public GameVM(Game game)
        {
            this.game = game;
            game.ScoresChanged += Game_ScoresChanged;
        }

        private void Game_ScoresChanged(object sender, System.EventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Scores)));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}