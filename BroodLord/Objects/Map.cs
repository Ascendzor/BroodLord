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
        private static Tile[,] tiles; //I wonder if there should be a TileManager class who we ask for everything tile-related¿ -Troy
        private static int renderWidth;

        public static void Initialize(int _renderWidth)
        {
            renderWidth = _renderWidth;

            tiles = new Tile[Data.MapSize, Data.MapSize];

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
            foreach (Tile tile in tiles)
            {
                tile.CheckGameObjects();
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
            GetTile((int)gameObject.Position.X / Data.TileSize, (int)gameObject.Position.Y / Data.TileSize).RemoveObject(gameObject);
        }

        public static void RemoveGameObject(Guid id)
        {
            foreach (Tile tile in tiles)
            {
                if (tile.GetGameObject(id) != null)
                {
                    RemoveGameObject(tile.GetGameObject(id));
                }
            }
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
                    if (xCenter + x >= 0 && xCenter + x < Data.MapSize && yCenter + y >= 0 && yCenter + y < Data.MapSize)
                    {
                        renderedTiles.Add(tiles[xCenter + x, yCenter + y]);
                    }
                }
            }

            return renderedTiles;
        }

        public static Tile GetTile(int x, int y)
        {
            if (x < 0 || x >= Data.MapSize || y < 0 || y >= Data.MapSize)
            {
                return null;
            }
            return tiles[x, y];
        }

        public static int GetMapSize()
        {
            return Data.MapSize;
        }

        public static GameObject GetGameObject(Guid id)
        {
            foreach (Tile tile in tiles)
            {
                if (tile.GetGameObject(id) != null)
                {
                    return tile.GetGameObject(id);
                }
            }
            return null;
        }

        public static List<GameObject> GetGameObjects()
        {
            List<GameObject> allGameObjects = new List<GameObject>();
            foreach (Tile tile in tiles)
            {
                foreach (GameObject go in tile.GetGameObjects())
                {
                    allGameObjects.Add(go);
                }
            }

            return allGameObjects;
        }

        public static List<Mob> GetMobs()
        {
            List<Mob> allMobs = new List<Mob>();
            foreach (Tile tile in tiles)
            {
                foreach (Mob mob in tile.GetMobs())
                {
                    allMobs.Add(mob);
                }
            }

            return allMobs;
        }

        public static Loot GetLoot(Guid id)
        {
            foreach (Tile tile in tiles)
            {
                if (tile.GetGameObject(id) != null)
                {
                    return (Loot)tile.GetGameObject(id);
                }
            }

            return null;
        }

        //this currently can only collide with adjacent tiles, should be more dynamic if we wanted bigger things
        //this is also using interactable as collidable, may or may not want to keep it this way
        public static List<Doodad> GetCollidableDoodads(GameObject go)
        {
            List<Doodad> collidableDoodads = new List<Doodad>();

            int xTile = (int)go.Position.X / Data.TileSize;
            int yTile = (int)go.Position.Y / Data.TileSize;

            for (int x = (xTile-1); x < (xTile + 2); x++)
            {
                for (int y = (yTile-1); y < (yTile + 2); y++)
                {
                    if (GetTile(x, y) != null)
                    {
                        foreach (Doodad doodad in GetTile(x, y).GetDoodads())
                        {
                            if (doodad.IsInteractable)
                            {
                                collidableDoodads.Add(doodad);
                            }
                        }
                    }
                }
            }

            return collidableDoodads;
        }
    }
}
