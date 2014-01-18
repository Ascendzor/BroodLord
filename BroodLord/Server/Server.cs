using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Net;
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
        private NetworkStream broadcastStream;

        private List<Connection> connections;

        public Server()
        {
            port = 41337;

            listener = new TcpListener(port);
            streams = new List<NetworkStream>();
            connections = new List<Connection>();
        }

        /// <summary>
        /// sends a connection message with the specified port number
        /// </summary>
        /// <param name="portNumber"></param>
        /// <param name="stream"></param>
        private void sendConnectionMessage(int portNumber, NetworkStream stream)
        {

        }
        /// <summary>
        /// accepts new clients and adds them the network streams list
        /// </summary>
        public void ListenForNewConnections()
        {
            listener.Start();


            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                //accept a connection for the new stream to broadcast to
                NetworkStream outStream = client.GetStream(); //server broadcastStream
                //get ip address of connection
                IPAddress clientIP = ((IPEndPoint)client.Client.RemoteEndPoint).Address;

                // open a input stream from the client
                TcpClient serverClient = new TcpClient(clientIP.ToString(), 41338);
                NetworkStream inStream = serverClient.GetStream();
    

                //add client to the client connections list.
                connections.Add(new Connection(inStream, outStream));

                //send the map data to the new client
                sendMap(outStream);

                new Thread(()=> listen()).Start();
                Console.WriteLine("client connected from: {0}", clientIP);
            }
        }

        /// <summary>
        /// Send game data to the client.
        /// </summary>
        /// <param name="stream"> The network stream associated with the client </param>
        private void sendMap(NetworkStream stream)
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
                Console.WriteLine("Sent the map {0} bytes long", allGameData.Length);
                ms.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Server > SendData(NetworkStream)");
            }
        }

        /// <summary>
        /// runs continuously in a thread listening to the stream
        /// </summary>
        /// <param name="stream"> stream to listen on</param>
        private void listen()
        {
            try
            {
                while (true)
                {
                    MemoryStream clientBroadcastStream = new MemoryStream();
                    //read events from all clients and push them onto the broadcaststream
                    byte[] messageBytes;
                    foreach (var client in connections)
                    {
                        //read datasizemessage
                        GameDataSizeMessage message = Net.RecieveDataSizeMessage(client.InStream);
                        MemoryStream temp = new MemoryStream();
                        //push the datasize message on the clientbroadcaststream
                        new BinaryFormatter().Serialize(temp, message);
                        temp.WriteTo(clientBroadcastStream);

                        //read next message
                        messageBytes = new byte[message.SizeOfData];
                        client.InStream.Read(messageBytes, 0, message.SizeOfData);
                        //write the whole message to the clientbroadcaststream
                        clientBroadcastStream.Write(messageBytes, 0, messageBytes.Length);
                    }
                    broadcast(clientBroadcastStream);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Broadcast to all clients the contents of outStream
        /// </summary>
        /// <param name="stream"> stream which will be broadcasted to every connection </param>
        private void broadcast(MemoryStream outStream)
        {
            try
            {
                //write the stream to every client
                foreach (var client in connections)
                {
                    outStream.WriteTo(client.OutStream);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("something died :( Server=>Listen(NetworkStream)");
                // this should throw something more informative
                throw;
            }
        }

        static void Main(string[] args)
        {
            Data.Initialize();
            Environment environment = new Environment();

            Server server = new Server();
            new Thread(() => server.ListenForNewConnections()).Start();

            environment.Play();
        }
    }
}
