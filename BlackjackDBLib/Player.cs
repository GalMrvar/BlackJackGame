using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BlackjackDBLib
{
    public class Player
    {
        [Key]
        public int PlayerId { get; set; }

        public string Name { get; set; }

        public int Money { get; set; }

    }
}
