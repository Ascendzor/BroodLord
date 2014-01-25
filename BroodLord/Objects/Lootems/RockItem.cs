using System;
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
            this.textureKey = "RockBag";
            this.origin = new Vector2(Data.GetTextureSize(textureKey).X / 2, Data.GetTextureSize(textureKey).Y * 0.85f);
            this.hitbox = new Rectangle(0, 0, 0, 0); //set this when going to click on the item
        }

        public override Loot CreateLoot(Vector2 position)
        {
            return new RockLoot(id, position);
        }

        public override bool Use(Toon dude)
        {
            return false;
        }
    }
}
