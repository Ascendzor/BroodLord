using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Objects
{
    [Serializable()]
    public class HostileMob : Mob
    {

        
        public Vector2 GetRandomNewGoalPosition()
        {
            Random randomPositionGenerator = new Random();
            Vector2 newGoalPosition = new Vector2(
                GetGoalPosition().X + randomPositionGenerator.Next(-500, 500),
                GetGoalPosition().Y + randomPositionGenerator.Next(-500, 500));
            return newGoalPosition;
        } 

        protected override void Interact(GameObject gameObject)
        {
            if (DateTime.Now.CompareTo(interactionOffCooldown) < 0)
            {
                return;
            }

            goalPosition = position;

            //bad, change it to be more dynamic!
            if (gameObject is Toon)
            {
                InteractWithObject((Toon)gameObject);
            }
        }

        private void InteractWithObject(Toon toon)
        {
            Console.WriteLine("cat smacking the bitch: " + toon.GetId());
            interactionOffCooldown = DateTime.Now.AddMilliseconds(interactionCooldown); //<--- this allows the interaction to define the cooldown, ie chopping may take longer than attacking
            AttackMob(toon);
        }

        public void Behave()
        {
            if (IsGameObjectNull())
            {
                foreach (Toon toon in Map.GetToons())
                {
                    if (IsGameObjectInsideRange(toon, 300))
                    {
                        Client.SendEvent(new MoveToGameObjectEvent(GetId(), toon.GetId()));
                        MobState currentState = mobState;
                        currentState.IsActive = false;
                        mobState = new MoveToGameObjectMobState(toon, this);
                        mobState.NextState = currentState;
                    }
                }
            }

            if (mobState is MoveToGameObjectMobState)
            {
                HandleMobState((MoveToGameObjectMobState)mobState);
            }
            else if (mobState is MoveToPostionMobState)
            {
                HandleMobState((MoveToPostionMobState)mobState);
            }
            else if (mobState is IdleMobState)
            {
                HandleMobState((IdleMobState)mobState);
            }
        }

        public void HandleMobState(MoveToGameObjectMobState leState)
        {

            if (!IsGameObjectNull() && !IsGameObjectInsideRange(leState.GoalGameObject, 600))
            {
                Console.WriteLine("Cat not chasing no more");
                goalGameObject = null;
            }

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
    }
}
