using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Objects
{
    [Serializable()]
    public class Swampert : HostileMob
    {
        
        public Swampert(Guid id, Vector2 position)
        {
            
            this.id = id;
            this.position = position;
            this.textureKey = "cat";
            this.origin = new Vector2(Data.GetTextureSize(textureKey).X / 2, Data.GetTextureSize(textureKey).Y * 0.85f);
            this.movementSpeed = 2;
            this.interactRange = 100;
            this.interactionCooldown = 5000;
            this.attackDamage = 6;
            this.health = 100;
            this.mobState = mobState;
            isInteractable = true;
            
            Map.InsertGameObject(this);

            MoveToPostionMobState MovPosState = new MoveToPostionMobState(new Vector2(position.X, position.Y), this);
            MoveToPostionMobState MovPosState2 = new MoveToPostionMobState(new Vector2(position.X - 200, position.Y), this);
            IdleMobState idleMobState = new IdleMobState(3, this);
            MovPosState.NextState = idleMobState;
            idleMobState.NextState = MovPosState2;
            MovPosState2.NextState = MovPosState;
            mobState = MovPosState;
        
        }

        public override Rectangle GetHitBox()
        {
            return new Rectangle((int)(position.X - origin.X), (int)(position.Y - origin.Y), (int)Data.GetTextureSize(textureKey).X, (int)Data.GetTextureSize(textureKey).Y);
        }

        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);

            if (Data.IsServer && health <= 0)
            {
                
                Client.SendEvent(new DeathEvent(GetId()));
                Client.SendEvent(new SpawnMeatEvent(Guid.NewGuid(), new Vector2(position.X - 50, position.Y)));
                Client.SendEvent(new SpawnMeatEvent(Guid.NewGuid(), new Vector2(position.X + 50, position.Y)));
                Client.SendEvent(new SpawnMeatEvent(Guid.NewGuid(), new Vector2(position.X + 50, position.Y)));
                Client.SendEvent(new SpawnMeatEvent(Guid.NewGuid(), new Vector2(position.X + 50, position.Y)));
            }
        }
    }
}
