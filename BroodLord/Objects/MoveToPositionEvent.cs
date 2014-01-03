using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Objects
{
    [Serializable()]
    public class MoveToPositionEvent : Event
    {
        public Vector2 Position;

        public MoveToPositionEvent(Guid id, Vector2 position)
        {
            this.Id = id;
            this.Position = position;
        }
    }
}
