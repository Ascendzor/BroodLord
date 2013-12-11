using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Objects
{
    class Tree
    {
        private Vector2 position;
        public string textureKey;

        public Tree(Vector2 position, string textureKey)
        {
            this.position = position;
            this.textureKey = textureKey;
        }

        public void Draw(SpriteBatch sb, Texture2D texture)
        {
            sb.Draw(texture, new Vector2(position.X - (texture.Width * 0.5f), position.Y - texture.Height), Color.White);
        }
    }
}
