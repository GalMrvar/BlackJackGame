using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack
{
    public class StartGameEvent : EventArgs
    {
        private int players;
        private int decks;

        public StartGameEvent(int players, int decks)
        {
            this.players = players;
            this.decks = decks;
        }

        #region Get Set
        public int Players
        {
            get
            {
                return this.players;
            }
            set
            {
                if (value > 0)
                    this.players = value;
            }
        }

        public int Decks
        {
            get
            {
                return this.decks;
            }
            set
            {
                if (value > 0)
                    this.decks = value;
            }
        }

        #endregion
    }
}
