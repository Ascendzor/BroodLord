using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace Objects
{
    [Serializable()]
    public class WoodLoot : Loot
    {
        public WoodLoot(Guid id, Vector2 position) : base()
        {
            this.id = id;
            this.position = position;
            this.textureKey = "wood";
            this.origin = new Vector2(Data.GetTextureSize(textureKey).X / 2, Data.GetTextureSize(textureKey).Y * 0.85f);
            this.hitbox = new Rectangle((int)(position.X - origin.X), (int)(position.Y - origin.Y), (int)Data.GetTextureSize(textureKey).X, (int)Data.GetTextureSize(textureKey).Y);
            this.quantity = 1;

            Map.InsertGameObject(this);
        }
    }
}
