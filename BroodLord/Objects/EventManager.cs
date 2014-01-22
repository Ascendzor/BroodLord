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
            dynamic specificEvent = Convert.ChangeType(leEvent, leEvent.GetType());
            Console.WriteLine(leEvent);
            go.ReceiveEvent(specificEvent);
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
    }
}
