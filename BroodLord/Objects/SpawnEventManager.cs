using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Objects
{
    [Serializable()]
    public class SpawnEventManager
    {
        public static void HandleEvent(Event leEvent)
        {
            dynamic dynamicEvent = Convert.ChangeType(leEvent, leEvent.GetType());
            dynamic gameObject = Map.GetGameObject(leEvent.Id);
            gameObject.ReceiveEvent(dynamicEvent);
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

        public static void HandleEvent(LootedLootEvent leEvent)
        {
            return;
        }
    }
}
