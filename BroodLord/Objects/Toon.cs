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
    public class Toon : GameObject
    {
        private float movementSpeed;
        private Vector2 goalPosition;

        public Toon(Vector2 position, string textureKey, Map map)
        {
            this.id = Guid.NewGuid();
            this.position = position;
            this.textureKey = textureKey;
            this.movementSpeed = 10;
            this.goalPosition = position;
            this.map = map;
            colRadius = Data.toonRadius;

            xTileCoord = (int)position.X / map.GetTileSize();
            yTileCoord = (int)position.Y / map.GetTileSize();

            collidable = false;
        }

        public void ReceiveEvent(Event leEvent)
        {
            goalPosition = (Vector2)leEvent.value;
        }

        public void Update()
        {
            Vector2 moveDirection = goalPosition - position;
            if (moveDirection.Length() > 10)
            {
                moveDirection.Normalize();

                Vector2 newPos = position + moveDirection * movementSpeed;

                newPos = CheckCol(newPos);

                position = newPos;
                
            }
            
        }

        public void CheckGrid()
        {
            if (position.X < 0 || position.X > map.GetMapSize()*map.GetTileSize() || position.Y < 0 || position.Y > map.GetMapSize()*map.GetTileSize())//temp escape stopping code
            {
                position = new Vector2(50, 50);
            }

            int xNewCoords = (int)position.X / map.GetTileSize();
            int yNewCoords = (int)position.Y / map.GetTileSize();

            if (xTileCoord != xNewCoords || yTileCoord != yNewCoords)
            {
                map.GetTile(xTileCoord, yTileCoord).GetObjects().Remove(this);
                map.GetTile(xNewCoords, yNewCoords).GetObjects().Add(this);
                xTileCoord = xNewCoords;
                yTileCoord = yNewCoords;
            }
        }

        public Vector2 CheckCol(Vector2 newPos)
        {
            for(int x =-1; x<2;x++)
                for (int y = -1; y < 2; y++)
                {
                    int xTile = xTileCoord + x;
                    int yTile = yTileCoord + y;

                    if(xTile > 0 && xTile < map.GetMapSize() && yTile > 0 && yTile < map.GetMapSize())
                    {
                        foreach (GameObject gameObject in map.GetTile(xTile, yTile).GetObjects())
                        {
                            if (gameObject.IsCollidable())
                            {
                                if ((position.X > gameObject.Position.X - gameObject.GetColRadius() && position.X < gameObject.Position.X + gameObject.GetColRadius()) ||
                                    (newPos.X > gameObject.Position.X - gameObject.GetColRadius() && newPos.X < gameObject.Position.X + gameObject.GetColRadius()))
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
                }
            return newPos;
        }
        
    }
}