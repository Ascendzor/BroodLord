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
        int count;

        public WanderMobState(Vector2 position, Mob mob)
        {
            this.position = position;
            this.mob = mob;
            this.count = 5;
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
                if (count == 0)
                {
                    Console.WriteLine("Cat Changing State");
                    return true;
                }
                else
                {
                    
                    count--;
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public void ChangePosition()
        {

        }

        public void Activate()
        {
            IsActive = true;
            mob.SetGoalPosition(position);
            Client.SendEvent(new MoveToPositionEvent(mob.GetId(), Position));
        }
    }
}
