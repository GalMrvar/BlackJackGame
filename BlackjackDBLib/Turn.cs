using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BlackjackDBLib
{
    public class Turn
    {
        [Key]
        public int TurnId { get; set; }
        public Player Player { get; set; }
        public Result Result { get; set; }
        public Round Round { get; set; }
        public int Bet { get; set; }
        public bool PlacedBet { get; set; }
    }
}
