using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Objects
{
    [Serializable()]
    public class MoveToGameObjectEvent : Event
    {
        public Guid GoalGameObject;

        public MoveToGameObjectEvent(Guid id, Guid goalGameObject)
        {
            this.Type = 1;
            this.Id = id;
            this.GoalGameObject = goalGameObject;
        }

        public override byte[] Serialize()
        {
            byte[] typeBytes = BitConverter.GetBytes(Type);
            byte[] idBytes = Id.ToByteArray();
            byte[] idDestinationBytes = GoalGameObject.ToByteArray();
            byte[] bytes = new byte[typeBytes.Length + idBytes.Length + idDestinationBytes.Length];

            Buffer.BlockCopy(typeBytes, 0, bytes, 0, 4);
            Buffer.BlockCopy(idBytes, 0, bytes, 4, 16);
            Buffer.BlockCopy(idDestinationBytes, 0, bytes, 20, 16);

            return bytes;
        }

        public static MoveToGameObjectEvent Deserialize(byte[] bytes)
        {
            byte[] idBytes = new byte[16];
            byte[] idDestinationBytes = new byte[16];

            Buffer.BlockCopy(bytes, 4, idBytes, 0, 16);
            Buffer.BlockCopy(bytes, 20, idDestinationBytes, 0, 16);

            return new MoveToGameObjectEvent(new Guid(idBytes), new Guid(idDestinationBytes));
        }
    }
}
