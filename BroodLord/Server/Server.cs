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
            for (int i = 0; i < gos.Count;)
            {
                List<GameObject> goBuffer = new List<GameObject>();
                for (int x = 0; x <= 1250; x++)
                {
                    goBuffer.Add(gos[i]);
                    i++;
                    if (i == gos.Count)
                    {
                        break;
                    }
                }

                MemoryStream ms = new MemoryStream();
                new BinaryFormatter().Serialize(ms, goBuffer);
                byte[] allGameData = ms.ToArray();
                leInt = allGameData.Length;
                Console.WriteLine("leInt: " + leInt);
                data = BitConverter.GetBytes(leInt);
                stream.Write(data, 0, data.Length);
                Thread.Sleep(200);

                stream.Write(allGameData, 0, allGameData.Length);
                Thread.Sleep(500);
                if (i == gos.Count)
                {
                    break;
                }
            }

            leInt = -1;
            data = BitConverter.GetBytes(leInt);
            stream.Write(data, 0, data.Length);
        }

        private void SendTileTextures(NetworkStream stream)
        {
            Console.WriteLine("About to send tiles");
            List<string> terrainTextures = Map.GetTilesTextureKeys();

            byte[] data;
            Int32 leInt;
            for (int x = 0; x < terrainTextures.Count;)
            {
                List<string> tileBuffer = new List<string>();

                for (int counter = 0; counter <= 34000; counter++)
                {
                    tileBuffer.Add(terrainTextures[x]);
                    x++;
                    if (x == terrainTextures.Count)
                    {
                        break;
                    }
                }
                
                MemoryStream ms = new MemoryStream();
                new BinaryFormatter().Serialize(ms, tileBuffer);
                byte[] allGameData = ms.ToArray();
                leInt = allGameData.Length;
                data = BitConverter.GetBytes(leInt);
                stream.Write(data, 0, data.Length);
                Thread.Sleep(200);

                stream.Write(allGameData, 0, allGameData.Length);
                Thread.Sleep(500);
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

        public void MakeEvilDude()
        {
            List<Toon> dudes = new List<Toon>();
            foreach(Mob mob in Map.GetMobs())
            {
                if(mob is Toon)
                {
                    dudes.Add((Toon)mob);
                }
            }
            int leEvilDude = new Random().Next(0, dudes.Count);
            Console.WriteLine("you're evil: " + dudes[leEvilDude].GetId());
            Client.SendEvent(new EvilDudeEvent(dudes[leEvilDude].GetId()));
        }

        static void Main(string[] args)
        {
            Data.Initialize();
            Environment environment = new Environment();

            Server server = new Server();
            new Thread(() => server.ListenForNewConnections()).Start();

            Client.Initialize();

            Thread.Sleep(10000);
            server.MakeEvilDude();
            environment.Play();

        }
    }
}
