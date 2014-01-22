﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Objects
{
    [Serializable()]
    public class RockItem : Item
    {
        public RockItem(Guid id) : base()
        {
            this.id = id;
            this.textureKey = "rock";
            this.origin = new Vector2(Data.GetTextureSize(textureKey).X / 2, Data.GetTextureSize(textureKey).Y * 0.85f);
            this.hitbox = new Rectangle(0, 0, 0, 0); //set this when going to click on the item
            this.quantity = 1;
        }
    }
}