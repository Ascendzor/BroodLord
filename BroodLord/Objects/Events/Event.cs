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
            else if (theType == 5)
            {
                leEvent = DroppedItemEvent.Deserialize(bytes);
            }
            else if (theType == 6)
            {
                leEvent = SpawnMeatEvent.Deserialize(bytes);
            }
            else if (theType == 7)
            {
                leEvent = DeathEvent.Deserialize(bytes);
            }
            else if (theType == 8)
            {
                leEvent = DestroyItemEvent.Deserialize(bytes);
            }
            else if (theType == 9)
            {
                leEvent = SpawnCoconutEvent.Deserialize(bytes);
            }
            else if (theType == 10)
            {
                leEvent = EvilDudeEvent.Deserialize(bytes);
            }

            return leEvent;
        }
    
      
    }
}