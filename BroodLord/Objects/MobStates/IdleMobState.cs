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
using System.Threading;
using System.Diagnostics;

namespace Objects
{
    [Serializable()]
    public class IdleMobState : MobState
    {
        int lengthTime;
        DateTime start;
        public IdleMobState(int lengthTime, Mob mob)
        {
            this.lengthTime = lengthTime;
            
        }


        public override bool CheckState()
        {
            if ((DateTime.Now - start).TotalSeconds > lengthTime)//stopWatch.ElapsedMilliseconds/100 > lengthTime)
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
            start = DateTime.Now;
            
        }
    }
}
