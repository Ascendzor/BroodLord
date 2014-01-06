using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Objects
{
    [Serializable()]
    public class Rock : Loot
    {
        public Rock(Vector2 position)
        {
            this.id = Guid.NewGuid();
            this.position = position;
            this.textureKey = "rock";
            this.textureKeyInBag = "rockBag";
            this.onGround = true;
            this.textureWidth = 40;
            this.textureHeight = 55;
            this.origin = new Vector2(textureWidth / 2, textureHeight * 0.85f);
            this.hitbox = new Rectangle((int)(position.X - origin.X), (int)(position.Y - origin.Y), (int)textureWidth, (int)textureHeight);

            Data.AddGameObject(this);
        }
    }
}
