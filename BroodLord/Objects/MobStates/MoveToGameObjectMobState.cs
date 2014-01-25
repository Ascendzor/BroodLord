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
    public class MoveToGameObjectMobState : MobState
    {
        GameObject gameObject;

        public MoveToGameObjectMobState(GameObject gameObject, Mob mob)
        {
            this.gameObject = gameObject;
            this.mob = mob;
        }

        public GameObject GoalGameObject
        {
            get { return gameObject;}
            set { gameObject = value;}
        }

        public override bool CheckState()
        {
            if (mob.GetGoalGameObject() == null)
            {
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
            
        }
    }
}
