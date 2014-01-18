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

        /*public Mob(Vector2 position, string textureKey, Guid id, Vector2 origin, Rectangle hitbox) //: base(position, textureKey, id, origin, hitbox, client)
        {

        }*/

        public void ReceiveEvent(MoveToPositionEvent leEvent)
        {
            goalPosition = leEvent.Position;
            goalGameObject = null;
        }

        public void ReceiveEvent(MoveToGameObjectEvent leEvent)
        {
            goalGameObject = Map.GetGameObject(leEvent.GoalGameObject);
        }

        public void ReceiveEvent(ChopEvent leEvent)
        {
        }
        
        //All non-event behaviour is handled in Update.
        //This means basically only Moving is handled in Update.
        //Check if the move is unnecessary (close enough to target) before moving
        public void Update()
        {
            if (goalGameObject != null)
            {
                goalPosition = goalGameObject.Position;
            }
            Vector2 moveDirection = goalPosition - position;
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
            oldPosition = new Vector2(position.X, position.Y);
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

        public Vector2 GetGoalPosition()
        {
            return goalPosition;
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
