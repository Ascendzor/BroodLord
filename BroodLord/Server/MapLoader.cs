﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Objects;
using Microsoft.Xna.Framework;
using System.Runtime.Serialization.Formatters.Binary;

namespace Server
{
    static class MapLoader
    {

        //WTF
        private static Client client;

        /// <summary>
        /// Loads the map from a .bmp
        /// 
        /// At the moment the mapping of colors to objects is:
        /// (255,0,0) => Tree
        /// (0,255,0) => Rock
        /// (*, *, *) => blank
        /// </summary>
        /// <param name="path"> Path to a BMP</param>
        public static void LoadMap(string path)
        {
            System.Drawing.Bitmap mapImage = new System.Drawing.Bitmap(path);

            Data.Initialize();
            Map.Initialize(Data.TileSize, Data.MapSize, 5);

            for (int y = 0; y < mapImage.Height; y++)
            {
                for (int x = 0; x < mapImage.Width; x++)
                {
                    System.Drawing.Color pixel = mapImage.GetPixel(x, y);
                    if (pixel.R == 255 && pixel.G == 0 && pixel.B == 0)
                    {
                        new Tree(new Vector2(x * Data.TileSize, y * Data.TileSize), "tree", client);
                    }
                    if (pixel.R == 0 && pixel.G == 255 && pixel.B == 0)
                    {
                        new Rock(new Vector2(x * Data.TileSize, y * Data.TileSize));
                    }
                }
            }
        }
    }
}