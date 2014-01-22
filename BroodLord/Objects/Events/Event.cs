using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Objects
{
    [Serializable()]
    public class Event
    {
        public int Type;
        public Guid Id;

        public virtual byte[] Serialize()
        {
            return null;
        }

        public static Event Deserialize(byte[] bytes)
        {
            byte[] typeBytes = new byte[4];
            Buffer.BlockCopy(bytes, 0, typeBytes, 0, 4);

            int theType = BitConverter.ToInt16(typeBytes, 0);
            Event leEvent = null;
            if (theType == 0)
            {
                leEvent = MoveToPositionEvent.Deserialize(bytes);
            }
            else if (theType == 1)
            {
                leEvent = MoveToGameObjectEvent.Deserialize(bytes);
            }
            else if (theType == 2)
            {
                leEvent = SpawnToonEvent.Deserialize(bytes);
            }
            else if (theType == 3)
            {
                leEvent = SpawnWoodEvent.Deserialize(bytes);
            }
            else if (theType == 4)
            {
                leEvent = SpawnRockEvent.Deserialize(bytes);
            }

            return leEvent;
        }
    }
}