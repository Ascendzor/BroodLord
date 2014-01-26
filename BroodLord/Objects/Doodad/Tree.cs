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

        public Tree(Guid id, Vector2 position, string textureKey)
        {
            this.id = id;
            this.position = position;
            this.textureKey = textureKey;
            this.xTileCoord = (int)position.X / Data.TileSize;
            this.yTileCoord = (int)position.Y / Data.TileSize;
            this.collisionWidth = Data.TreeRadius;
            this.origin = new Vector2(Data.GetTextureSize(textureKey).X / 2, Data.GetTextureSize(textureKey).Y * 0.85f);
            this.hitbox = new Rectangle((int)(position.X - origin.X), (int)(position.Y - origin.Y), (int)Data.GetTextureSize(textureKey).X, (int)Data.GetTextureSize(textureKey).Y);
            this.health = 999;
            this.isInteractable = true;

            Map.InsertGameObject(this);
        }

        public void GotChopped(Toon dude)
        {
           if (!Data.IsServer)
           {
                Sounds.PlaySound(Data.FindSound["WoodChop"]);
           }

            health -= (int)dude.GetAttackDamage();
            if (health < 0)
            {
                textureKey = "stump";
                this.isInteractable = false;
                Console.WriteLine(health);

                if (Data.IsServer)
                {
                    Client.SendEvent(new SpawnWoodEvent(Guid.NewGuid(), new Vector2(position.X - 10, position.Y)));
                    Client.SendEvent(new SpawnWoodEvent(Guid.NewGuid(), new Vector2(position.X + 20, position.Y + 10)));
                }
            }
        }
    }
}
