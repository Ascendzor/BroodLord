using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Objects
{
    public class Tree : Doodad
    {
        private int health;

        public Tree(Vector2 position, string textureKey, Map map, Client client)
        {
            this.id = Guid.NewGuid();
            this.position = position;
            this.textureKey = textureKey;
            this.map = map;
            this.xTileCoord = (int)position.X / map.GetTileSize();
            this.yTileCoord = (int)position.Y / map.GetTileSize();
            this.collisionWidth = Data.TreeRadius;
            this.origin = new Vector2(Data.FindTexture[textureKey].Width / 2, Data.FindTexture[textureKey].Height * 0.85f);
            this.hitbox = new Rectangle((int)(position.X - origin.X), (int)(position.Y - origin.Y), Data.FindTexture[textureKey].Width, Data.FindTexture[textureKey].Height);
            this.client = client;
            this.health = 999;

            map.GetTile(xTileCoord, yTileCoord).GetObjects().Add(this);

            Data.AddGameObject(this);
        }

        public override void ReceiveEvent(Event leEvent)
        {
            if (leEvent is TookDamage)
            {
                TookDamage td = (TookDamage)leEvent;
                health -= td.DamageTaken;

                if (health <= 0)
                {
                    Console.WriteLine("tree rip in peace");
                }
            }
        }

        public void GotChopped(Toon dude)
        {
            Console.WriteLine("chopped by: " + dude.GetId());
            client.SendEvent(new TookDamage(id, dude.GetAttackDamage()));
        }
    }
}
