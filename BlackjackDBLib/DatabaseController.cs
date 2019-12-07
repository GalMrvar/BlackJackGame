using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace BlackjackDBLib
{
    public class DatabaseController
    {
        #region fields

        private BlackjackDbContext dbContext = new BlackjackDbContext();

        #endregion

        #region Methods

        public int AddNewPlayer(string name, int money)
        {
            Player p = new Player();
            p.Money = money;
            p.Name = name;
            dbContext.Players.Add(p);
            Save();
            return p.PlayerId;
        }

        /// <summary>
        /// Calling when starting a new round
        /// </summary>
        public void StartNewRound()
        {
            Round r = new Round();

            dbContext.Rounds.Add(r);
            Save();
        }

        /// <summary>
        /// Everytime we start a new round we predefine turns for all players
        /// </summary>
        /// <param name="playerId"></param>
        public void InitializeNewTurnForPlayer(int playerId)
        {
            Result r = new Result();

            dbContext.Results.Add(r);

            Turn t = new Turn();
            t.Round = GetCurrentRound();
            t.Player = GetPlayer(playerId);
            t.Result = r;
            t.PlacedBet = false;

            dbContext.Turns.Add(t);
            Save();
        }

        /// <summary>
        /// Setting the results for provided player
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="win"></param>
        public void SetResultForPlayerThisRound(int playerId, bool win)
        {
            Turn turn = GetCurrentTurnForPlayer(playerId);
            Result result = dbContext.Results.Find(turn.Result.ResultId);

            result.Win = win;

            ChangeMoneyState(playerId, win);

            Save();
        }

        /// <summary>
        /// Placing a bet
        /// </summary>
        /// <param name="playerId"></param>
        public void PlaceBet(int playerId, int bet)
        {
            Turn turn = GetCurrentTurnForPlayer(playerId);
            turn.Bet = bet;
            turn.PlacedBet = true;
            Save();
        }

        /// <summary>
        /// Checks if player has already placed a bet
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public bool HasPlayerPlacedBetThisTurn(int playerId)
        {
            Turn turn = GetCurrentTurnForPlayer(playerId);
            return turn.PlacedBet;
        }

        /// <summary>
        /// Returns the amount of money that player currently has
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public int GetCurrentMoneyState(int playerId)
        {
            return dbContext.Players.Find(playerId).Money;
        }

        /// <summary>
        /// returns current account balance of a player
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public int GetCurrentPlayerMoneyStatus(int playerId)
        {
            Player p = dbContext.Players.Find(playerId);
            return p.Money;
        }

        /// <summary>
        /// returns last 20 results from database
        /// </summary>
        /// <returns></returns>
        public List<ResultData> GetLast20results()
        {
            List<ResultData> list = new List<ResultData>();
            //get all players
            var lastPlayers = dbContext.Players.OrderByDescending(x => x.PlayerId).Take(20);
            foreach(Player player in lastPlayers)
            {
                int win = 0; int lose = 0; decimal ratio = 0;
                var turnsForPlayer = dbContext.Turns.Where(x => x.Player.PlayerId == player.PlayerId).Include("Result");
                foreach (Turn turn in turnsForPlayer)
                {
                    if (turn.Result == null)
                        continue;
                    if (turn.Result.Win)
                        win++;
                    else
                        lose++;
                }
                if (lose == 0)
                    ratio = win / win * 100;
                else if (win == 0 && lose == 0 || win == 0)
                    ratio = 0;
                else
                    ratio = ((decimal)win / (decimal)(lose+win) *100);

                ratio = decimal.Round(ratio, 2);
                list.Add(new ResultData
                {
                    Id = player.PlayerId,
                    PlayerName = player.Name,
                    Money = player.Money,
                    Wins = win,
                    Loses = lose,
                    Ratio = ratio

                });
            }
            return list;
        }

        /// <summary>
        /// Updating player on provided data
        /// </summary>
        /// <param name="data"></param>
        public void UpdatePlayer(ResultData data)
        {
            Player player = dbContext.Players.Find(data.Id);
            player.Money = data.Money;
            player.Name = data.PlayerName;
            Save();
        }

        /// <summary>
        /// Deletes a player
        /// </summary>
        /// <param name="data"></param>
        public void DeletePlayer(ResultData data)
        {
            Player player = dbContext.Players.Find(data.Id);
            dbContext.Players.Remove(player);
            Save();
        }

        /// <summary>
        /// returns search result by string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public List<ResultData> SearchByString(string str)
        {
            List<ResultData> list = new List<ResultData>();
            int value = 0;
            if(!int.TryParse(str, out value))
            {
                value = int.MinValue;
            }
            //get all players
            var lastPlayers = dbContext.Players
                .Where(x => x.Name.Contains(str) || x.Money == value || x.PlayerId == value)
                .OrderByDescending(x => x.PlayerId)
                .Take(20);
            foreach (Player player in lastPlayers)
            {
                int win = 0; int lose = 0; decimal ratio = 0;
                var turnsForPlayer = dbContext.Turns.Where(x => x.Player.PlayerId == player.PlayerId).Include("Result");
                foreach (Turn turn in turnsForPlayer)
                {
                    if (turn.Result == null)
                        continue;
                    if (turn.Result.Win)
                        win++;
                    else
                        lose++;
                }
                if (win == 0 && lose == 0 || win == 0)
                    ratio = 0;
                else if (lose == 0)
                    ratio = win / win * 100;
                else
                    ratio = ((decimal)win / (decimal)(lose + win) * 100);

                ratio = decimal.Round(ratio, 2);
                list.Add(new ResultData
                {
                    Id = player.PlayerId,
                    PlayerName = player.Name,
                    Money = player.Money,
                    Wins = win,
                    Loses = lose,
                    Ratio = ratio

                });
            }
            return list;
        }

        /// <summary>
        /// Changing state of money for player
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="win"></param>
        private void ChangeMoneyState(int playerId, bool win)
        {
            Turn currentTurn = GetCurrentTurnForPlayer(playerId);

            int bet = currentTurn.Bet;
            Player player = dbContext.Players.Find(playerId);
            if (win)
                player.Money += bet;
            else
                player.Money -= bet;
            Save();
        }

        /// <summary>
        /// Get turn for provided round and player id
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="roundId"></param>
        /// <returns></returns>
        private Turn GetCurrentTurnForPlayer(int playerId)
        {
            Round round = GetCurrentRound();

            return dbContext.Turns.Where(x => x.Player.PlayerId == playerId && x.Round.RoundId == round.RoundId).FirstOrDefault();
        }

        /// <summary>
        /// Returns current round
        /// </summary>
        /// <returns></returns>
        private Round GetCurrentRound()
        {
            return dbContext.Rounds.OrderByDescending(p => p.RoundId).FirstOrDefault();
        }

        /// <summary>
    /// Returns current player
    /// </summary>
    /// <param name="playerId"></param>
    /// <returns></returns>
        private Player GetPlayer(int playerId)
        {
            return dbContext.Players.Find(playerId);
        }

        /// <summary>
        /// Saving changes
        /// </summary>
        private void Save()
        {
            dbContext.SaveChanges();
        }

        #endregion
    }
}
