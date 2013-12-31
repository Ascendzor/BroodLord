using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Objects
{
    public class Loot : GameObject
    {
        protected string textureKeyOnGround;
        protected string textureKeyInBag;
        protected bool onGround;

        public void Draw(SpriteBatch sb)
        {
            if (onGround)
            {
                sb.Draw(Data.FindTexture[textureKeyOnGround], position, Color.White);
            }
            else
            {
                //dude is holding the rock
            }
        }
    }
}
