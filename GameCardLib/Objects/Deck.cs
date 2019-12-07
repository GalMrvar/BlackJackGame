using System;
using System.Collections.Generic;
using System.Text;

namespace GameCardLib
{
    public class Deck
    {
        private Random rng = new Random();

        private List<Card> cards;
        private List<Card> usedCards;

        private int count;
        private int numberOfDecks;

        #region constructor and initialization
        public Deck(int numberOfDecks)
        {
            cards = new List<Card>();
            usedCards = new List<Card>();
            this.numberOfDecks = numberOfDecks;
            for (int i = 0; i < numberOfDecks; i++)
            {
                InitializeDeck();
                Shuffle();
            }
        }

        /// <summary>
        /// Creates deck and connects it with pictures
        /// </summary>
        private void InitializeDeck()
        {
            InitializeDeckHelper(Suites.Clubs); //clubs
            InitializeDeckHelper(Suites.Hearts); //hearts
            InitializeDeckHelper(Suites.Spades); //spades
            InitializeDeckHelper(Suites.Diamonds); //diamonds
        }

        /// <summary>
        /// Helper method for initilize deck in Deck.cs
        /// </summary>
        /// <param name="suites"></param>
        private void InitializeDeckHelper(Suites suites)
        {
            for (int i = 1; i <= 13; i++)
            {
                cards.Add(new Card(suites, i));
                count++;
            }
        }
        #endregion

        /// <summary>
        /// Shuffles card deck randomly
        /// </summary>
        public void Shuffle()
        {
            int n = cards.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Card value = cards[k];
                cards[k] = cards[n];
                cards[n] = value;
            }
        }

        /// <summary>
        /// Drrawing next card
        /// </summary>
        /// <returns></returns>
        public Card DrawNextCard()
        {
            if(cards.Count == 0) //if there are no cards left we have to take them from used cards pile and reshuffle them
            {
                AddCardsFromUsedPile();
                Shuffle();
            }
            count++;
            Card card = cards[0];
            cards.RemoveAt(0);
            return card;
        }

        /// <summary>
        /// Returns true if there is less then 25% of cards in deck
        /// </summary>
        /// <returns></returns>
        public bool LessThen25()
        {
            if ((cards.Count) < (52 * numberOfDecks * 0.25))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// add cards to used cards pile (out of game)
        /// </summary>
        public void AddToUsedCardsPile(List<Card> cards)
        {
            if(cards.Count != 0)
                usedCards.AddRange(cards);
        }

        /// <summary>
        /// If there are no cards left or les then 25% you take them from used pile
        /// </summary>
        public void AddCardsFromUsedPile()
        {
            //adding cards to the bottom
            cards.AddRange(usedCards);
        }

        #region Get Set
        public int Count
        {
            get
            {
                return cards.Count;
            }
        }

        public int NumberOfDecks
        {
            get
            {
                return numberOfDecks;
            }
        }

        #endregion
    }
}
