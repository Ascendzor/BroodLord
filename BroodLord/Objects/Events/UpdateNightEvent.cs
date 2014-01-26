using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Objects
{
    public class UpdateNightEvent : Event
    {
        public int Nightness;

        public UpdateNightEvent(int nightness)
        {
            this.Type = 11;
            this.Nightness = nightness;
        }

        public override byte[] Serialize()
        {
            byte[] typeBytes = BitConverter.GetBytes(Type);
            byte[] nightness = BitConverter.GetBytes(Nightness);
            byte[] bytes = new byte[8];
            Buffer.BlockCopy(typeBytes, 0, bytes, 0, 4);
            Buffer.BlockCopy(nightness, 0, bytes, 4, 4);
            return bytes;
        }

        public static UpdateNightEvent Deserialize(byte[] bytes)
        {
            byte[] nightness = new byte[4];

            Buffer.BlockCopy(bytes, 4, nightness, 0, 4);

            return new UpdateNightEvent(BitConverter.ToInt16(nightness, 0));
        }
    }
}
