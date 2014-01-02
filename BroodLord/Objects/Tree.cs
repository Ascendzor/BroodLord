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
        public Tree(Vector2 position, string textureKey, Map map)
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

            map.GetTile(xTileCoord, yTileCoord).GetObjects().Add(this);
        }
    }
}
