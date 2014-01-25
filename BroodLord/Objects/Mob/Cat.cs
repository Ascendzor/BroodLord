using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Objects
{
    [Serializable()]
    public class Cat : Mob
    {
        public Cat(Guid id, Vector2 position)
        {
            this.id = id;
            this.position = position;
            this.textureKey = "cat";
            this.origin = new Vector2(Data.GetTextureSize(textureKey).X / 2, Data.GetTextureSize(textureKey).Y * 0.85f);
            this.movementSpeed = 2;
            this.goalPosition = position;
            this.interactRange = 100;
            this.interactionCooldown = 5000;
            this.attackDamage = 60;
            this.health = 100;
            this.hitbox = new Rectangle((int)(position.X - origin.X), (int)(position.Y - origin.Y), (int)Data.GetTextureSize(textureKey).X, (int)Data.GetTextureSize(textureKey).Y);

            Map.InsertGameObject(this);

            MoveToPostionMobState MovPosState = new MoveToPostionMobState(new Vector2(position.X, position.Y), this);
            MoveToPostionMobState MovPosState2 = new MoveToPostionMobState(new Vector2(position.X - 100, position.Y), this);
            IdleMobState idleMobState = new IdleMobState(3, this);
            MovPosState.NextState = idleMobState;
            idleMobState.NextState = MovPosState2;
            MovPosState2.NextState = MovPosState;
            mobState = MovPosState;
        }

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

            foreach (Toon toon in Map.GetToons())
            {
                if (IsGameObjectNull() && IsGameObjectInsideRange(toon, 300))
                {
                    Client.SendEvent(new MoveToGameObjectEvent(GetId(), toon.GetId()));
                    MobState currentState = mobState;
                    currentState.IsActive = false;
                    mobState = new MoveToGameObjectMobState(toon, this);
                    mobState.NextState = currentState;
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
