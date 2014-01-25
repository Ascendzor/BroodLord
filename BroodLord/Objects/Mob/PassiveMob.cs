using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Objects
{
    [Serializable()]
    public class PassiveMob : Mob
    {

        public void Behave()
        {
            if (mobState is MoveToPostionMobState)
            {
                HandleMobState((MoveToPostionMobState)mobState);
            }
            else if (mobState is IdleMobState)
            {
                HandleMobState((IdleMobState)mobState);
            }else if (mobState is WanderMobState)
            {
                HandleMobState((WanderMobState)mobState);
            }
        }

        public void HandleMobState(MoveToPostionMobState leState)
        {
            if (!leState.IsActive)
            {
                leState.Activate();
            }
            else if (leState.CheckState())
            {
                leState.IsActive = false;
                mobState = leState.NextState;
            }
        }

        public void HandleMobState(IdleMobState leState)
        {
            if (!leState.IsActive)
            {
                leState.Activate();
            }
            else if (leState.CheckState())
            {
                leState.IsActive = false;
                mobState = leState.NextState;
            }
        }

        public void HandleMobState(WanderMobState leState)
        {
            if (!leState.IsActive)
            {
                leState.Activate();
            }
            else if (leState.CheckState())
            {
                leState.IsActive = false;
                mobState = leState.NextState;
            }
        }
    }
}
