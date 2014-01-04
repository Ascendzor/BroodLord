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
        private static Tile[,] tiles = new Tile[20, 20];
        private static int tileSize;
        private static int MapSize;
        private static int renderWidth;

        public static void Initialize(int _tileSize, int _MapSize, int _renderWidth)
        {
            MapSize = _MapSize;
            tileSize = _tileSize;
            renderWidth = _renderWidth;

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

        public static List<Tile> GetRenderedTiles(int xCenter, int yCenter)
        {
            List<Tile> renderedTiles = new List<Tile>();
            
            for (int x = -renderWidth; x <= renderWidth; x++)
            {
                for (int y = -renderWidth; y < renderWidth; y++)
                {
                    if (xCenter + x >= 0 && xCenter + x < MapSize && yCenter + y >= 0 && yCenter + y < MapSize)
                    {
                        renderedTiles.Add(tiles[xCenter + x, yCenter + y]);
                    }
                }
            }

            return renderedTiles;
        }

        public static Tile GetTile(int x, int y)
        {
            return tiles[x, y];
        }

        public static int GetMapSize()
        {
            return MapSize;
        }

        public static int GetTileSize()
        {
            return tileSize;
        }

        public static void Draw(SpriteBatch sb, int xCenter, int yCenter)
        {
            //draw ground
            for (int x = -renderWidth; x <= renderWidth; x++)
            {
                for (int y = -renderWidth; y < renderWidth; y++)
                {
                    if (xCenter + x >= 0 && xCenter + x < MapSize && yCenter + y >= 0 && yCenter + y < MapSize)
                    {
                        tiles[xCenter + x, yCenter + y].Draw(sb, new Vector2((xCenter + x) * tileSize, (yCenter + y) * tileSize));
                    }
                }
            }
        }

        public static void RemoveGameObject(GameObject gameObject)
        {
            GetTile(gameObject.GetGridCoordX(), gameObject.GetGridCoordY()).RemoveObject(gameObject);
        }

        public static void InsertGameObject(GameObject gameObject)
        {
            GetTile(gameObject.GetGridCoordX(), gameObject.GetGridCoordY()).InsertObject(gameObject);
        }
    }
}
