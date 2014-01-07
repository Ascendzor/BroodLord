﻿using System;
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

        public static void ReceiveEvent()
        {
            stream = client.GetStream();
            byte[] bytes = new byte[9001];
            Event leEvent = null;
            try
            {
                //first thing to receive is the data of the map
                stream.Read(bytes, 0, bytes.Length);
                MemoryStream mStream = new MemoryStream();
                mStream.Write(bytes, 0, bytes.Length);
                mStream.Seek(0, SeekOrigin.Begin);
                List<GameObject> gos = (List<GameObject>)new BinaryFormatter().Deserialize(mStream);
                foreach (GameObject go in gos)
                {
                    Data.AddGameObject(go);
                }

                bytes = new byte[1024];

                while (true)
                {
                    stream.Read(bytes, 0, bytes.Length);
                    MemoryStream memStream = new MemoryStream();
                    memStream.Write(bytes, 0, bytes.Length);
                    memStream.Seek(0, SeekOrigin.Begin);
                    leEvent = (Event)new BinaryFormatter().Deserialize(memStream);

                    if (!Data.FindGameObject.ContainsKey(leEvent.Id))
                    {
                        new Toon(leEvent.Id, new Microsoft.Xna.Framework.Vector2(100, 100), "link");
                    }

                    dynamic gameObject = Convert.ChangeType(leEvent, leEvent.GetType());
                    Data.FindGameObject[leEvent.Id].ReceiveEvent(gameObject);
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
                new BinaryFormatter().Serialize(ms, leEvent);
                byte[] bytes = ms.ToArray();
                stream.Write(bytes, 0, bytes.Length);
                ms.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("something died :( Client=>SendEvent(Event)");
            }
        }
    }
}
