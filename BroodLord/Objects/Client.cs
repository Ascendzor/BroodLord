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
        private static NetworkStream stream;

        public static void Initialize()
        {
            port = 41337;
            client = new TcpClient("127.0.0.1", port);

            new Thread(ReceiveEvent).Start();
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

            Event leEvent = null;
            try
            {
                // read the GameDataSizeMessage
                MemoryStream mStream = new MemoryStream();
                byte[] messageData =  new byte[Data.SizeOfNetEventPacket];
                Console.WriteLine("Recieveing {0}", messageData.Length);
                stream.Read(messageData, 0, messageData.Length);
                mStream.Write(messageData, 0, messageData.Length);
                mStream.Seek(0, SeekOrigin.Begin);
                GameDataSizeMessage dataMessage = (GameDataSizeMessage)new BinaryFormatter().Deserialize(mStream);

                // read the list of game objects
                mStream = new MemoryStream();
                messageData = new byte[dataMessage.sizeOfData];
                stream.Read(messageData, 0, messageData.Length);
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

                byte[] bytes = new byte[512];
                while (true)
                {
                    stream.Read(bytes, 0, bytes.Length);
                    Console.WriteLine("Received an event");
                    MemoryStream memStream = new MemoryStream();
                    memStream.Write(bytes, 0, bytes.Length);
                    memStream.Seek(0, SeekOrigin.Begin);
                    leEvent = (Event)new BinaryFormatter().Deserialize(memStream);
                    
                    //This is specific to spawning events, if we change SpawnEventManager to GlobalEventManager this will have to change -Troy
                    GameObject receiver = Map.GetGameObject(leEvent.Id);
                    if (receiver != null) 
                    {
                        dynamic dynamicEvent = Convert.ChangeType(leEvent, leEvent.GetType());
                        dynamic gameObject = receiver;
                        gameObject.ReceiveEvent(dynamicEvent);
                    }
                    else
                    {
                        dynamic dynamicEvent = Convert.ChangeType(leEvent, leEvent.GetType());
                        SpawnEventManager.HandleEvent(dynamicEvent);
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
                Console.WriteLine(leEvent.GetType());
                new BinaryFormatter().Serialize(ms, leEvent);
                byte[] bytes = ms.ToArray();
                Console.WriteLine(bytes.Length);
                stream.Write(bytes, 0, bytes.Length);
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
