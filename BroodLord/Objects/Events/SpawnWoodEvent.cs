﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Objects
{
    [Serializable()]
    public class SpawnWoodEvent : Event
    {
        public Guid WoodId;
        public Vector2 Position;

        public SpawnWoodEvent(Guid id, Vector2 position)
        {
            this.Id = id;
            this.Position = position;
        }
    }
}