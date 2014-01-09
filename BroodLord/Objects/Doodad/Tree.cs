using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Threading;

namespace Objects
{
    [Serializable()]
    public class Tree : Doodad
    {
        private int health;
        private bool isStump;

        public Tree(Guid id, Vector2 position, string textureKey)
        {
            this.id = id;
            this.position = position;
            this.textureKey = textureKey;
            this.xTileCoord = (int)position.X / Map.GetTileSize();
            this.yTileCoord = (int)position.Y / Map.GetTileSize();
            this.textureWidth = 256;
            this.textureHeight = 256;
            this.collisionWidth = Data.TreeRadius;
            this.origin = new Vector2(textureWidth/2, textureHeight * 0.85f);
            this.hitbox = new Rectangle((int)(position.X - origin.X), (int)(position.Y - origin.Y), (int)textureWidth, (int)textureHeight);
            this.health = 999;
            this.isInteractable = true;
            Data.AddGameObject(this);
        }

        public void ReceiveEvent(TookDamageEvent leEvent)
        {
            if (isStump)
            {
                return;
            }

            if (leEvent is TookDamageEvent)
            {
                TookDamageEvent td = (TookDamageEvent)leEvent;
                health -= (int)td.DamageTaken;
            }
        }

        public void ReceiveEvent(SpawnWoodEvent leEvent)
        {
            Console.WriteLine("received wood");
            new Wood(leEvent.WoodId, leEvent.Position);
        }

        public void ReceiveEvent(TreeRipEvent leEvent)
        {
            Console.WriteLine(id + " rip in peace");
            isStump = true;
            textureKey = "stump";
            this.isInteractable = false;
        }

        public void GotChopped(Toon dude)
        {
            if (health < dude.GetAttackDamage())
            {
                Client.SendEvent(new TreeRipEvent(id));
                Client.SendEvent(new SpawnWoodEvent(id, Guid.NewGuid(), position + new Vector2(-20, 5)));
                Client.SendEvent(new SpawnWoodEvent(id, Guid.NewGuid(), position + new Vector2(20, 20)));
                Client.SendEvent(new SpawnWoodEvent(id, Guid.NewGuid(), position + new Vector2(50, 20)));
                Client.SendEvent(new SpawnWoodEvent(id, Guid.NewGuid(), position + new Vector2(30, 20)));
                Client.SendEvent(new SpawnWoodEvent(id, Guid.NewGuid(), position + new Vector2(40, 20)));
                Client.SendEvent(new SpawnWoodEvent(id, Guid.NewGuid(), position + new Vector2(20, 40)));
                return;
            }
            Client.SendEvent(new TookDamageEvent(id, dude.GetAttackDamage()));
        }
    }
}
