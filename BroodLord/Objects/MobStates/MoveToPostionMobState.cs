using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;

namespace Objects
{
    [Serializable()]
    public class MoveToPostionMobState : MobState
    {
        Vector2 position;

        public MoveToPostionMobState(Vector2 position, Mob mob)
        {
            this.position = position;
            this.mob = mob;
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public override bool CheckState()
        {
            if (mob.AtGoalPosition())
            {
                Console.WriteLine("Cat Changing State");
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Activate()
        {
            IsActive = true;
            mob.SetGoalPosition(position);
            Client.SendEvent(new MoveToPositionEvent(mob.GetId(), Position));
        }
    }
}
