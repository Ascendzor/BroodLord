using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Objects
{
    [Serializable()]
    public class ChopEvent : Event
    {
        public ChopEvent(Guid id)
        {
            this.Type = 0;
            this.Id = id;
        }

        public override byte[] Serialize()
        {
            byte[] typeBytes = BitConverter.GetBytes(Type);
            byte[] idBytes = Id.ToByteArray();
            byte[] bytes = new byte[typeBytes.Length + idBytes.Length];

            Buffer.BlockCopy(typeBytes, 0, bytes, 0, 4);
            Buffer.BlockCopy(idBytes, 0, bytes, 4, 16);

            return bytes;
        }

        public static ChopEvent Deserialize(byte[] bytes)
        {
            byte[] idBytes = new byte[16];

            Buffer.BlockCopy(bytes, 4, idBytes, 0, 16);

            return new ChopEvent(new Guid(idBytes));
        }
    }
}