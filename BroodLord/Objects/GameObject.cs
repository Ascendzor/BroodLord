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
    public class GameObject
    {
        protected Vector2 position;
        protected string textureKey;
        protected Guid id;
        protected Vector2 origin;
        protected Rectangle hitbox;
        protected int xTileCoord; 
        protected int yTileCoord;
        protected Client client;

        /*public GameObject(Vector2 position, string textureKey, Guid id, Vector2 origin, Rectangle hitbox,Client client)
        {
            this.position = position;
            this.textureKey = textureKey;
            this.id = id;    
            this.origin = origin;
            this.hitbox = hitbox;
            this.client = client;

            xTileCoord = (int)(position.X / Map.GetTileSize());
            yTileCoord = (int)(position.Y / Map.GetTileSize());

            Data.AddGameObject(this);

            Map.InsertGameObject(this);
        }*/

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public string TextureKey
        {
            get { return textureKey; }
            set { textureKey = value; }
        }

        public Rectangle GetHitbox()
        {
            return hitbox;
        }

        public int GetGridCoordX()
        {
            return xTileCoord;
        }

        public int GetGridCoordY()
        {
            return yTileCoord;
        }

        public Guid GetId()
        {
            return id;
        }

        public virtual void Draw(SpriteBatch sb)
        {
        }

        public virtual void ReceiveEvent(Event leEvent)
        {
        }
    }
}
