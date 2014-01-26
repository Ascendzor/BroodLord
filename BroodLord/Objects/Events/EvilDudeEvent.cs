using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Objects
{
    public class EvildeDudeEvent : Event
    {
        public EvildeDudeEvent(Guid Id)
        {
            this.Type = 8;
            this.Id = Id;
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

        public static EvildeDudeEvent Deserialize(byte[] bytes)
        {
            byte[] idBytes = new byte[16];
            byte[] idDestinationBytes = new byte[16];

            Buffer.BlockCopy(bytes, 4, idBytes, 0, 16);
            Buffer.BlockCopy(bytes, 20, idDestinationBytes, 0, 16);

            return new EvildeDudeEvent(new Guid(idBytes));
        }
    }
}
