using System;
using System.Collections.Generic;
using System.Text;

namespace BlackjackDBLib
{
    public class ResultData
    {
        public int Id { get; set; }
        public string PlayerName { get; set; }
        public int Money { get; set; }
        public int Wins { get; set; }
        public int Loses { get; set; }
        public decimal Ratio { get; set; }
    }
}
