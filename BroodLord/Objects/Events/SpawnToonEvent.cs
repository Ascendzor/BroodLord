using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Objects
{
    [Serializable()]
    public class SpawnToonEvent : Event
    {
        public SpawnToonEvent(Guid id)
        {
            this.Type = 2;
            this.Id = id;
        }

        public override byte[] Serialize()
        {
            byte[] typeBytes = BitConverter.GetBytes(Type);
            byte[] idBytes = Id.ToByteArray();

            byte[] bytes = new byte[typeBytes.Length + idBytes.Length];

            Buffer.BlockCopy(typeBytes, 0, bytes, 0, typeBytes.Length);
            Buffer.BlockCopy(idBytes, 0, bytes, typeBytes.Length, idBytes.Length);
            return bytes;
        }

        public static SpawnToonEvent Deserialize(byte[] bytes)
        {
            byte[] idBytes = new byte[16];
            Buffer.BlockCopy(bytes, 4, idBytes, 0, 16);

            return new SpawnToonEvent(new Guid(idBytes));
        }
    }
}
