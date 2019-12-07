using System;
using System.Xml.Serialization;

namespace GameCardLib
{
    [Serializable]
    public class Player
    {
        private bool isFinished;
        private string name;
        private int playerID;

        private bool isDealer = false;
        private string lastResult;
        private int lastRound;

        private Hand hand;

        private int wins = 0;
        private int loses = 0;

        #region constructor

        public Player() { }

        public Player (int number)
        {
            this.name = "Player " + number.ToString();
            this.playerID = number;
            this.hand = new Hand();
            if (number == 0)
                isDealer = true;
        }
        #endregion

        #region methods

        /// <summary>
        /// Adds result to player
        /// </summary>
        /// <param name="result"></param>
        /// <param name="playerId"></param>
        /// <param name="round"></param>
        public void AddResult(string result, int round)
        {
            if(lastRound != round)
            {
                lastResult = result;
                lastRound = round;
                if (result == "Won")
                    Wins = 1;
                else
                    Loses = 1;
            }
        }

        #endregion

        #region get set

        [XmlIgnore()]
        public bool IsFinished
        {
            get
            {
                return isFinished;
            }
            set
            {
                isFinished = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (value.Length > 0)
                    name = value;
            }
        }

        public int PlayerID
        {
            get
            {
                return playerID;
            }
            set
            {
                if (value >= 0) //it has to be bigger than 0
                    playerID = value;
            }
        }

        [XmlIgnore()]
        public int Score
        {
            get
            {
                return hand.Score;
            }
        }

        [XmlIgnore()]
        public Hand Hand
        {
            get
            {
                return this.hand;
            }
        }

        [XmlIgnore()]
        public string Result
        {
            get
            {
                return this.lastResult;
            }
            set
            {
                if (value.Length > 0)
                    this.lastResult = value;
            }
        }

        public int Wins
        {
            get
            {
                return wins;
            }
            set
            {
                if(value > 0)
                    wins += 1;
            }
        }

        public int Loses
        {
            get
            {
                return loses;
            }
            set
            {
                if (value > 0)
                    loses += 1;
            }
        }
        #endregion
    }
}
