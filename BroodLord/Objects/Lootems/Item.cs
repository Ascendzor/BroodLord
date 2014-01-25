using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Objects
{
    [Serializable()]
    public abstract class Item
    {
        protected Guid id;
        protected string textureKey;
        protected Vector2 origin;
        protected Rectangle hitbox;

        public string TextureKey
        {
            get { return textureKey; }
            set { textureKey = value; }
        }

        public Guid Id
        {
            get { return id; }
        }

        public abstract Loot CreateLoot(Vector2 position);

        public abstract bool Use(Toon dude);
    }
}
