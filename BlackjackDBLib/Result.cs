using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BlackjackDBLib
{
    public class Result
    {
        [Key]
        public int ResultId { get; set; }
        public bool Win { get; set; }
    }
}
