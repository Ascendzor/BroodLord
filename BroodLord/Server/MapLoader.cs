using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Objects;
using Microsoft.Xna.Framework;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing;

namespace Server
{
    static class MapLoader
    {
        /// <summary>
        /// Loads the map from a .bmp
        /// 
        /// At the moment the mapping of colors to objects is:
        /// (255,0,0) => Tree
        /// (0,255,0) => Rock
        /// (*, *, *) => blank
        /// </summary>
        /// <param name="path"> Path to a BMP</param>
        public static void LoadMap(string terrainPath, string mobsPath, string doodadsPath, string lootsPath)
        {
            Bitmap mapImage = new Bitmap(terrainPath);

            Map.Initialize(5);

            for (int y = 0; y < mapImage.Height; y++)
            {
                for (int x = 0; x < mapImage.Width; x++)
                {
                    System.Drawing.Color pixel = mapImage.GetPixel(x, y);
                    if (pixel.R == 255 && pixel.G == 0 && pixel.B == 0)
                    {
                        new Tree(Guid.NewGuid(), new Vector2(x * Data.TileSize, y * Data.TileSize), "tree");
                    }
                    if (pixel.R == 0 && pixel.G == 255 && pixel.B == 0)
                    {
                        new RockLoot(Guid.NewGuid(), new Vector2(x * Data.TileSize, y * Data.TileSize));
                    }
                    if (pixel.R == 0 && pixel.G == 0 && pixel.B == 255)
                    {
                        
                    }
                }
            }

            Bitmap terrainMap = new Bitmap(terrainPath);
            Bitmap mobsMap = new Bitmap(mobsPath);
            Bitmap doodadsMap = new Bitmap(doodadsPath);
            Bitmap lootsMap = new Bitmap(lootsPath);
            System.Drawing.Point smallestMap = GetSmallestMap(new List<Bitmap>() { terrainMap, mobsMap, doodadsMap, lootsMap });
            for (int x = 0; x < smallestMap.X; x++)
            {
                for (int y = 0; y < smallestMap.Y; y++)
                {
                    LoadTerrain(x, y, terrainMap.GetPixel(x, y));
                }
            }
        }

        //get the smallest width of every submitted map
        private static System.Drawing.Point GetSmallestMap(List<Bitmap> maps)
        {
            System.Drawing.Point smallestMap = new System.Drawing.Point(maps[0].Width, maps[0].Height);
            foreach (Bitmap map in maps)
            {
                if (map.Width < smallestMap.X)
                {
                    smallestMap.X = map.Width;
                }
                if (map.Height < smallestMap.Y)
                {
                    smallestMap.Y = map.Height;
                }
            }

            return smallestMap;
        }

        public static void LoadTerrain(int x, int y, System.Drawing.Color color)
        {
            //load terrain here
            Map.SetTileTexture(x, y, "snow1");
        }

        public static void LoadMobs(int x, int y, System.Drawing.Color color)
        {
            if (color.B == 255)
            {
                new Cat(Guid.NewGuid(), new Vector2(x * Data.TileSize, y * Data.TileSize));
            }
        }

        public static void LoadDoodads(int x, int y, System.Drawing.Color color)
        {
            if (color.R == 255)
            {
                new Tree(Guid.NewGuid(), new Vector2(x * Data.TileSize, y * Data.TileSize), "tree");
            }
        }

        public static void LoadLoots(int x, int y, System.Drawing.Color color)
        {
            if (color.G == 255)
            {
                new RockLoot(Guid.NewGuid(), new Vector2(x * Data.TileSize, y * Data.TileSize));
            }
        }
    }
}
