using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Objects
{
    [Serializable()]
    public class Rock : Loot
    {
        public Rock(Guid id, Vector2 position) :base()
        {
            this.id = id;
            this.position = position;
            this.textureKey = "rock";
            this.textureKeyInBag = "rockBag";
            this.onGround = true;
            this.origin = new Vector2(Data.GetTextureSize(textureKey).X / 2, Data.GetTextureSize(textureKey).Y * 0.85f);
            this.hitbox = new Rectangle((int)(position.X - origin.X), (int)(position.Y - origin.Y), (int)Data.GetTextureSize(textureKey).X, (int)Data.GetTextureSize(textureKey).Y);
            this.quantity = 1;

            xTileCoord = (int)position.X / Data.TileSize;
            yTileCoord = (int)position.Y / Data.TileSize;
            Data.AddGameObject(this);
        }
    }
}
