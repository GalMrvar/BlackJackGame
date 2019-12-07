using GameCardLib;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Blackjack
{
    /**
    * @author Gal Mrvar
    */
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private NewGameWindow newGameWindow;
        private GameController gameController;

        private List<PlayerWindow> openedPlayerWindows;

        private int players = 0;
        private int decks = 0;

        public MainWindow()
        {
            InitializeComponent();
            InitializeWindow();
        }
        #region Buttons

        /// <summary>
        /// starting a new game
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="e"></param>
        public void ButtonNewGame_Click(object obj, RoutedEventArgs e)
        {
            newGameWindow = new NewGameWindow();
            newGameWindow.Show();

            //adding to event
            newGameWindow.startGameEvent += OnStartOfGame;
        }

        /// <summary>
        /// Exit the app
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        /// <summary>
        /// When starting new round
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonNewRound_Click(object sender, RoutedEventArgs e)
        {
            gameController.StartNewRound();
            ButtonNewRound.IsEnabled = false;
            if (!gameController.RoundOnGoing)
                ButtonNewRound.IsEnabled = true;
        }

        /// <summary>
        /// On clikc of results button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonResults_Click(object sender, RoutedEventArgs e)
        {
            ResultsWindow resultsWindow = new ResultsWindow();
            resultsWindow.Show();
        }

        /// <summary>
        /// Serialize to xml
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonExport1_Click(object sender, RoutedEventArgs e)
        {
            if(gameController == null)
            {
                MessageBox.Show("Please start the game before exporting to xml", "game didnt start yet", MessageBoxButton.OK);
            }
            else
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "XML Files (*.xml)|*.xml"; //only for xml
                saveFileDialog1.DefaultExt = "xml";
                saveFileDialog1.AddExtension = true;
                saveFileDialog1.InitialDirectory = System.Reflection.Assembly.GetExecutingAssembly().Location;

                if (saveFileDialog1.ShowDialog() == true)
                {
                    string xmlFileName = saveFileDialog1.FileName;  //important

                    if (gameController.SerializeToXML(xmlFileName))
                    {
                        MessageBox.Show("successfully exported to local folder", "success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("something went wrong while exporting to xml", "Failure", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

            }
        }

        #endregion

        #region event handlers

        /// <summary>
        /// When button for new game is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnStartOfGame(object sender, StartGameEvent e)
        {
            this.players = e.Players;
            this.decks = e.Decks;

            newGameWindow.Close(); //closing window for new game
            StartNewGame();
        }

        /// <summary>
        /// event triggeres each turn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckTurnEvent(object sender, TurnEvent e)
        {
            if(e.PlayerId == 0)//dealer
            {
                DealersTurn();
            }
        }

        /// <summary>
        /// If dealer got new cards
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckNewCards(object sender, NewCardsEvent e)
        {
            if (e.PlayerId == 0)
            {
                UpdateMainWindow();
            }
        }

        /// <summary>
        /// When round ends this method is triggered
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckEndOfRound(object sender, EndOfRoundEvent e)
        {
            ButtonNewRound.IsEnabled = true;
        }

        #endregion

        #region methods

        /// <summary>
        /// Dealer playes if his score is less then 17
        /// </summary>
        private void DealersTurn()
        {
            Player dealer = gameController.Dealer;
            if (dealer.Score >= 17)
                gameController.Stand(0);
            else
            {
                gameController.Hit(0);
            }
        }

        /// <summary>
        /// Add all methods to necessary events
        /// </summary>
        private void AddToEvent()
        {
            gameController.turnEvent += CheckTurnEvent;
            gameController.newCardsEvent += CheckNewCards;
            gameController.endOfRoundEvent += CheckEndOfRound;
        }

        /// <summary>
        /// Initializes window on start
        /// </summary>
        private void InitializeWindow()
        {
            openedPlayerWindows = new List<PlayerWindow>();
            ButtonNewRound.IsEnabled = false;
            ChangePictures(new List<string>());
        }

        /// <summary>
        /// Starting new game
        /// </summary>
        private void StartNewGame()
        {
            CloseAllPlayerWindows();
            gameController = new GameController();
            //add events
            AddToEvent();
            gameController.StartNewGame(decks, players);
            InitilizePlayerWindows();
            gameController.StartNewRound();   
            UpdateMainWindow();
        }

        /// <summary>
        /// Initializing and showing all player windows
        /// </summary>
        public void InitilizePlayerWindows()
        {
            for (int i = 0; i < players; i++)
            {
                PlayerWindow playerWindow = new PlayerWindow(gameController.GetPlayers[i].PlayerID,gameController);
                openedPlayerWindows.Add(playerWindow);
                playerWindow.Show();
            }
        }

        /// <summary>
        /// Closing all opened player windows and deleting opened ones
        /// </summary>
        private void CloseAllPlayerWindows()
        {
            foreach(PlayerWindow playerWindow in openedPlayerWindows)
            {
                playerWindow.Close();
            }
            openedPlayerWindows = new List<PlayerWindow>();
        }

        /// <summary>
        /// Updates main window
        /// </summary>
        public void UpdateMainWindow()
        {
            if (decks != 0)
                labelDecks.Content = decks.ToString();
            if (players != 0)
                labelPlayers.Content = players.ToString();
            labelDealerScore.Content = gameController.Dealer.Score;
            if (gameController.Dealer.Hand.NumberOfCards > 0)
            { //changing cards for dealer
                ChangePictures(gameController.Dealer.Hand.CardsStringListImages);
            }
            if (!gameController.RoundOnGoing)
                ButtonNewRound.IsEnabled = true;
        }

        /// <summary>
        /// Changes images in the same order as in list
        /// </summary>
        /// <param name="pictures"></param>
        public void ChangePictures(List<string> pictures)
        {
            for (int i = 1; i <= 5; i++)
            {
                object c = this.FindName("Card" + i);
                Image image = ((Image)c);
                image.Source = null;
            }
            for (int i = 1; i <= pictures.Count; i++) //max 5 cards
            {
                object c = this.FindName("Card"+i);
                Image image = ((Image)c);
                if (string.IsNullOrEmpty(pictures[i-1])){
                    image.Source = null;
                }
                else
                {
                    image.Source = new BitmapImage(new Uri(@"cards/" + pictures[i-1], UriKind.Relative));
                }
            }
        }

        /// <summary>
        /// Closing complete app
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
        #endregion
    }
}
