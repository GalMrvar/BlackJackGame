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
using System.Windows.Shapes;

namespace Blackjack
{
    /**
    * @author Gal Mrvar
    */
    /// <summary>
    /// Interaction logic for NewGameWindow.xaml
    /// </summary>
    public partial class NewGameWindow : Window
    {
        private int players;
        private int decks;

        public event EventHandler<StartGameEvent> startGameEvent;

        public NewGameWindow()
        {
            InitializeComponent();
        }

        public void Start_Click(object obj, RoutedEventArgs e)
        {
            if (!int.TryParse(txtNumberOfDecks.Text, out decks) || !int.TryParse(txtNumberOfPlayers.Text, out players) ||
                decks <= 0 || players <= 0)
            {
                MessageBox.Show("Please input correct values");
                return;
            }
            ///This triggers an event for new game
            StartGameEvent startEvent = new StartGameEvent(players, decks);

            if(startGameEvent != null)
                startGameEvent(this, startEvent);
        }

        private void TxtNumberOfDecks_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
