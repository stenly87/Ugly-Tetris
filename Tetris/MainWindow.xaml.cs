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
            game = new Game(mainCanvas, 500, 500);
            DataContext = game;
        }
        private void keyUpMethod(object sender, KeyEventArgs e)
        {
            game.KeyUp(e.Key);
        }
    }
}
