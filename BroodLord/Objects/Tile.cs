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
using System.Threading;

namespace Objects
{
    public class Tile
    {
        private Dictionary<Guid, GameObject> gameObjects;
        private string textureKey;
        private Rectangle area;

        public Tile(string textureKey, int positionX, int positionY)
        {
            gameObjects = new Dictionary<Guid, GameObject>();
            this.textureKey = textureKey;
            this.area = new Rectangle(positionX * Data.TileSize, positionY * Data.TileSize, Data.TileSize, Data.TileSize);
        }

        public void Update()
        {
            foreach (GameObject go in gameObjects.Values.ToList())
            {
                if (go is Mob) //<-- this entire if might go and we might end up just updating GameObject, we'll see -Troy
                {
                    ((Mob)go).Update();
                }
            }

            CheckGameObjects();
        }

        private void CheckGameObjects()
        {
            foreach (GameObject go in gameObjects.Values.ToList())
            {
                if (go is Mob)
                {
                    Mob mobGo = (Mob)go;
                    Vector2 mobPosition = mobGo.Position;
                    if (!area.Contains(new Point((int)mobPosition.X, (int)mobPosition.Y)))
                    {
                        RemoveObject(mobGo);
                        Map.InsertGameObject(mobGo);
                    }
                }
            }
        }

        public void InsertGameObject(GameObject go)
        {
            if(gameObjects.ContainsKey(go.GetId()))
            {
                return;
            }
            gameObjects.Add(go.GetId(), go);
        }

        public GameObject GetGameObject(Guid id)
        {
            if (gameObjects.ContainsKey(id))
            {
                return gameObjects[id];
            }

            return null;
        }

        public List<GameObject> GetGameObjects()
        {
            return gameObjects.Values.ToList();
        }

        public List<Doodad> GetDoodads()
        {
            List<Doodad> doodads = new List<Doodad>();
            foreach (GameObject doodad in gameObjects.Values.ToList())
            {
                if (doodad is Doodad)
                {
                    doodads.Add((Doodad)doodad);
                }
            }

            return doodads;
        }

        public List<Mob> GetMobs()
        {
            List<Mob> allMobs = new List<Mob>();
            foreach(GameObject go in gameObjects.Values.ToList())
            {
                if(go is Mob)
                {
                    allMobs.Add((Mob)go);
                }
            }

            return allMobs;
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (GameObject gameObject in gameObjects.Values.ToList())
            {
                gameObject.Draw(sb);
            }

            sb.Draw(Data.FindTexture[textureKey], area, null, Color.White, 0, Vector2.One, SpriteEffects.None, 0.99999f);
        }

        public void RemoveObject(GameObject gameObject)
        {
            gameObjects.Remove(gameObject.GetId());
        }
    }
}
