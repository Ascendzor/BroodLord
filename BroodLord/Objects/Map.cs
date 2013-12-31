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
        private Tile[,] tiles = new Tile[20,20];
        private int tileSize;
        private int mapSize;
        private int renderWidth;

        public Map(int tileSize, int mapSize, int renderWidth)
        {
            this.mapSize = mapSize;
            this.tileSize = tileSize;
            this.renderWidth = renderWidth;

            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                for (int y = 0; y < tiles.GetLength(1); y++)
                {
                    int z = (x + y) / 5 + 1;
                    tiles[x, y] = new Tile("snow" + z);
                    ///to be changed to dynamic
                }
            }
        }

        public List<Tile> GetRenderedTiles(int xCenter, int yCenter)
        {
            List<Tile> renderedTiles = new List<Tile>();
            
            for (int x = -renderWidth; x <= renderWidth; x++)
            {
                for (int y = -renderWidth; y < renderWidth; y++)
                {
                    if (xCenter + x >= 0 && xCenter + x < mapSize && yCenter + y >= 0 && yCenter + y < mapSize)
                    {
                        renderedTiles.Add(tiles[xCenter + x, yCenter + y]);
                    }
                }
            }

            return renderedTiles;
        }

        public Tile GetTile(int x, int y)
        {
            return tiles[x, y];
        }

        public int GetMapSize()
        {
            return mapSize;
        }

        public int GetTileSize()
        {
            return tileSize;
        }

        public void Draw(SpriteBatch sb, int xCenter, int yCenter)
        {
            //draw ground
            for (int x = -renderWidth; x <= renderWidth; x++)
            {
                for (int y = -renderWidth; y < renderWidth; y++)
                {
                    if (xCenter + x >= 0 && xCenter + x < mapSize && yCenter + y >= 0 && yCenter + y < mapSize)
                    {
                        tiles[xCenter + x, yCenter + y].Draw(sb, new Vector2((xCenter + x) * tileSize, (yCenter + y) * tileSize));
                    }
                }
            }
        }
    }
}
