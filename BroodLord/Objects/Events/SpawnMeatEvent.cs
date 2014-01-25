using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Objects
{
    [Serializable()]
    public class SpawnMeatEvent : Event
    {
        public Guid MeatId;
        public Vector2 Position;

        public SpawnMeatEvent(Guid id, Vector2 position)
        {
            this.Type = 6;
            this.Id = id;
            this.Position = position;
        }

        public override byte[] Serialize()
        {  
            byte[] typeBytes = BitConverter.GetBytes(Type);
            byte[] idBytes = Id.ToByteArray();
            byte[] positionX = BitConverter.GetBytes(Position.X);
            byte[] positionY = BitConverter.GetBytes(Position.Y);
            byte[] bytes = new byte[typeBytes.Length + idBytes.Length + positionX.Length + positionY.Length];

            Buffer.BlockCopy(typeBytes, 0, bytes, 0, 4);
            Buffer.BlockCopy(idBytes, 0, bytes, 4, 16);
            Buffer.BlockCopy(positionX, 0, bytes, 20, 4);
            Buffer.BlockCopy(positionY, 0, bytes, 24, 4);

            return bytes;
        }

        public static SpawnMeatEvent Deserialize(byte[] bytes)
        {
            byte[] idBytes = new byte[16];
            byte[] positionX = new byte[4];
            byte[] positionY = new byte[4];

            Buffer.BlockCopy(bytes, 4, idBytes, 0, 16);
            Buffer.BlockCopy(bytes, 20, positionX, 0, 4);
            Buffer.BlockCopy(bytes, 24, positionY, 0, 4);

            float x = BitConverter.ToSingle(positionX, 0);
            float y = BitConverter.ToSingle(positionY, 0);

            return new SpawnMeatEvent(new Guid(idBytes), new Vector2(x, y));
        }
    }
}
