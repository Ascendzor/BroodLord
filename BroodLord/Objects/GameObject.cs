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
        protected int xTileCoord;
        protected int yTileCoord;
        protected Map map;
        protected int colRadius;
        protected bool isCollidable;

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

        public int GetColRadius()
        {
            return colRadius;
        }

        public bool IsCollidable()
        {
            return isCollidable;
        }

        public void Draw(SpriteBatch sb)
        {
            Console.WriteLine(position.Y / (map.GetMapSize() * map.GetTileSize()));
            sb.Draw(Data.findTexture[textureKey], new Rectangle((int)(position.X - (Data.findTexture[textureKey].Width * 0.5f)), (int)(position.Y - Data.findTexture[textureKey].Height*.85), Data.findTexture[textureKey].Width, Data.findTexture[textureKey].Height), null, Color.White, 0, Vector2.One, SpriteEffects.None, 1-(position.Y / (map.GetMapSize() * map.GetTileSize())));
        }
    }
}
