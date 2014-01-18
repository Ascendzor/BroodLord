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
        public static Dictionary<Guid, GameObject> FindGameObject;
        public static Dictionary<Guid, Mob> FindMob;
        public static Dictionary<Guid, Loot> FindLoot;
        public static Dictionary<Guid, Doodad> FindDoodad;
        public static int ToonRadius = 28;
        public static int ToonInteractionRange = 100;
        public static int TreeRadius = 56;
        public static int TileSize = 84;
        public static int MapSize = 20;

       

        public static void Initialize(ContentManager Content)
        {
            FindGameObject = new Dictionary<Guid, GameObject>();
            FindMob = new Dictionary<Guid, Mob>();
            FindLoot = new Dictionary<Guid, Loot>();
            FindDoodad = new Dictionary<Guid, Doodad>();

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
            FindGameObject = new Dictionary<Guid, GameObject>();
            FindMob = new Dictionary<Guid, Mob>();
            FindLoot = new Dictionary<Guid, Loot>();
            FindDoodad = new Dictionary<Guid, Doodad>();

            FindTextureSize = new Dictionary<string, Vector2>();
            InitializeAllTextures();

            foreach (string key in allTextures)
            {
                Image leImage = Image.FromFile(@"../../../BroodLord/BroodLordContent/" + key + ".png");
                FindTextureSize.Add(key, new Vector2(leImage.Width, leImage.Height));
            }
        }

        public static Vector2 GetTextureSize(string textureKey)
        {
            return FindTextureSize[textureKey];
        }

        public static void AddGameObject(GameObject go)
        {
            if (FindGameObject.ContainsKey(go.GetId()))
            {
                return;
            }
            Map.InsertGameObject(go);
            FindGameObject.Add(go.GetId(), go);
            
            if (go is Mob)
            {
                FindMob.Add(go.GetId(), (Mob)go);
            }
            else if (go is Loot)
            {
                FindLoot.Add(go.GetId(), (Loot)go);
            }
            else if (go is Doodad)
            {
                FindDoodad.Add(go.GetId(), (Doodad)go);
            }
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
