﻿using System;
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

        /// <summary>
        /// accepts new clients and adds them the network streams list
        /// </summary>
        public void ListenForNewConnections()
        {
            listener.Start();

            while (true)
            {
                NetworkStream stream = listener.AcceptTcpClient().GetStream();
                Console.WriteLine("found client");
                //first thing is to send the map data to the new client
                SendData(stream);
                Console.WriteLine("client connected");
                streams.Add(stream);
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
                //TODO refactor this so two stream.writes are done, first send a stream with the size of the following stream write

                // write all the game data to a by
                MemoryStream ms = new MemoryStream();
                new BinaryFormatter().Serialize(ms, Data.FindGameObject.Values.ToList());
                byte[] allGameData = ms.ToArray();


                // Send a message to the server to tell it the size of the next Message with all game objects
                ms = new MemoryStream();
                new BinaryFormatter().Serialize(ms, new GameDataSizeMessage( allGameData.Length ));
                byte[] dataSizeMessage = ms.ToArray();
                stream.Write(dataSizeMessage, 0, dataSizeMessage.Length);
                Console.WriteLine("Sent the data size message {0} long", dataSizeMessage.Length);

                //now send the game data
                stream.Write(allGameData, 0, allGameData.Length);
                Console.WriteLine("Sent the game data {0} bytes long", allGameData.Length);
                ms.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Server > SendData(NetworkStream)");
            }
        }

        /// <summary>
        /// Broadcast to clients continuously
        /// </summary>
        /// <param name="stream"> stream which is associated with a client</param>
        public void Listen(NetworkStream stream)
        {
            byte[] bytes = new byte[512];
            try
            {
                while (true)
                {
                    stream.Read(bytes, 0, bytes.Length);
                    MemoryStream memStream = new MemoryStream();
                    memStream.Write(bytes, 0, bytes.Length);
                    memStream.Seek(0, SeekOrigin.Begin);

                    BroadcastEvent((Event)new BinaryFormatter().Deserialize(memStream));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("something died :( Server=>Listen(NetworkStream)");
            }
        }

        //broadcasts an event
        public void BroadcastEvent(Event leEvent)
        {
            MemoryStream ms = new MemoryStream();
            new BinaryFormatter().Serialize(ms, leEvent);
            byte[] bytes = ms.ToArray();
            ms.Close();
            foreach (NetworkStream ns in streams)
            {
                ns.Write(bytes, 0, bytes.Length);
            }
        }

        static void Main(string[] args)
        {
            //Environment is the Servers player ;)
            Environment environment = new Environment();

            Server server = new Server();
            new Thread(() => server.ListenForNewConnections()).Start();

            Client.Initialize();

            //wait 10 seconds because I scared -Troy
            Thread.Sleep(5000);
            environment.Play();
        }
    }
}
