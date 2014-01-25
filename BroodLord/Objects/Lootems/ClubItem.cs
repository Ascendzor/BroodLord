using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Objects
{
    [Serializable()]
    public class ClubItem : Item
    {
        private const int HEAL_AMOUNT = 4000;
        public ClubItem(Guid id)
            : base()
        {
            this.id = id;
            this.textureKey = "ClubBag";
            this.origin = new Vector2(Data.GetTextureSize(textureKey).X / 2, Data.GetTextureSize(textureKey).Y * 0.85f);
            this.hitbox = new Rectangle(0, 0, 0, 0); //set this when going to click on the item
        }

        public override Loot CreateLoot(Vector2 position)
        {
            return new ClubLoot(id, position);
        }

        public override bool Use(Toon dude)
        {
            return false;
        }
    }
}
