using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCardLib
{
    public class TurnEvent : EventArgs
    {
        private int playerId;

        public TurnEvent(int index)
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
