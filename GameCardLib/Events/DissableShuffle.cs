using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCardLib
{
    public class DissableShuffleEvent : EventArgs
    {
        private int playerId;

        public DissableShuffleEvent(int index)
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
