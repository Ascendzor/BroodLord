using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Objects
{
    public class Tile
    {
        private List<GameObject> gameObjects;
        private List<Item> items;

        string textureKey;

        public Tile(string textureKey)
        {
            gameObjects = new List<GameObject>();
            items = new List<Item>();
            this.textureKey = textureKey;
        }

        public List<GameObject> GetObjects()
        {
            return gameObjects;
        }

        public void Draw(SpriteBatch sb, Vector2 position)
        {
            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Draw(sb);
            }
            foreach (Item item in items)
            {
                item.Draw(sb);
            }
            sb.Draw(Data.findTexture[textureKey], new Rectangle((int)position.X, (int)position.Y, Data.findTexture[textureKey].Width, Data.findTexture[textureKey].Height), null, Color.White, 0, Vector2.One, SpriteEffects.None, 0.99999f);
        }

        public void InsertThing(Item item)
        {
            items.Add(item);
        }
    }
}
