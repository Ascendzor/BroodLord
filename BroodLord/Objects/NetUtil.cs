using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Objects
{
    public class Net
    {
                /// <summary>
        /// Attempts to read a datasizemessage of the stream, throws an Exception if it fails for some reason
        /// </summary>
        /// <param name="stream"> stream which will be read from </param>
        /// <returns>DataSizeMessage</returns>        public static GameDataSizeMessage RecieveDataSizeMessage(NetworkStream stream)
        {
            try
            {
                MemoryStream mStream = new MemoryStream();
                byte[] messageData = new byte[GameDataSizeMessage.MessageSize];
                stream.Read(messageData, 0, messageData.Length);
                mStream.Write(messageData, 0, messageData.Length);
                mStream.Seek(0, SeekOrigin.Begin);
                GameDataSizeMessage dataMessage = (GameDataSizeMessage)new BinaryFormatter().Deserialize(mStream);
                return dataMessage;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
