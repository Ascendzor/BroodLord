using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Objects;


namespace Objects
{
    public class Map
    {
        private ContentManager Content;
       
        private Tile[,] Tiles = new Tile[20,20];
        private int tileSize;
        private int mapSize;
        public Map(ContentManager Content,int tileSize,int mapSize)
        {
            this.mapSize = mapSize;
            this.tileSize = tileSize;
            this.Content = Content;
            for(int x = 0; x<Tiles.GetLength(0);x++)
                for (int y = 0; y < Tiles.GetLength(1); y++)
                {
                    int z = (x + y) / 5 +1;
                    Tiles[x, y] = new Tile("snow" + z);
                    ///to be changed to dynamic
                }

        }

        public Tile GetTile(int x, int y)
        {
            return Tiles[x, y];
        }

        public int GetMapSize()
        {
            return mapSize;
        }

        public int GetTileSize()
        {
            return tileSize;
        }

        public void Draw(SpriteBatch sb, int size, int xCenter, int yCenter)
        {
            //draw ground
            for (int x = -size; x <= size; x++)
                for (int y = -size; y < size; y++)
                {
                    if (xCenter + x >= 0 && xCenter + x < mapSize && yCenter + y >= 0 && yCenter + y < mapSize)
                    {
                        Tiles[xCenter + x, yCenter + y].Draw(sb, new Vector2((xCenter + x) * tileSize, (yCenter + y) * tileSize));
                    }
                }
        }

        public void Draw(SpriteBatch sb)
        {
            //draw ground
            for (int x = 0; x < Tiles.GetLength(0); x++)
                for (int y = 0; y < Tiles.GetLength(1); y++)
                {
                    Tiles[x, y].Draw(sb, new Vector2(x * tileSize, y * tileSize));
                }
        } 

    }
}
