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

namespace Objects
{
    public class Data
    {
        public static Dictionary<string, Texture2D> FindTexture;
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

            Data.FindTexture = new Dictionary<string, Texture2D>();
            
            Data.FindTexture.Add("link", Content.Load<Texture2D>("link"));
            Data.FindTexture.Add("tree", Content.Load<Texture2D>("tree"));
            Data.FindTexture.Add("rock", Content.Load<Texture2D>("rock"));
            Data.FindTexture.Add("wood", Content.Load<Texture2D>("wood"));
            Data.FindTexture.Add("cat", Content.Load<Texture2D>("cat"));
            Data.FindTexture.Add("treeOutline", Content.Load<Texture2D>("treeOutline"));
            Data.FindTexture.Add("stump", Content.Load<Texture2D>("stump"));
            Data.FindTexture.Add("inventorySlots", Content.Load<Texture2D>("inventorySlots"));
            for (int x = 1; x < 9; x++)
            {
                Data.FindTexture.Add("snow" + x, Content.Load<Texture2D>("snow" + x));
            }
        }

        public static void Initialize()
        {
            FindGameObject = new Dictionary<Guid, GameObject>();
            FindMob = new Dictionary<Guid, Mob>();
            FindLoot = new Dictionary<Guid, Loot>();
            FindDoodad = new Dictionary<Guid, Doodad>();
        }

        public static void AddGameObject(GameObject go)
        {
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
