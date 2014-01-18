using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Objects
{
    [Serializable()]
    public class Cat : Mob
    {
        public Cat(Guid id, Vector2 position)
        {
            this.id = id;
            this.position = position;
            this.textureKey = "cat";
            this.origin = new Vector2(Data.GetTextureSize(textureKey).X / 2, Data.GetTextureSize(textureKey).Y * 0.85f);
            this.movementSpeed = 1;
            this.goalPosition = position;

            Map.InsertGameObject(this);
        }
    }
}
