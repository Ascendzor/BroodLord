﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Objects
{
    public class Data
    {
        public static Dictionary<string, Texture2D> findTexture;
        public static int toonRadius = 28;
        public static int treeRadius = 56;
        public static int tileSize = 84;
        public static int mapSize = 20;
        /*
         * 
         * Some Notes
         * 
         * We do not want to check for other players collision but we may still nee to do tile checking on them if we want to draw
         * 
         * 
         * 
         */
    }
}