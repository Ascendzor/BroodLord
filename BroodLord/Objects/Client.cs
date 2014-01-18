using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace Objects
{
    [Serializable()]
    public class Client
    {
        private static int port;
        private static int outputPort;
        private static Connection server;
        private static TcpClient client;
        private static TcpListener serverListener;

        public static void Initialize()
        {
            // port which the server will open a stream on
            outputPort = 41338;
            serverListener = new TcpListener(outputPort);
            // server port
            port = 41337;

            // send a connectionmessage to the server
            client = new TcpClient("192.168.1.100", port); //server address here
            NetworkStream outStream = client.GetStream();

            // accept connection from server for input
            serverListener.Start();
            TcpClient serverClient = serverListener.AcceptTcpClient();
            NetworkStream inStream = serverClient.GetStream();


            //have in and outstream on server so start network event handler
            server = new Connection(inStream, outStream);
            new Thread(()=>receiveEvent(server.InStream)).Start();
            Console.WriteLine("connected to the server");
        }


        /// <summary>
        /// recieve the map from the client
        /// note: recieve
        /// </summary>
        /// <param name="stream"></param>
        private static void recieveMap(NetworkStream stream)
        {
            MemoryStream mStream = new MemoryStream();
            // recieve a datamessage so we know how much to read from the network stream
            int sizeofmap = Net.RecieveDataSizeMessage(stream).SizeOfData;
            byte[] messageData = new byte[sizeofmap];
            stream.Read(messageData, 0, messageData.Length);
            mStream.Write(messageData, 0, messageData.Length);
            mStream.Seek(0, SeekOrigin.Begin);
            List<GameObject> gos = (List<GameObject>)new BinaryFormatter().Deserialize(mStream);

            // Update the game objects
            foreach (GameObject go in gos)
            {
                Data.AddGameObject(go);
            }
        }
        /// <summary>
        /// Update event that is run in a thread
        /// </summary>
        private static void receiveEvent( NetworkStream inStream )
        {
            Console.WriteLine("Should be recieving events");
            Event leEvent = null;
            try
            {
                recieveMap(inStream);
                Console.WriteLine("map recieved");

                while (true)
                {
                    GameDataSizeMessage dataMessage = Net.RecieveDataSizeMessage(inStream);

                    byte[] bytes = new byte[dataMessage.SizeOfData];

                    inStream.Read(bytes, 0, bytes.Length);
                    MemoryStream memStream = new MemoryStream();
                    memStream.Write(bytes, 0, bytes.Length);
                    memStream.Seek(0, SeekOrigin.Begin);
                    leEvent = (Event)new BinaryFormatter().Deserialize(memStream);
                    memStream.Close();

                    if (Data.FindGameObject.ContainsKey(leEvent.Id))
                    {
                        dynamic dynamicEvent = Convert.ChangeType(leEvent, leEvent.GetType());
                        dynamic gameObject = Data.FindGameObject[leEvent.Id];
                        gameObject.ReceiveEvent(dynamicEvent);
                    }
                    else
                    {
                        Console.WriteLine("event: " + leEvent.GetType());
                        dynamic dynamicEvent = Convert.ChangeType(leEvent, leEvent.GetType());
                        SpawnEventManager.HandleEvent(dynamicEvent);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("something died :( Client=>ReceiveEvent)");
                throw e;
            }
        }


        public static void SendEvent(Event leEvent)
        {

            MemoryStream ms = new MemoryStream();
            try
            {
                Console.WriteLine("sent: "  + leEvent.GetType());
                new BinaryFormatter().Serialize(ms, leEvent);
                byte[] bytes = ms.ToArray();
                Console.WriteLine(bytes.Length);
                server.OutStream.Write(bytes, 0, bytes.Length);
                ms.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("something died :( Client=>SendEvent(Event)");
            }
        }
    }
}
