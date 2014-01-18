using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Objects
{
    [Serializable()]
    public class Item
    {
        protected Guid id;
        protected string textureKey;
        protected Vector2 origin;
        protected Rectangle hitbox;
        protected int quantity;

        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        public string TextureKey
        {
            get { return textureKey; }
            set { textureKey = value; }
        }
    }
}
