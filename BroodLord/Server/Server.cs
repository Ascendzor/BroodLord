using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using Objects;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Xna.Framework;

namespace Server
{
    class Server
    {
        int port;
        private List<NetworkStream> streams;
        private TcpListener listener;

        public Server()
        {
            port = 41337;
            listener = new TcpListener(port);
            streams = new List<NetworkStream>();
        }

        public void Run()
        {
            ListenForNewConnections();
        }

        public void ListenForNewConnections()
        {
            listener.Start();

            while (true)
            {
                NetworkStream stream = listener.AcceptTcpClient().GetStream();
                streams.Add(stream);
                Console.WriteLine("connected");
                new Thread(() => Listen(stream)).Start();
            }
        }

        public void Listen(NetworkStream stream)
        {
            byte[] bytes = new byte[1024];
            Event leEvent = null;
            try
            {
                while (true)
                {
                    stream.Read(bytes, 0, bytes.Length);
                    MemoryStream memStream = new MemoryStream();
                    memStream.Write(bytes, 0, bytes.Length);
                    memStream.Seek(0, SeekOrigin.Begin);
                    leEvent = (Event)new BinaryFormatter().Deserialize(memStream);
                    Broadcast(leEvent);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("something died :( Server=>Listen(NetworkStream)");
            }
        }

        public void Broadcast(Event leEvent)
        {
            MemoryStream ms = new MemoryStream();
            new BinaryFormatter().Serialize(ms, leEvent);

            byte[] bytes = ms.ToArray();

            foreach (NetworkStream ns in streams)
            {
                ns.Write(bytes, 0, bytes.Length);

                ms.Close();
            }
        }

        static void Main(string[] args)
        {
            Server server = new Server();
            server.Run();
        }
    }
}
