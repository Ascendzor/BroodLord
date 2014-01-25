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
            //client = new TcpClient("127.0.0.1", port);
            //otherClient = new TcpClient("127.0.0.1", 41338);
            string leIp = "127.0.0.1";
            client = new TcpClient(leIp, port);
            otherClient = new TcpClient(leIp, 41338);
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
                ReceiveGameObjects();
                ReceiveTileTextures();

                byte[] bytes = new byte[128];
                while (true)
                {
                    otherStream.Read(bytes, 0, bytes.Length);
                    dynamic leEvent = Event.Deserialize(bytes);
                    if (leEvent != null && !leEvent.Id.Equals(Guid.Empty))
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

        private static void ReceiveGameObjects()
        {
            BinaryFormatter bf = new BinaryFormatter();

            byte[] messageData = new byte[32];
            Int32 leSize;
            List<GameObject> allObjectsGiven = new List<GameObject>();
            while (true)
            {
                otherStream.Read(messageData, 0, 32);
                leSize = BitConverter.ToInt32(messageData, 0);

                Console.WriteLine(leSize);
                if (leSize == -1)
                {
                    break;
                }

                MemoryStream stream = new MemoryStream();
                messageData = new byte[leSize];
                otherStream.Read(messageData, 0, messageData.Length);
                Console.WriteLine("incoming object size: " + messageData.Length);
                stream.Write(messageData, 0, messageData.Length);
                stream.Position = 0;
                List<GameObject> goz = (List<GameObject>)bf.Deserialize(stream);
                foreach (GameObject go in goz)
                {
                    Map.InsertGameObject(go);
                }
            }
            return;
        }

        private static void ReceiveTileTextures()
        {
            BinaryFormatter bf = new BinaryFormatter();

            byte[] messageData = new byte[32];
            Int32 leSize;
            List<string> textureKeys = new List<string>();
            while (true)
            {
                messageData = new byte[32];
                otherStream.Read(messageData, 0, 32);
                leSize = BitConverter.ToInt32(messageData, 0);
                if (leSize == -1)
                {
                    break;
                } 

                MemoryStream stream = new MemoryStream();
                messageData = new byte[leSize];
                otherStream.Read(messageData, 0, messageData.Length);
                stream.Write(messageData, 0, messageData.Length);
                stream.Position = 0;
                List<string> textureKeyBatch = (List<string>)bf.Deserialize(stream);
                foreach (string textureKey in textureKeyBatch)
                {
                    textureKeys.Add(textureKey);
                }
            }
            Map.SetTilesTextureKeys(textureKeys);
            return;
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
