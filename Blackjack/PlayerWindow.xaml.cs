using GameCardLib;
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
using System.Windows.Shapes;

namespace Blackjack
{
    /**
    * @author Gal Mrvar
    */
    /// <summary>
    /// Interaction logic for PlayerWindow.xaml
    /// </summary>
    public partial class PlayerWindow : Window
    {
        private int playerID;
        GameController gameController;
        private bool thisPlayersTurn = false;

        public PlayerWindow(int index, GameController gameController)
        {
            this.playerID = index;
            this.gameController = gameController;
            InitializeComponent();
            AddToEvent();
            UpdatePlayerWindow();
        }  

        #region event handlers
        private void CheckNewCards(object sender, NewCardsEvent e)
        {
            if (e.PlayerId == playerID)
            {
                UpdatePlayerWindow();
                Player player = gameController.getPlayerById(playerID);
                if (player.Score >= 21 || player.Hand.NumberOfCards == 5)
                {
                    EnableButtons(false);
                    thisPlayersTurn = false;
                    gameController.Stand(playerID);
                }
            }
        }

        /// <summary>
        /// if it's current players turn he can play
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckTurnEvent(object sender, TurnEvent e)
        {
            Player player = gameController.getPlayerById(playerID);
            if (e.PlayerId == playerID)
            {
                thisPlayersTurn = true;
                UpdatePlayerWindow();
                if (player.Score >= 21)
                {
                    EnableButtons(false);
                    thisPlayersTurn = false;
                    buttonBet.IsEnabled = false; //only one bet allowed
                    TextBet.IsEnabled = false;
                    gameController.Stand(playerID);
                }
            }
            if (player.IsFinished && gameController.RoundOnGoing)
            {
                labelTurn.Content = "";
            }
        }

        private void CheckDissableShuffleEvent(object sender, DissableShuffleEvent e)
        {
            if (e.PlayerId == playerID)
            {
                buttonShuffle.IsEnabled = false;
            }
        }

        private void CheckEndOfRoundEvent(object sender, EndOfRoundEvent e)
        {
            Player player = gameController.getPlayerById(playerID);
            labelTurn.Content = "You "+player.Result;
        }

        #endregion


        #region button events

        /// <summary>
        /// When button shuffle is pressed it calls shuffle function in controller
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Shuffle(object sender, RoutedEventArgs e)
        {
            gameController.Shuffle(playerID);
        }

        /// <summary>
        /// Calling Hit function in controller
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Hit(object sender, RoutedEventArgs e)
        {
            buttonBet.IsEnabled = false; //only one bet allowed
            TextBet.IsEnabled = false;
            if (!gameController.HasPlayerPlacedABet(playerID))
                gameController.Bet(playerID, 0);
            gameController.Hit(playerID);
            if (gameController.RoundOnGoing)
                UpdatePlayerWindow();
        }

        /// <summary>
        /// Calling stand function in controller
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Stand(object sender, RoutedEventArgs e)
        {
            thisPlayersTurn = false;
            gameController.Stand(playerID);
            buttonBet.IsEnabled = false; //only one bet allowed
            TextBet.IsEnabled = false;
            if (!gameController.HasPlayerPlacedABet(playerID))
                gameController.Bet(playerID, 0);
            if (gameController.RoundOnGoing)
                UpdatePlayerWindow();
            else
                EnableButtons(false);
        }

        /// <summary>
        /// On click of bet button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_Bet(object sender, RoutedEventArgs e)
        {
            int bet = 0;
            if(!int.TryParse(TextBet.Text, out bet))
            {
                MessageBox.Show("Please input a valid number", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (bet > gameController.GetCurrentPlayersMoneyState(playerID))
            {
                MessageBox.Show("You don't have enoug resources to place this bet", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            buttonBet.IsEnabled = false; //only one bet allowed
            TextBet.IsEnabled = false;
            gameController.Bet(playerID, bet);
        }
        #endregion


        #region methods

        /// <summary>
        /// Updates player window
        /// </summary>
        public void UpdatePlayerWindow()
        {
            Player player = gameController.GetPlayers.Find(x => x.PlayerID == playerID);
            labelScore.Content = player.Score;
            Title = player.Name;
            labelState.Content = gameController.GetCurrentPlayersMoneyState(playerID).ToString() + "vc";
            if (player.Hand.NumberOfCards > 0)
                ChangePictures(player.Hand.CardsStringListImages);
            if (thisPlayersTurn)
            {
                EnableButtons(true);
                labelTurn.Content = "It's your turn";
                if (gameController.HasPlayerPlacedABet(playerID))
                {
                    buttonBet.IsEnabled = false; //only one bet allowed
                    TextBet.IsEnabled = false;
                }
                else
                {
                    buttonBet.IsEnabled = true; //only one bet allowed
                    TextBet.IsEnabled = true;
                }
            }
            else
            {
                EnableButtons(false);
                labelTurn.Content = "";
            }
        }

        /// <summary>
        /// Helper to UpdatePlayerWindow class
        /// </summary>
        /// <param name="value"></param>
        private void EnableButtons(bool value)
        {
            buttonHit.IsEnabled = value;
            buttonStand.IsEnabled = value;
            buttonShuffle.IsEnabled = value;
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
                object c = this.FindName("Card" + i);
                Image image = ((Image)c);
                if (string.IsNullOrEmpty(pictures[i - 1]))
                {
                    image.Source = null;
                }
                else
                {
                    image.Source = new BitmapImage(new Uri(@"cards/" + pictures[i - 1], UriKind.Relative));
                }
            }
        }

        /// <summary>
        /// Add all methods to necessary events;
        /// </summary>
        public void AddToEvent()
        {
            gameController.turnEvent += CheckTurnEvent;
            gameController.newCardsEvent += CheckNewCards;
            gameController.dissableShuffleEvent += CheckDissableShuffleEvent;
            gameController.endOfRoundEvent += CheckEndOfRoundEvent;
        }

        /// <summary>
        /// Players can't close their windows
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void PlayerWindow_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
        }
        #endregion

    }
}
