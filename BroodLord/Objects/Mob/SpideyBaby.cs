using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Objects
{
    [Serializable()]
    public class SpideyBaby : PassiveMob
    {
        public SpideyBaby(Guid id, Vector2 position)
        {
            
            this.id = id;
            this.position = position;
            this.textureKey = "Spidey1";
            this.origin = new Vector2(Data.GetTextureSize(textureKey).X / 2, Data.GetTextureSize(textureKey).Y * 0.85f);
            this.movementSpeed = 2;
            this.interactRange = 100;
            this.interactionCooldown = 3000;
            this.attackDamage = 1;
            this.interactionCooldown = 5000;
            this.attackDamage = 0;
            this.health = 2;
            this.mobState = mobState;
            this.oldPosition = position;
            this.animation = 5;
            this.textureBase = "Spidey";
            this.animationTot = 2;
            isInteractable = true;
            
            Map.InsertGameObject(this);

            MoveToPostionMobState MovPosState = new MoveToPostionMobState(position, this);
            WanderMobState wanderState = new WanderMobState(this, position);
            MovPosState.NextState=wanderState;
            wanderState.NextState=MovPosState;
            mobState = wanderState;
        
        }

        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);

            if (Data.IsServer && health <= 0)
            {
                
                Client.SendEvent(new DeathEvent(GetId()));

                Client.SendEvent(new SpawnMeatEvent(Guid.NewGuid(), new Vector2(position.X + 50, position.Y)));
            }
        }
    }
}
