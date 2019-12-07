using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameCardLib
{
    public class Hand
    {
        public List<Card> cards = new List<Card>();

        public bool AddCard(Card card)
        {
            cards.Add(card);
            return true;
        }

        /// <summary>
        /// Deletes all cards from hand
        /// </summary>
        public void Clear()
        {
            cards = new List<Card>();
        }

        /// <summary>
        /// Removes all cards from hand
        /// </summary>
        public void EmptyHand()
        {
            cards = new List<Card>();
        }

        #region Get Set
        /// <summary>
        /// Only get
        /// </summary>
        public List<Card> Cards
        {
            get
            {
                return cards;
            }
        }

        public List<string> CardsStringListImages
        {
            get
            {
                List<string> list = new List<string>();
                foreach (Card card in cards)
                {
                    list.Add(card.Image);
                }
                return list;
            }
        }

        public int NumberOfCards
        {
            get
            {
                return cards.Count;
            }
        }

        /// <summary>
        /// returns score
        /// </summary>
        public int Score
        {
            get
            {
                /*int sum = 0;
                foreach (Card c in cards)
                {
                    sum += c.Value;
                }*/
                return cards.Sum(x=> x.Value)+1;

                //return sum;
            }
        }


        #endregion
    }
}
