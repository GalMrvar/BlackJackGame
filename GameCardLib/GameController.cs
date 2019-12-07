using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using BlackjackDBLib;

namespace GameCardLib
{
    public class GameController
    {
        /**
        * @author Gal Mrvar
        */

        private List<Player> players;
        private Deck deck;
        private int dealerIndex;
        private bool roundOnGoing;
        private Player currentPlayer;
        private DatabaseController databaseController = new DatabaseController();

        private int numberOfRounds;

        public event EventHandler<TurnEvent> turnEvent;
        public event EventHandler<NewCardsEvent> newCardsEvent;
        public event EventHandler<DissableShuffleEvent> dissableShuffleEvent;
        public event EventHandler<EndOfRoundEvent> endOfRoundEvent;

        public delegate Player GetPlayerById(int id);
        public GetPlayerById getPlayerById;

        public GameController()
        {
            getPlayerById = (int id) => players.Find(p => p.PlayerID == id); //checks if there are still players left in a game
        }

        #region Methods
        /// <summary>
        /// Starts new game from start
        /// </summary>
        /// <param name="decks"></param>
        /// <param name="player"></param>
        public void StartNewGame(int decks, int player)
        {
            players = new List<Player>();
            int index = 0;
            for (int i = 1; i <= player; i++)
            {
                Player p = new Player(i);
                int id = databaseController.AddNewPlayer(p.Name, 1000); //Everyone starts with 1000
                p.PlayerID = id;
                players.Add(p);
                index++;
            }
            players.Add(new Player(0)); //dealer
            dealerIndex = index;
            deck = new Deck(decks);   
        }

        /// <summary>
        /// Starts first round
        /// </summary>
        public void StartNewRound()
        {
            //get all cards from players to used cards pile and reset to empty
            AllCardsToUsedPileAndResetHand();
            databaseController.StartNewRound();

            if(numberOfRounds > 0)//check if there is less then 25% of cards and if it is we ask players if they want to reshuffle
            {
                if (deck.LessThen25())
                {
                    if (MessageBox.Show("The Deck has less then 25% of cards remaining.", "Re-shuffle?", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        deck.Shuffle();
                    }
                }
            }
            numberOfRounds++;
            //then start
            roundOnGoing = true;
            foreach (Player player in players) //first we need to setup turn in db otherwise dealer can trigger end of round before initialization
            {
                if (player.PlayerID != 0)
                    databaseController.InitializeNewTurnForPlayer(player.PlayerID);
            }

            foreach (Player player in players) //every player gets 2 cards
            {  
                player.IsFinished = false;
                AddNextCardToPlayer(player);
                AddNextCardToPlayer(player);
            }
            NextTurn();
        }
        /// <summary>
        /// next turn
        /// </summary>
        public void NextTurn()
        {
            List<Player> listOfActivePlayers = players.FindAll(x => x.IsFinished == false);
            if(listOfActivePlayers.Count == 0)
            {
                EndOfRound();
                return;
            }
            currentPlayer = listOfActivePlayers[0];

            TurnEvent startEvent = new TurnEvent(currentPlayer.PlayerID);
            if (turnEvent != null)
                turnEvent(this, startEvent);
        }

        /// <summary>
        /// Same player again
        /// </summary>
        public void CurrentPlayersTurn()
        {
            TurnEvent startEvent = new TurnEvent(currentPlayer.PlayerID);
            if (turnEvent != null)
                turnEvent(this, startEvent);
        }

        /// <summary>
        /// End of round controller
        /// </summary>
        private void EndOfRound()
        {
            if (roundOnGoing)
            {
                foreach (Player player in players)
                {
                    //Won
                    if (player.Score > Dealer.Score && player.Score <= 21 || player.Score <= 21 && Dealer.Score > 21) //dealer excluded
                    {
                        player.AddResult("Won", numberOfRounds);
                        if (player.PlayerID != 0)
                            databaseController.SetResultForPlayerThisRound(player.PlayerID, true);
                    }
                    //lost
                    else
                    {
                        player.AddResult("Lost", numberOfRounds);
                        if (player.PlayerID != 0)
                            databaseController.SetResultForPlayerThisRound(player.PlayerID, false);
                    }
                }
            }
            roundOnGoing = false;
            EndOfRoundEvent startEvent = new EndOfRoundEvent();
            if (endOfRoundEvent != null)
                endOfRoundEvent(this,startEvent);
        }

        /// <summary>
        /// Calls serialize function in helper class
        /// </summary>
        public bool SerializeToXML(string fileName)
        {
            List<Player> results = new List<Player>();
            foreach(Player player in players)
            {
                if(player.PlayerID != 0)
                {
                    results.Add(player);
                }
            }
            return UtilitiesLib.UtilitiesStaticHelper<Player>.XmlSerialize(fileName, results);
        }

        /// <summary>
        /// When next card is added it triggers an event
        /// </summary>
        /// <param name="player"></param>
        public void AddNextCardToPlayer(Player player)
        {
            player.Hand.AddCard(deck.DrawNextCard());
            NewCardsEvent cardEvent = new NewCardsEvent(player.PlayerID);
            if (newCardsEvent != null)
                newCardsEvent(this, cardEvent);
        }

        /// <summary>
        /// Adds all cards from previous round to used card pile and then resets all players hands
        /// </summary>
        public void AllCardsToUsedPileAndResetHand()
        {
            foreach(Player player in players)
            {
                deck.AddToUsedCardsPile(player.Hand.cards);
                player.Hand.EmptyHand();
            }
        }

        /// <summary>
        /// Checking if player has placed a bet
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public bool HasPlayerPlacedABet(int playerId)
        {
            return databaseController.HasPlayerPlacedBetThisTurn(playerId);
        }

        /// <summary>
        /// Returns current money balance of a player
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public int GetCurrentPlayersMoneyState(int playerId)
        {
            return databaseController.GetCurrentMoneyState(playerId);
        }

        #endregion

        #region Players response

        /// <summary>
        /// When player clicks shuffle button this method is called
        /// </summary>
        /// <param name="playerId"></param>
        public void Shuffle(int playerId)
        {
            Deck.Shuffle();
            DissableShuffleEvent eventShuffle = new DissableShuffleEvent(playerId);
            if (dissableShuffleEvent != null)
                dissableShuffleEvent(this, eventShuffle);
        }

        public void Hit(int playerId)
        {
            Player player = getPlayerById(playerId);
            if(player.Hand.NumberOfCards < 5) // only 5 cards
                AddNextCardToPlayer(player); //ading next cart
            CurrentPlayersTurn();
        }

        public void Stand(int playerId)
        {
            Player player = getPlayerById(playerId);
            player.IsFinished = true;
            if (playerId == 0)
            { // dealer breakes 
                EndOfRound();
                return;
            }

            NextTurn();
        }

        public void Bet(int playerId, int bet)
        {
            databaseController.PlaceBet(playerId, bet);
        }
        #endregion

        #region Get Set
        public Deck Deck
        {
            get
            {
                return deck;
            }
        }

        public List<Player> GetPlayers
        {
            get
            {
                return players;
            }
        }

        public Player Dealer
        {
            get
            {
                return players[dealerIndex];
            }
        }

        public bool RoundOnGoing
        {
            get
            {
                return roundOnGoing;
            }
        }
        #endregion 
    }
}
