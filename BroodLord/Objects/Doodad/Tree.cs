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
        private bool isStump;

        public Tree(Vector2 position, string textureKey, Client client)
        {
            this.id = Guid.NewGuid();
            this.position = position;
            this.textureKey = textureKey;
            this.xTileCoord = (int)position.X / Map.GetTileSize();
            this.yTileCoord = (int)position.Y / Map.GetTileSize();
            this.collisionWidth = Data.TreeRadius;
            this.origin = new Vector2(Data.FindTexture[textureKey].Width / 2, Data.FindTexture[textureKey].Height * 0.85f);
            this.hitbox = new Rectangle((int)(position.X - origin.X), (int)(position.Y - origin.Y), Data.FindTexture[textureKey].Width, Data.FindTexture[textureKey].Height);
            this.client = client;
            this.health = 999;

            Map.GetTile(xTileCoord, yTileCoord).GetObjects().Add(this);
            
            Data.AddGameObject(this);
        }

        public override void ReceiveEvent(Event leEvent)
        {
            if (isStump)
            {
                return;
            }

            if (leEvent is TookDamage)
            {
                TookDamage td = (TookDamage)leEvent;
                health -= (int)td.DamageTaken;

                if (health <= 0)
                {
                    isStump = true;
                    textureKey = "stump";
                    this.hitbox = Rectangle.Empty; //temporary code to allow you to pick up the wood
                    DropLoot();
                }
            }
        }

        //made this into a method because it's going to changed a lot. This is just a proof of concept.
        private void DropLoot()
        {
            new Wood(position + new Vector2(-20, 5));
            new Wood(position + new Vector2(20, 20));
        }

        public void GotChopped(Toon dude)
        {
            client.SendEvent(new TookDamage(id, dude.GetAttackDamage()));
        }
    }
}
