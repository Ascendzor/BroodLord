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
        protected DateTime lastInteractionTimestamp;
        protected DateTime interactionOffCooldown;
        protected Dictionary<Guid, Loot> inventory;

        /*public Mob(Vector2 position, string textureKey, Guid id, Vector2 origin, Rectangle hitbox, Client client) //: base(position, textureKey, id, origin, hitbox, client)
        {

        }*/

        public override void ReceiveEvent(Event leEvent)
        {
            if (leEvent is MoveToPositionEvent)
            {
                goalPosition = ((MoveToPositionEvent)leEvent).Position;
                goalGameObject = null;
            }
            else if (leEvent is MoveToGameObjectEvent)
            {
                MoveToGameObjectEvent mtgoe = (MoveToGameObjectEvent)leEvent;
                goalGameObject = Data.FindGameObject[mtgoe.GoalGameObject];
                goalPosition = goalGameObject.Position;
            }
            else if (leEvent is ChopEvent)
            {
                //do chop animation
            }
        }
        
        //All non-event behaviour is handled in Update.
        //This means basically only Moving is handled in Update.
        //Check if the move is unnecessary (close enough to target)
        //move and update the grid with where you have moved
        public void Update()
        {
            //goalPosition = new Vector2(500, 500);
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
            position = newPos;
            CheckGrid();
        }

        private void CheckGrid()
        {
            if (position.X < 0 || position.X > Map.GetMapSize() * Map.GetTileSize() || position.Y < 0 || position.Y > Map.GetMapSize() * Map.GetTileSize())//temp escape stopping code
            {
                position = new Vector2(50, 50);
            }
            int xNewCoords = (int)(position.X / Map.GetTileSize());
            int yNewCoords = (int)(position.Y / Map.GetTileSize());

            if (xTileCoord != xNewCoords || yTileCoord != yNewCoords)
            {
                Map.GetTile(xTileCoord, yTileCoord).GetObjects().Remove(this);
                Map.GetTile(xNewCoords, yNewCoords).GetObjects().Add(this);
                xTileCoord = xNewCoords;
                yTileCoord = yNewCoords;
            }
        }

        private Vector2 CheckCol(Vector2 newPos)
        {
            for (int x = -1; x < 2; x++)
                for (int y = -1; y < 2; y++)
                {
                    int xTile = xTileCoord + x;
                    int yTile = yTileCoord + y;

                    if (xTile > 0 && xTile < Map.GetMapSize() && yTile > 0 && yTile < Map.GetMapSize())
                    {
                        foreach (Doodad gameObject in Map.GetTile(xTile, yTile).GetDoodads())
                        {
                            if ((position.X > gameObject.Position.X - gameObject.GetCollisionWidth() && position.X < gameObject.Position.X + gameObject.GetCollisionWidth()) ||
                                (newPos.X > gameObject.Position.X - gameObject.GetCollisionWidth() && newPos.X < gameObject.Position.X + gameObject.GetCollisionWidth()))
                            {
                                float a = position.Y - gameObject.Position.Y;
                                float b = newPos.Y - gameObject.Position.Y;

                                if (a / Math.Abs(a) + b / Math.Abs(b) == 0)
                                {
                                    newPos.Y += 1.1f * -b;
                                }
                            }

                            /*
                            Vector2 midDir = position - gameObject.Position;
                            int totRadius = colRadius + gameObject.GetColRadius();
                            float midLen = midDir.Length();
                            if (midLen < totRadius)
                            {
                                float distanceToMove = totRadius - midLen;
                                midDir.Normalize();
                                position += midDir * distanceToMove;
                            }*/
                        }
                    }
                }
            return newPos;
        }

        protected virtual void Interact(GameObject gameObject)
        {
            //start cooldown
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
                    1 - (position.Y / (Map.GetMapSize() * Map.GetTileSize())));
        }
    }
}
