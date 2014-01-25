using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Objects
{
    [Serializable()]
    public class DestroyItemEvent : Event
    {
        public Guid ItemId;

        public DestroyItemEvent(Guid id, Guid itemId)
        {
            this.Type = 8;
            this.Id = id;
            this.ItemId = itemId;
        }

        public override byte[] Serialize()
        {
            byte[] typeBytes = BitConverter.GetBytes(Type);
            byte[] idBytes = Id.ToByteArray();
            byte[] idDestinationBytes = ItemId.ToByteArray();
            byte[] bytes = new byte[typeBytes.Length + idBytes.Length + idDestinationBytes.Length];

            Buffer.BlockCopy(typeBytes, 0, bytes, 0, 4);
            Buffer.BlockCopy(idBytes, 0, bytes, 4, 16);
            Buffer.BlockCopy(idDestinationBytes, 0, bytes, 20, 16);

            return bytes;
        }

        public static DestroyItemEvent Deserialize(byte[] bytes)
        {
            byte[] idBytes = new byte[16];
            byte[] idDestinationBytes = new byte[16];

            Buffer.BlockCopy(bytes, 4, idBytes, 0, 16);
            Buffer.BlockCopy(bytes, 20, idDestinationBytes, 0, 16);

            return new DestroyItemEvent(new Guid(idBytes), new Guid(idDestinationBytes));
        }
    }
}
