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

        public void ListenForNewConnections()
        {
            listener.Start();

            while (true)
            {
                NetworkStream stream = listener.AcceptTcpClient().GetStream();
                Console.WriteLine("found client");

                //first thing is to send the map data to the new client
                Console.WriteLine("Waiting to make sure the client is ready for the data");
                Thread.Sleep(5000);
                SendData(stream);
                Console.WriteLine("data sent");
                streams.Add(stream);

                new Thread(() => Listen(stream)).Start();
            }
        }

        private void SendData(NetworkStream stream)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                new BinaryFormatter().Serialize(ms, Data.FindGameObject.Values.ToList());

                byte[] bytes = ms.ToArray();

                stream.Write(bytes, 0, bytes.Length);

                ms.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Server -> SendData(NetworkStream)");
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
            //Environment is the Servers player ;)
            Environment environment = new Environment();

            Server server = new Server();
            new Thread(() => server.ListenForNewConnections()).Start();

            environment.Play();
        }
    }
}
