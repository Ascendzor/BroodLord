using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Objects
{
    [Serializable()]
    public class CoconutLoot : Loot
    {
        public CoconutLoot(Guid id, Vector2 position)
            : base()
        {
            this.id = id;
            this.position = position;
            this.textureKey = "Coconut";
            this.origin = new Vector2(Data.GetTextureSize(textureKey).X / 2, Data.GetTextureSize(textureKey).Y * 0.85f);
            this.hitbox = new Rectangle((int)(position.X - origin.X), (int)(position.Y - origin.Y), (int)Data.GetTextureSize(textureKey).X, (int)Data.GetTextureSize(textureKey).Y);
            this.stackable = true;

            Map.InsertGameObject(this);
        }

        public override Item CreateItem(Loot loot)
        {
            return new CoconutItem(loot.GetId());
        }
    }
}