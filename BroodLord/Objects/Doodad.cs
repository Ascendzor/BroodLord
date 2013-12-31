using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Objects
{
    public class Doodad : GameObject
    {
        protected int collisionWidth;

        public int GetCollisionWidth()
        {
            return collisionWidth;
        }
    }
}
