using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Objects
{
    /// <summary>
    /// this is intended the message which tells the client/server how much data is about to be sent
    /// </summary>
    [Serializable()]
    public class GameDataSizeMessage
    {
        private int sizeOf;

        public GameDataSizeMessage(int sizeOf)
        {
            this.sizeOf = sizeOf;
        }

        /// <summary>
        /// Get the size of the next set of Data Message
        /// </summary>
        public int sizeOfData
        { 
            get {
                return sizeOf;
            }
        }
    }
}
