using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

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
        public int SizeOfData
        { 
            get {
                return sizeOf;
            }
        }
        /// <summary>
        /// Returns the size of this message when it is seralized
        /// </summary>
        /// <returns>the size</returns>
        public static int MessageSize
        {
            get
            {
                MemoryStream ms = new MemoryStream();
                new BinaryFormatter().Serialize(ms, new ConnectionMessage());
                byte[] d = ms.ToArray();
                return d.Length;
            }
        }
    }
}
