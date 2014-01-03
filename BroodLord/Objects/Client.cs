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
    public class Client
    {
        int port;
        private TcpClient client;
        private NetworkStream stream;
        private Map Map;

        public Client(Map Map)
        {
            port = 41337;
            client = new TcpClient("127.0.0.1", port);

            new Thread(ReceiveEvent).Start();

            this.Map = Map;
        }

        public void ReceiveEvent()
        {
            stream = client.GetStream();
            byte[] bytes = new byte[1024];
            Event leEvent = null;
            try
            {
                while (true)
                {
                    stream.Read(bytes, 0, bytes.Length);
                    MemoryStream memStream = new MemoryStream();
                    memStream.Write(bytes, 0, bytes.Length);
                    memStream.Seek(0, SeekOrigin.Begin);
                    leEvent = (Event)new BinaryFormatter().Deserialize(memStream);

                    if (!Data.FindGameObject.ContainsKey(leEvent.Id))
                    {
                        new Toon(Guid.NewGuid(), new Microsoft.Xna.Framework.Vector2(100, 100), "link", Map, this);
                    }
                    Data.FindGameObject[leEvent.Id].ReceiveEvent(leEvent);
                }
            }
            catch (Exception)
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
                ms.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("something died :( Client=>SendEvent(Event)");
            }
        }
    }
}
