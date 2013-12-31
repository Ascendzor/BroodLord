﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Objects
{
    public class Item
    {
        protected Map map;
        protected Vector2 position;
        protected string textureKeyOnGround;
        protected string textureKeyInBag;
        protected bool onGround;

        public void Draw(SpriteBatch sb)
        {
            if (onGround)
            {
                sb.Draw(Data.findTexture[textureKeyOnGround], position, Color.White);
            }
            else
            {
                //draw the rock 
            }
        }
    }
}