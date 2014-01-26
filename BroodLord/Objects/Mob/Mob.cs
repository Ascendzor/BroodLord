 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Objects
{
    [Serializable()]
    public class Mob : GameObject
    {
        protected float movementSpeed;
        protected Vector2 goalPosition;
        protected int interactRange;
        protected GameObject goalGameObject;
        protected enum States {Idle, Moving, TreeCutting, Attacking, Crafting}
        protected States state;
        protected double attackDamage;
        protected double interactionCooldown;
        protected DateTime interactionOffCooldown;
        protected Inventory inventory;
        protected int health;
        protected MobState mobState;


        public GameObject GoalGameObject
        {
            get {return goalGameObject;}
            set {goalGameObject = value;}
        }
        /*public Mob(Vector2 position, string textureKey, Guid id, Vector2 origin, Rectangle hitbox) //: base(position, textureKey, id, origin, hitbox, client)
        {

        }*/

        public override Rectangle GetHitBox()
        {
            return new Rectangle((int)(position.X - origin.X), (int)(position.Y - origin.Y), (int)Data.GetTextureSize(textureKey).X, (int)Data.GetTextureSize(textureKey).Y);
        }
        public virtual void ReceiveEvent(DeathEvent leEvent)
        {
            Map.ErradicateGameObject(leEvent.Id);
        }


        public void ReceiveEvent(MoveToPositionEvent leEvent)
        {
            goalPosition = leEvent.Position;
            goalGameObject = null;
        }

        public void ReceiveEvent(MoveToGameObjectEvent leEvent)
        {
            goalGameObject = Map.GetGameObject(leEvent.GoalGameObject);
        }

        
        //All non-event behaviour is handled in Update.
        //This means basically only Moving is handled in Update.
        //Check if the move is unnecessary (close enough to target) before moving
        public virtual void Update()
        {
            Vector2 moveDirection;
            if (GetGoalGameObject() != null)
            {
                moveDirection = GetGoalGameObject().Position - position;
            }
            else
            {
                moveDirection = GetGoalPosition() - position;
            }

            if (moveDirection.Length() <= 10)
            {
                return;
            }

            if (goalGameObject != null)
            {
                if ((position - goalGameObject.Position).Length() < interactRange)
                {
                    Interact(goalGameObject);
                    return;
                }
            }

            moveDirection.Normalize();
            Vector2 newPos = position + moveDirection * movementSpeed;
            newPos = CheckCol(newPos);
            position = newPos;
        }

        private Vector2 CheckCol(Vector2 newPos)
        {
            foreach (Doodad doodad in Map.GetCollidableDoodads(this))
            {
                if ((position.X > doodad.Position.X - doodad.GetCollisionWidth() && position.X < doodad.Position.X + doodad.GetCollisionWidth()) ||
                                (newPos.X > doodad.Position.X - doodad.GetCollisionWidth() && newPos.X < doodad.Position.X + doodad.GetCollisionWidth()))
                {
                    float a = position.Y - doodad.Position.Y;
                    float b = newPos.Y - doodad.Position.Y;

                    if (a / Math.Abs(a) + b / Math.Abs(b) == 0)
                    {
                        newPos.Y += 1.1f * -b;
                    }
                }
            }
            return newPos;
        }

        protected virtual void Interact(GameObject gameObject)
        {
        }

        public GameObject GetGoalGameObject()
        {
            return goalGameObject;
        }

        public bool IsGameObjectNull()
        {
            return GetGoalGameObject() == null;
        }

        public bool IsGameObjectInsideRange(GameObject gameObject, int range)
        {
            return (gameObject.Position - Position).Length() < range;
        }

        public Vector2 GetGoalPosition()
        {
            return goalPosition;
        }

        public void SetGoalPosition(Vector2 newPos)
        {
            goalPosition = newPos;
        }

        public double GetAttackDamage()
        {
            return attackDamage;
        }

        public bool AtGoalPosition()
        {
            return (GetGoalPosition() - Position).Length() < 10;
        }

        public void AttackMob(Mob mob)
        {
            mob.TakeDamage((int)GetAttackDamage());
        }

        public virtual void TakeDamage(int mobDamage)
        {
            health -= mobDamage;
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(Data.FindTexture[textureKey],
                new Rectangle((int)position.X,
                    (int)position.Y,
                    Data.FindTexture[textureKey].Width, Data.FindTexture[textureKey].Height),
                    null,
                    Color.White,
                    0,
                    origin,
                    SpriteEffects.None,
                    1 - (position.Y / (Data.MapSize * Data.TileSize)));
        }
    }
}
