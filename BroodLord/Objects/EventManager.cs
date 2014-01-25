using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Objects
{
    [Serializable()]
    public class EventManager
    {
        public static void HandleEvent(Event leEvent)
        {
            dynamic go = Map.GetGameObject(leEvent.Id);
            if (go != null)
            {
                dynamic specificEvent = Convert.ChangeType(leEvent, leEvent.GetType());
                Console.WriteLine(leEvent);
                go.ReceiveEvent(specificEvent);
            }
        }

        public static void HandleEvent(SpawnToonEvent leEvent)
        {
            Console.WriteLine("new toon event");
            new Toon(leEvent.Id, new Vector2(100, 100), "link");
        }

        public static void HandleEvent(SpawnWoodEvent leEvent)
        {
            new WoodLoot(leEvent.Id, leEvent.Position);
        }

        public static void HandleEvent(SpawnRockEvent leEvent)
        {
            new RockLoot(leEvent.Id, leEvent.Position);
        }

        public static void HandleEvent(SpawnMeatEvent leEvent)
        {
            new MeatLoot(leEvent.Id, leEvent.Position);
        }

        public static void HandleEvent(SpawnCoconutEvent leEvent)
        {
            new CoconutLoot(leEvent.Id, leEvent.Position);
        }
    }
}
