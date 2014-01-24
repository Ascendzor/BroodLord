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
        private TcpListener otherListener;

        public Server()
        {
            port = 41337;
            listener = new TcpListener(port);
            otherListener = new TcpListener(41338);
            streams = new List<NetworkStream>();
        }

        /// <summary>
        /// accepts new clients and adds them the network streams list
        /// </summary>
        public void ListenForNewConnections()
        {
            listener.Start();
            otherListener.Start();

            while (true)
            {
                NetworkStream stream = listener.AcceptTcpClient().GetStream();
                NetworkStream otherStream = otherListener.AcceptTcpClient().GetStream();
                Console.WriteLine("found client");
                //first thing is to send the map data to the new client
                SendData(otherStream);
                Console.WriteLine("client connected");
                streams.Add(otherStream);
                // spawn a listening thread for the stream
                new Thread(() => Listen(stream)).Start();
            }
        }

        /// <summary>
        /// Send game data to the client.
        /// </summary>
        /// <param name="stream"> The network stream associated with the client </param>
        private void SendData(NetworkStream stream)
        {
            try
            {
                SendGameObjects(stream);
                SendTileTextures(stream);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Server > SendData(NetworkStream)");
            }
        }

        private void SendGameObjects(NetworkStream stream)
        {
            List<GameObject> gos = Map.GetGameObjects();

            byte[] data;
            Int32 leInt;
            foreach (GameObject go in gos)
            {
                MemoryStream ms = new MemoryStream();
                new BinaryFormatter().Serialize(ms, go);
                byte[] allGameData = ms.ToArray();
                leInt = allGameData.Length;
                data = BitConverter.GetBytes(leInt);
                stream.Write(data, 0, data.Length);
                Console.WriteLine("sending: " + go);
                Thread.Sleep(5);

                stream.Write(allGameData, 0, allGameData.Length);
                Thread.Sleep(5);
            }

            leInt = -1;
            data = BitConverter.GetBytes(leInt);
            stream.Write(data, 0, data.Length);
        }

        private void SendTileTextures(NetworkStream stream)
        {
            Console.WriteLine("About to send tiles");
            Thread.Sleep(2000);
            List<string> terrainTextures = Map.GetTilesTextureKeys();

            byte[] data;
            Int32 leInt;
            foreach (string terrainTextureKey in terrainTextures)
            {
                MemoryStream ms = new MemoryStream();
                new BinaryFormatter().Serialize(ms, terrainTextureKey);
                byte[] allGameData = ms.ToArray();
                leInt = allGameData.Length;
                data = BitConverter.GetBytes(leInt);
                stream.Write(data, 0, data.Length);
                Console.WriteLine("sending: " + terrainTextureKey);
                Thread.Sleep(10);

                Console.WriteLine("allDataLength: " + allGameData.Length);
                stream.Write(allGameData, 0, allGameData.Length);
                Thread.Sleep(10);
            }

            leInt = -1;
            data = BitConverter.GetBytes(leInt);
            stream.Write(data, 0, data.Length);
            Console.WriteLine("sent the killer of tiles");
        }

        /// <summary>
        /// Broadcast to clients continuously
        /// </summary>
        /// <param name="stream"> stream which is associated with a client</param>
        public void Listen(NetworkStream stream)
        {
            byte[] bytes = new byte[128];
            try
            {
                while (true)
                {
                    stream.Read(bytes, 0, bytes.Length);
                    foreach (NetworkStream ns in streams)
                    {
                        ns.Write(bytes, 0, bytes.Length);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("something died :( Server=>Listen(NetworkStream)");
            }
        }

        static void Main(string[] args)
        {
            Data.Initialize();
            Environment environment = new Environment();

            Server server = new Server();
            new Thread(() => server.ListenForNewConnections()).Start();

            Client.Initialize();

            environment.Play();
        }
    }
}
