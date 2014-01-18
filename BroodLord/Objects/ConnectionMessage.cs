using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Objects
{

    [Serializable()]
    class ConnectionMessage
    {
        /// <summary>
        /// Returns the size of this message when it is seralized
        /// </summary>
        /// <returns>the size</returns>
        public static int serializedMessageSize()
        {
            MemoryStream ms = new MemoryStream();
            new BinaryFormatter().Serialize(ms, new ConnectionMessage());
            byte[] d = ms.ToArray();
            return d.Length;
        }
    }
}