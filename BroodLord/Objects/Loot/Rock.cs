﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Objects
{
    public class Rock : Loot
    {
        public Rock(Vector2 position)
        {
            this.id = Guid.NewGuid();
            this.position = position;
            this.textureKey = "rock";
            this.textureKeyInBag = "rockBag";
            this.onGround = true;
            this.origin = new Vector2(Data.FindTexture[textureKey].Width / 2, Data.FindTexture[textureKey].Height * 0.85f);
            this.hitbox = new Rectangle((int)(position.X - origin.X), (int)(position.Y - origin.Y), Data.FindTexture[textureKey].Width, Data.FindTexture[textureKey].Height);
            this.quantity = 1;

            Map.GetTile((int)(position.X/Map.GetTileSize()), (int)(position.Y/Map.GetTileSize())).InsertThing(this);

            Data.AddGameObject(this);
        }
    }
}
