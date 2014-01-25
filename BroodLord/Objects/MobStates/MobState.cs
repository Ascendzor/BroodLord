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
    public abstract class MobState
    {
        MobState nextState;
        bool active;
        public Mob mob;

        /*public void SetNextState(MobState mobState)
        {
            nextState = mobState;
        }*/

        public MobState NextState
        {
            get { return nextState; }
            set { nextState = value; }
        }

        public bool IsActive
        {
            get { return active; }
            set { active = value; }
        }

        public abstract bool CheckState();
    }
}
