using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCardLib
{
    public class NewCardsEvent : EventArgs
    {
        private int playerId;

        public NewCardsEvent(int index)
        {
            playerId = index;
        }

        public int PlayerId
        {
            get
            {
                return playerId;
            }
        }
    }
}
