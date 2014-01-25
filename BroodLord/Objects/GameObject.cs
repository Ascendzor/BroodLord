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
    [Serializable()]
    public abstract class GameObject
    {
        protected Vector2 oldPosition;
        protected Vector2 position;
        protected string textureKey;
        protected Guid id;
        protected Vector2 origin;
        protected Rectangle hitbox;
        protected int xTileCoord; 
        protected int yTileCoord;
        protected bool isInteractable;

        public bool IsInteractable
        {
            get { return isInteractable; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Vector2 OldPosition
        {
            get { return oldPosition; }
            set { oldPosition = value; }
        }

        public string TextureKey
        {
            get { return textureKey; }
            set { textureKey = value; }
        }

        public virtual Rectangle GetHitBox()
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

        public abstract void Draw(SpriteBatch sb);
    }
}
