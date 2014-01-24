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
                Thread.Sleep(15);

                stream.Write(allGameData, 0, allGameData.Length);
                Thread.Sleep(15);
            }

            leInt = -1;
            data = BitConverter.GetBytes(leInt);
            stream.Write(data, 0, data.Length);
        }

        private void SendTileTextures(NetworkStream stream)
        {
            // write all the game data to a by
            MemoryStream ms = new MemoryStream();
            new BinaryFormatter().Serialize(ms, Map.GetTilesTextureKeys());
            byte[] allGameData = ms.ToArray();


            // Send a message to the server to tell it the size of the next Message with all game objects
            ms = new MemoryStream();
            new BinaryFormatter().Serialize(ms, new GameDataSizeMessage(allGameData.Length));
            byte[] dataSizeMessage = ms.ToArray();
            stream.Write(dataSizeMessage, 0, dataSizeMessage.Length);
            Console.WriteLine("Sent the data size message {0} long", dataSizeMessage.Length);

            //now send the game data
            stream.Write(allGameData, 0, allGameData.Length);

            Console.WriteLine("Sent the game data {0} bytes long", allGameData.Length);
            ms.Close();
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
