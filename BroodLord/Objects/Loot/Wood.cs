using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Objects
{
    public class Wood : Loot
    {
        public Wood(Vector2 position)
        {
            this.id = Guid.NewGuid();
            this.Map = Map;
            this.position = position;
            this.textureKey = "wood";
            this.textureKeyInBag = "woodBag";
            this.onGround = true;
            this.origin = new Vector2(Data.FindTexture[textureKey].Width / 2, Data.FindTexture[textureKey].Height * 0.85f);
            this.hitbox = new Rectangle((int)(position.X - origin.X), (int)(position.Y - origin.Y), Data.FindTexture[textureKey].Width, Data.FindTexture[textureKey].Height);

            Map.GetTile((int)(position.X/Map.GetTileSize()), (int)(position.Y/Map.GetTileSize())).InsertThing(this);

            Data.AddGameObject(this);
        }
    }
}
