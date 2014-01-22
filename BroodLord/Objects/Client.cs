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
        private static TcpClient client;
        private static TcpClient otherClient;
        private static NetworkStream stream;
        private static NetworkStream otherStream;
        private static Queue<Event> outgoingEvents;

        public static void Initialize()
        {
            port = 41337;
            client = new TcpClient("127.0.0.1", port);
            otherClient = new TcpClient("127.0.0.1", 41338);
            outgoingEvents = new Queue<Event>();

            new Thread(ReceiveEvent).Start();
            new Thread(ShaveOutgoingQueue).Start();
        }

        public static NetworkStream getStream()
        {
            return stream;
        }

        /// <summary>
        /// Update event that is run in a thread
        /// </summary>
        private static void ReceiveEvent()
        {
            stream = client.GetStream();
            otherStream = otherClient.GetStream();

            try
            {
                // read the GameDataSizeMessage
                MemoryStream mStream = new MemoryStream();
                byte[] messageData =  new byte[Data.SizeOfNetEventPacket];
                Console.WriteLine("Recieveing {0}", messageData.Length);
                otherStream.Read(messageData, 0, messageData.Length);
                mStream.Write(messageData, 0, messageData.Length);
                mStream.Seek(0, SeekOrigin.Begin);
                GameDataSizeMessage dataMessage = (GameDataSizeMessage)new BinaryFormatter().Deserialize(mStream);

                // read the list of game objects
                mStream = new MemoryStream();
                messageData = new byte[dataMessage.sizeOfData];
                otherStream.Read(messageData, 0, messageData.Length);
                mStream.Write(messageData, 0, messageData.Length);
                mStream.Seek(0, SeekOrigin.Begin);
                List<GameObject> gos = (List<GameObject>)new BinaryFormatter().Deserialize(mStream);
                Console.WriteLine(gos.Count);
                Console.WriteLine("adding game objects from server");
                // Update the game objects
                foreach (GameObject go in gos)
                {
                    Map.InsertGameObject(go);
                    Console.WriteLine("added: " + go);
                }

                byte[] bytes = new byte[128];
                while (true)
                {
                    otherStream.Read(bytes, 0, bytes.Length);
                    dynamic leEvent = Event.Deserialize(bytes);

                    if (!leEvent.Id.Equals(Guid.Empty))
                    {
                        EventManager.HandleEvent(leEvent);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("something died :( Client=>ReceiveEvent)");
            }
        }

        public static void SendEvent(Event leEvent)
        {
            MemoryStream ms = new MemoryStream();
            try
            {
                outgoingEvents.Enqueue(leEvent);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("something died :( Client=>SendEvent(Event)");
            }
        }

        public static void ShaveOutgoingQueue()
        {
            while (true)
            {
                Event leEvent = null;
                while (outgoingEvents.Count > 0)
                {
                    leEvent = outgoingEvents.Dequeue();
                    byte[] bytes = leEvent.Serialize();
                    stream.Write(bytes, 0, bytes.Length);
                    Thread.Sleep(30);
                }
            }
        }
    }
}
