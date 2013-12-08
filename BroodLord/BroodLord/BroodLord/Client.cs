using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Objects;
using System.Net.Sockets;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace BroodLord
{
    class Client
    {
        int port;
        private TcpClient client;
        private NetworkStream stream;

        public Client()
        {
            port = 41337;
            client = new TcpClient("192.168.1.123", port);
            stream = client.GetStream();

            new Thread(ReceiveEvent).Start();
        }

        public void ReceiveEvent()
        {
            byte[] bytes = new byte[1024];
            Event leEvent = null;
            try
            {
                while (true)
                {
                    stream.Read(bytes, 0, bytes.Length);
                    Console.WriteLine("received event from server");
                    MemoryStream memStream = new MemoryStream();
                    memStream.Write(bytes, 0, bytes.Length);
                    memStream.Seek(0, SeekOrigin.Begin);
                    leEvent = (Event)new BinaryFormatter().Deserialize(memStream);

                    if (!Game1.allToons.ContainsKey(leEvent.id))
                    {
                        Game1.allToons.Add(leEvent.id, new Toon(new Microsoft.Xna.Framework.Vector2(100, 100), "link"));
                    }
                    Game1.allToons[leEvent.id].ReceiveEvent(leEvent);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("something died :( Server=>Listen(NetworkStream)");
            }
        }

        public void SendEvent(Event leEvent)
        {
            MemoryStream ms = new MemoryStream();
            try
            {
                new BinaryFormatter().Serialize(ms, leEvent);

                byte[] bytes = ms.ToArray();
                stream.Write(bytes, 0, bytes.Length);
                Console.WriteLine("wrote");
                ms.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("something died :( Client=>SendEvent(Event)");
            }
        }
    }
}
