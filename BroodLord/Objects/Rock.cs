using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Objects
{
    public class Rock : Loot
    {
        public Rock(Vector2 position, Map map)
        {
            this.map = map;
            this.position = position;
            this.textureKeyOnGround = "rock";
            this.textureKeyInBag = "rockBag";
            this.onGround = true;

            map.GetTile((int)(position.X/map.GetTileSize()), (int)(position.Y/map.GetTileSize())).InsertThing(this);
        }
    }
}
