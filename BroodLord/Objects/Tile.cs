﻿using System;
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
        private Dictionary<Guid, GameObject> gameObjects;
        private string textureKey;
        private Rectangle area;

        public Tile(string textureKey, int positionX, int positionY)
        {
            gameObjects = new Dictionary<Guid, GameObject>();
            this.textureKey = textureKey;
            this.area = new Rectangle(positionX * Data.TileSize, positionY * Data.TileSize, Data.TileSize, Data.TileSize);
        }

        public void CheckGameObjects()
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
            foreach (GameObject doodad in gameObjects.Values)
            {
                if (doodad is Doodad)
                {
                    doodads.Add((Doodad)doodad);
                }
            }

            return doodads;
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (GameObject gameObject in gameObjects.Values)
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
