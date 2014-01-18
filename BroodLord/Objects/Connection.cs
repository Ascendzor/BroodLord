using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

namespace Objects
{
    public class Connection
    {
        private NetworkStream inStream;
        private NetworkStream outStream;

        public Connection(NetworkStream inStream, NetworkStream outStream)
        {
            this.inStream = inStream;
            this.outStream = outStream;
        }
        public void close()
        {
            inStream.Close();
            outStream.Close();
        }
        public NetworkStream InStream
        {
            get { return inStream; }
            set { inStream = value; }
        }

        public NetworkStream OutStream
        {
            get { return outStream; }
            set { outStream = value; }
        }

    }
}
