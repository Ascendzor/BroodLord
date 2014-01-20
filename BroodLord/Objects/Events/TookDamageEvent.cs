using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Objects
{
    [Serializable()]
    public class TookDamageEvent : Event
    {
        public double DamageTaken;

        public TookDamageEvent(Guid id, double damageTaken)
        {
            this.Type = 6;
            this.Id = id;
            this.DamageTaken = damageTaken;
        }

        public override byte[] Serialize()
        {
            byte[] typeBytes = BitConverter.GetBytes(Type);
            byte[] idBytes = Id.ToByteArray();
            byte[] damageTaken = BitConverter.GetBytes(DamageTaken);
            byte[] bytes = new byte[typeBytes.Length + idBytes.Length + damageTaken.Length];

            Buffer.BlockCopy(typeBytes, 0, bytes, 0, 4);
            Buffer.BlockCopy(idBytes, 0, bytes, 4, 16);
            Buffer.BlockCopy(damageTaken, 0, bytes, 20, 8);

            return bytes;
        }

        public static TookDamageEvent Deserialize(byte[] bytes)
        {
            byte[] idBytes = new byte[16];
            byte[] damageTaken = new byte[8];

            Buffer.BlockCopy(bytes, 4, idBytes, 0, 16);
            Buffer.BlockCopy(bytes, 20, damageTaken, 0, 8);

            return new TookDamageEvent(new Guid(idBytes), BitConverter.ToDouble(damageTaken, 0));
        }
    }
}
