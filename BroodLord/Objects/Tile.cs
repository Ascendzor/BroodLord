using System;
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

namespace Objects
{
    public class Tile
    {
        private List<GameObject> gameObjects;

        string textureKey;

        public Tile(string textureKey)
        {
            gameObjects = new List<GameObject>();
            this.textureKey = textureKey;
        }

        public void InsertGameObject(GameObject go)
        {
            gameObjects.Add(go);
        }

        public List<GameObject> GetObjects()
        {
            return gameObjects;
        }

        public List<Doodad> GetDoodads()
        {
            List<Doodad> doodads = new List<Doodad>();
            foreach (GameObject doodad in gameObjects)
            {
                if (doodad is Doodad)
                {
                    doodads.Add((Doodad)doodad);
                }
            }

            return doodads;
        }

        public void Draw(SpriteBatch sb, Vector2 position)
        {
            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Draw(sb);
            }

            sb.Draw(Data.FindTexture[textureKey], new Rectangle((int)position.X, (int)position.Y, Data.FindTexture[textureKey].Width, Data.FindTexture[textureKey].Height), null, Color.White, 0, Vector2.One, SpriteEffects.None, 0.99999f);
        }
    }
}
