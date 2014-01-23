using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Drawing;
using System.IO;

namespace Objects
{
    public class Data
    {
        private static List<string> allTextures;
        public static Dictionary<string, Texture2D> FindTexture;
        public static Dictionary<string, Vector2> FindTextureSize;
        public static int ToonRadius = 28;
        public static int ToonInteractionRange = 100;
        public static int TreeRadius = 56;
        public static int TileSize = 84;
        public static int MapSize = 40;
        public static bool IsServer;

        //This should probably be in GameDataSizeMessage
        public static int SizeOfNetEventPacket = 140;

        public static void Initialize(ContentManager Content)
        {
            FindTexture = new Dictionary<string, Texture2D>();

            Data.InitializeAllTextures();

            foreach (string textureKey in allTextures)
            {
                FindTexture.Add(textureKey, Content.Load<Texture2D>(textureKey));
            }

            FindTextureSize = new Dictionary<string, Vector2>();
            foreach (string textureKey in FindTexture.Keys)
            {
                FindTextureSize.Add(textureKey, new Vector2(FindTexture[textureKey].Width, FindTexture[textureKey].Height));
            }

            IsServer = false;
        }

        private static void InitializeAllTextures()
        {
            allTextures = new List<string>();

            allTextures.Add("link");
            allTextures.Add("tree");
            allTextures.Add("rock");
            allTextures.Add("wood");
            allTextures.Add("cat");
            allTextures.Add("treeOutline");
            allTextures.Add("stump");
            allTextures.Add("inventorySlots");
            allTextures.Add("InventorySlot");
            allTextures.Add("EmptyInventorySlot");
            allTextures.Add("snow1");
            allTextures.Add("snow2");
            allTextures.Add("snow3");
            allTextures.Add("snow4");
            allTextures.Add("snow5");
            allTextures.Add("snow6");
            allTextures.Add("snow7");
            allTextures.Add("snow8");
        }

        public static void Initialize()
        {
            FindTextureSize = new Dictionary<string, Vector2>();
            InitializeAllTextures();

            foreach (string key in allTextures)
            {
                Image leImage = Image.FromFile(@"../../../BroodLord/BroodLordContent/" + key + ".png");
                FindTextureSize.Add(key, new Vector2(leImage.Width, leImage.Height));
            }

            IsServer = true;
        }

        public static Vector2 GetTextureSize(string textureKey)
        {
            return FindTextureSize[textureKey];
        }

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
