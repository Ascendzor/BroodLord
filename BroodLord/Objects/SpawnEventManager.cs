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
            new Toon(leEvent.Id, new Vector2(100, 100), "link");
        }

        public static void HandleEvent(SpawnWoodEvent leEvent)
        {
            new Wood(leEvent.Id, leEvent.Position);
        }
    }
}
