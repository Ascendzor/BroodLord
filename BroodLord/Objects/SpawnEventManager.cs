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
        public static void HandleEvent(SpawnToonEvent leEvent)
        {
            Console.WriteLine("new toon event");
            Console.WriteLine(leEvent);
            try
            {
                new Toon(leEvent.Id, new Vector2(100, 100), "link");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public static void HandleEvent(SpawnWoodEvent leEvent)
        {
            new Wood(leEvent.Id, leEvent.Position);
        }
    }
}
