using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Threading;

namespace Objects
{
    [Serializable()]
    public class BrokenShip : Doodad
    {
        public BrokenShip(Guid id, Vector2 position, string textureKey)
        {
            this.id = id;
            this.position = position;
            this.textureKey = textureKey;
            this.collisionWidth = Data.TreeRadius;
            this.origin = new Vector2(Data.GetTextureSize(textureKey).X / 2, Data.GetTextureSize(textureKey).Y * 0.90f);
            this.hitbox = new Rectangle((int)(position.X - origin.X), (int)(position.Y - origin.Y), (int)Data.GetTextureSize(textureKey).X, (int)Data.GetTextureSize(textureKey).Y);
            this.isInteractable = true;

            Map.InsertGameObject(this);
        }
    }
}