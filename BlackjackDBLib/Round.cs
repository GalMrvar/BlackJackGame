using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace BlackjackDBLib
{
    public class Round
    {
        [Key]
        public int RoundId { get; set; }
    }
}
