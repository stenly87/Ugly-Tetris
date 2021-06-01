using System.Windows;
using System.Windows.Input;

namespace Tetris
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        Game game;

        public MainWindow()
        {
            InitializeComponent();
            game = new Game(mainCanvas, 200, 400, 
                new SoundWorker(), 
                new FigureGenerator(
                    new BrushRandomizer(), showNextFigureCanvas));
            DataContext = new GameVM(game);
        }
        private void keyUpMethod(object sender, KeyEventArgs e)
        {
            game.KeyUp(e.Key);
        }
    }
}
