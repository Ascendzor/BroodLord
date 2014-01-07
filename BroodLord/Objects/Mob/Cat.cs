using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Objects
{
    [Serializable()]
    public class Cat : Mob
    {
        public Cat(Guid id, Vector2 position)
        {
            this.id = id;
            this.position = position;
            this.textureKey = "cat";
            this.textureWidth = 197;
            this.textureHeight = 240;
            this.origin = new Vector2(textureWidth / 2, textureHeight * 0.85f);

            Data.AddGameObject(this);
        }
    }
}
