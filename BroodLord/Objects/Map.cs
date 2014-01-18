﻿using System;
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
        private static Tile[,] tiles = new Tile[20, 20]; //I wonder if there should be a TileManager class who we ask for everything tile-related¿ -Troy
        private static int MapSize;
        private static int renderWidth;

        public static void Initialize(int _MapSize, int _renderWidth)
        {
            MapSize = _MapSize;
            renderWidth = _renderWidth;

            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                for (int y = 0; y < tiles.GetLength(1); y++)
                {
                    int z = (x + y) / 5 + 1;
                    tiles[x, y] = new Tile("snow" + z, x, y);
                    //to be passed in by the server
                }
            }
        }

        public static void Update()
        {
            //check if all of the mobs are in the correct tiles
            for (int x = 0; x < tiles.GetLength(0); x++)
            {
                for (int y = 0; y < tiles.GetLength(1); y++)
                {
                    tiles[x, y].CheckGameObjects();
                }
            }
        }

        //use the given GameObjects position to manage what tile to go into
        public static void InsertGameObject(GameObject go)
        {
            //BAD BAD IF STATEMENT, BE GONE, DAMN ARRAYS
            if (go.Position.X < 0 || go.Position.X > tiles.GetLength(0) * Data.TileSize || go.Position.Y < 0 || go.Position.Y > tiles.GetLength(1) * Data.TileSize)
            {
                go.Position = new Vector2(100, 100);
            }
            tiles[(int)(go.Position.X / Data.TileSize), (int)(go.Position.Y / Data.TileSize)].InsertGameObject(go);
        }

        public static void Draw(SpriteBatch sb, Vector2 dudesPosition)
        {
            List<Tile> tilesToRender = GetRenderedTiles(dudesPosition);
            foreach (Tile tile in tilesToRender)
            {
                tile.Draw(sb);
            }
        }

        public static void RemoveGameObject(GameObject gameObject)
        {
            GetTile(gameObject.GetGridCoordX(), gameObject.GetGridCoordY()).RemoveObject(gameObject);
        }

        public static List<Tile> GetRenderedTiles(Vector2 dudesPosition)
        {
            List<Tile> renderedTiles = new List<Tile>();
            int xCenter = (int)dudesPosition.X / Data.TileSize;
            int yCenter = (int)dudesPosition.Y / Data.TileSize;
            //draw ground
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
    }
}
