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
    class WanderMobState : MobState
    {
        Vector2 position;

        public WanderMobState(Mob mob, Vector2 position)
        {
            this.position = position;
            this.mob = mob;
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
            Random random = new Random();

            int  x = random.Next(500) ;
            int xdir = random.Next(0, 1);
            if (xdir == 0)
                xdir = -1;

            int y = random.Next(500);
            int ydir = random.Next(0, 1);
            if (ydir == 0)
                ydir = -1;

            Vector2 newPos = new Vector2(position.X + x * xdir, position.Y + y * ydir);
            mob.SetGoalPosition(newPos);
            Client.SendEvent(new MoveToPositionEvent(mob.GetId(), newPos));
        }
    }
}
