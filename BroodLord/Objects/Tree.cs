using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Objects
{
    public class Tree : GameObject
    {
        
        public Tree(Vector2 position, string textureKey,Map map)
        {
            this.position = position;
            this.textureKey = textureKey;
            this.map = map;

            xTileCoord = (int)position.X / map.GetTileSize();
            yTileCoord = (int)position.Y / map.GetTileSize();

            map.GetTile(xTileCoord, yTileCoord).GetObjects().Add(this);

            colRadius = Data.treeRadius;
            isCollidable = true;
        }
    }
}
