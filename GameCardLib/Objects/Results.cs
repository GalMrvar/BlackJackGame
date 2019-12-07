using System;
using System.Collections.Generic;
using System.Text;

namespace GameCardLib
{
    [Serializable]
    public class Results
    {
        int numberOfRounds;
        int numberOfPlayers;
        int numberOfDecks;

        List<(Player, int)> listWins = new List<(Player, int )>();
        List<(Player, int)> listLoses = new List<(Player, int )>();

        public Results() { }

        public Results(int players, int decks)
        {
            numberOfDecks = decks;
            numberOfPlayers = players;
        }

        public void AddSetWinsOnPlayersID(int id)
        {
            foreach((Player player, int wins) in listWins)
            {
                if(player.PlayerID == id)
                {
                    //listWins;
                }
            }
        }

        public int NumberOfRounds
        {
            get
            {
                return this.numberOfRounds;
            }
        }

        public (Player, int) Wins
        {
            set
            {
                listWins.Add(value);
            }
        }

        public (Player, int) Loses
        {
            set
            {
                listLoses.Add(value);
            }
        }

    }
}
