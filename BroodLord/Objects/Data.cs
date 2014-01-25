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
        private static List<string> allSounds;
        public static Dictionary<string, Texture2D> FindTexture;
        public static Dictionary<string, Vector2> FindTextureSize;
        public static Dictionary<string, SoundEffect> FindSound;
        public static int ToonRadius = 28;
        public static int ToonInteractionRange = 100;
        public static int TreeRadius = 56;
        public static int TileSize = 84;
        public static int MapSize = 100;
        public static bool IsServer;
        public static Toon Dude;

        //This should probably be in GameDataSizeMessage
        public static int SizeOfNetEventPacket = 140;

        public static void Initialize(ContentManager Content)
        {
            FindTexture = new Dictionary<string, Texture2D>();
            FindSound = new Dictionary<string, SoundEffect>();

            Data.InitializeAllTextures();
            Data.InitializeAllSounds();

            foreach (string textureKey in allTextures)
            {
                FindTexture.Add(textureKey, Content.Load<Texture2D>(textureKey));
            }

            foreach (string soundKey in allSounds)
            {
                FindSound.Add(soundKey, Content.Load<SoundEffect>(soundKey));
            }

            FindTextureSize = new Dictionary<string, Vector2>();
            foreach (string textureKey in FindTexture.Keys)
            {
                FindTextureSize.Add(textureKey, new Vector2(FindTexture[textureKey].Width, FindTexture[textureKey].Height));
            }

            IsServer = false;
        }

        private static void InitializeAllSounds()
        {
            allSounds = new List<string>();

            allSounds.Add("Eat");
            allSounds.Add("WoodChop");
            allSounds.Add("WoodFall");
        }

        private static void InitializeAllTextures()
        {
            allTextures = new List<string>();

            allTextures.Add("link");
            allTextures.Add("tree");
            allTextures.Add("PalmTree");
            allTextures.Add("PalmStump");
            allTextures.Add("Rock");
            allTextures.Add("Flint");
            allTextures.Add("Club");
            allTextures.Add("Coconut");
            allTextures.Add("FlintBag");
            allTextures.Add("ClubBag");
            allTextures.Add("RockBag");
            allTextures.Add("CoconutBag");
            allTextures.Add("Wood");
            allTextures.Add("WoodBag");
            allTextures.Add("cat");
            allTextures.Add("treeOutline");
            allTextures.Add("stump");
            allTextures.Add("inventorySlots");
            allTextures.Add("InventorySlot");
            allTextures.Add("EmptyInventorySlot");
            allTextures.Add("health1");
            allTextures.Add("health2");
            allTextures.Add("health3");
            allTextures.Add("health4");
            allTextures.Add("health5");
            allTextures.Add("health6");
            allTextures.Add("health7");
            allTextures.Add("health8");
            allTextures.Add("health9");
            allTextures.Add("health10");
            allTextures.Add("healthBarOutline");
            allTextures.Add("hunger1");
            allTextures.Add("hunger2");
            allTextures.Add("hunger3");
            allTextures.Add("hunger4");
            allTextures.Add("hunger5");
            allTextures.Add("hunger6");
            allTextures.Add("hunger7");
            allTextures.Add("hunger8");
            allTextures.Add("hunger9");
            allTextures.Add("hunger10");
            allTextures.Add("hungerOutline");
            allTextures.Add("hydration1");
            allTextures.Add("hydration2");
            allTextures.Add("hydration3");
            allTextures.Add("hydration4");
            allTextures.Add("hydration5");
            allTextures.Add("hydration6");
            allTextures.Add("hydration7");
            allTextures.Add("hydration8");
            allTextures.Add("hydration9");
            allTextures.Add("hydration10");
            allTextures.Add("hydrationOutline");
            allTextures.Add("snow1");
            allTextures.Add("snow2");
            allTextures.Add("snow3");
            allTextures.Add("snow4");
            allTextures.Add("snow5");
            allTextures.Add("snow6");
            allTextures.Add("snow7");
            allTextures.Add("snow8");
            allTextures.Add("meatPH");
            allTextures.Add("evil man medium");
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
