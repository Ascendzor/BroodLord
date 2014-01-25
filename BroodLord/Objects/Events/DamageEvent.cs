using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Objects
{
    public class DamageEvent : Event
    {
        public int damage;
        public DamageEvent(Guid Id , int damage)
        {
            this.Type =13;
            this.Id = Id;
            this.damage = damage;
        }

        public override byte[] Serialize()
        {
            byte[] typeBytes = BitConverter.GetBytes(Type);
            byte[] idBytes = Id.ToByteArray();
            byte[] damageBytes = BitConverter.GetBytes(damage);
            byte[] bytes = new byte[typeBytes.Length + idBytes.Length + damageBytes.Length];

            Buffer.BlockCopy(typeBytes, 0, bytes, 0, 4);
            Buffer.BlockCopy(idBytes, 0, bytes, 4, 16);

            return bytes;
        }

        public static DamageEvent Deserialize(byte[] bytes)
        {
            byte[] idBytes = new byte[16];
            byte[] idDestinationBytes = new byte[16];
            byte[] damageBytes = new byte[4];

            Buffer.BlockCopy(bytes, 4, idBytes, 0, 16);
            Buffer.BlockCopy(bytes, 20, idDestinationBytes, 0, 16);
            Buffer.BlockCopy(bytes, 36, damageBytes, 0, 4);

            return new DamageEvent(new Guid(idBytes), BitConverter.ToInt32(damageBytes,0));
        }
    }
}
