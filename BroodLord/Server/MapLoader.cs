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
        static Random ran;
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
            Map.Initialize(5);
            ran = new Random();
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
                    LoadMobs(x, y, mobsMap.GetPixel(x, y));
                    LoadDoodads(x, y, doodadsMap.GetPixel(x, y));
                    LoadLoots(x, y, lootsMap.GetPixel(x, y));
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
            String grass = "Grass";
            String sand = "Sand";
            String marsh = "Marsh";
            String stone = "Stone";
            string shallow = "WaterShallow";
            string deep = "WaterDeep";
            int randomNumber;

            if (color.G == 255 && color.R == 0 && color.B == 0)
            {
                randomNumber = ran.Next(4) + 1;
                Map.SetTileTexture(x, y, grass + randomNumber.ToString());
            }
            else if (color.R == 255 && color.G == 255 && color.B == 0)
            {
                randomNumber = ran.Next(3) + 1;
                Map.SetTileTexture(x, y, sand + randomNumber.ToString());
            }
            else if (color.R == 255 && color.B == 255 && color.G == 0)
            {
                randomNumber = ran.Next(10);
                if (randomNumber > 9)
                {
                    Map.SetTileTexture(x, y, marsh + "1");
                }
                else if (randomNumber > 6)
                {
                    Map.SetTileTexture(x, y, marsh + "2");
                }
                else
                {
                    randomNumber = ran.Next(2) + 1;
                    Map.SetTileTexture(x, y, marsh + randomNumber.ToString());
                }
            }
            else if (color.G == 255 && color.B == 255 && color.G == 0)
            {
                randomNumber = ran.Next(10);
                if (randomNumber > 9)
                {
                    Map.SetTileTexture(x, y, stone + "1");
                }
                else if (randomNumber > 6)
                {
                    Map.SetTileTexture(x, y, stone + "2");
                }
                else
                {
                    Map.SetTileTexture(x, y, stone + "3");
                }
            }else if (color.B == 255 && color.R == 0 && color.G == 0)
            {
                //water
                randomNumber = ran.Next(3) + 1;
                Map.SetTileTexture(x, y, deep + randomNumber.ToString());

            }
            else if (color.B == 128 && color.R == 0 && color.G == 0)
            {
                //shallow water
                randomNumber = ran.Next(3) + 1;
                Map.SetTileTexture(x, y, shallow + randomNumber.ToString());
            }
            else if (color.R == 255 && color.G == 0 && color.B == 0)
            {
                //cliff
            }
        }

        public static void LoadMobs(int x, int y, System.Drawing.Color color)
        {
            if (color.R == 0 && color.G == 0 && color.B == 255)
            {
                new Cat(Guid.NewGuid(), new Vector2(x * Data.TileSize, y * Data.TileSize));
            }

            if (color.R == 255 && color.G == 0 && color.B == 0)
            {
                new CatPink(Guid.NewGuid(), new Vector2(x * Data.TileSize, y * Data.TileSize));
            }

            if (color.R == 0 && color.G == 255 && color.B == 0)
            {
                new SpideyBaby(Guid.NewGuid(), new Vector2(x * Data.TileSize, y * Data.TileSize));
            }

            if (color.R == 255 && color.G == 255 && color.B == 0)
            {
                new Swampert(Guid.NewGuid(), new Vector2(x * Data.TileSize, y * Data.TileSize));
            }
        }

        public static void LoadDoodads(int x, int y, System.Drawing.Color color)
        {
            if (color.R == 0 && color.G == 255 && color.B == 0)
            {
                new Tree(Guid.NewGuid(), new Vector2(x * Data.TileSize, y * Data.TileSize), "tree");
            }
            else if (color.R == 255 && color.G == 0 && color.B == 0)
            {
                new PalmTree(Guid.NewGuid(), new Vector2(x * Data.TileSize, y * Data.TileSize), "PalmTree");
            }
        }

        public static void LoadLoots(int x, int y, System.Drawing.Color color)
        {
            if (color.R == 0 && color.G == 255 && color.B == 0)
            {
                new RockLoot(Guid.NewGuid(), new Vector2(x * Data.TileSize, y * Data.TileSize));
            }
            else if (color.R == 0 && color.G == 0 && color.B == 255)
            {
                new MeatLoot(Guid.NewGuid(), new Vector2(x * Data.TileSize, y * Data.TileSize));
            }
            else if (color.R == 255 && color.G == 0 && color.B == 0)
            {
                new CoconutLoot(Guid.NewGuid(), new Vector2(x * Data.TileSize, y * Data.TileSize));
            }
            else if (color.R == 255 && color.G == 0 && color.B == 255)
            {
                new FlintLoot(Guid.NewGuid(), new Vector2(x * Data.TileSize, y * Data.TileSize));
            }
            else if (color.R == 0 && color.G == 255 && color.B == 255)
            {
                new ClubLoot(Guid.NewGuid(), new Vector2(x * Data.TileSize, y * Data.TileSize));
            }
        }
    }
}
