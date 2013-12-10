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

namespace Objects
{
    [Serializable()]
    public class Toon : GameObject
    {

        
        private float movementSpeed;
        private Vector2 goalPosition;

        public Toon(Vector2 position, string textureKey,Map map)
        {
            this.id = Guid.NewGuid();
            this.position = position;
            this.textureKey = textureKey;
            this.movementSpeed = 10;
            this.goalPosition = position;
            this.map = map;
            xTileCoord = (int)position.X / map.GetTileSize();
            yTileCoord = (int)position.Y / map.GetTileSize();

            map.GetTile(xTileCoord, yTileCoord).GetObjects().Add(this);
        }

        public Toon(Vector2 position, string textureKey)
        {
            this.id = Guid.NewGuid();
            this.position = position;
            this.textureKey = textureKey;
            this.movementSpeed = 10;
            this.goalPosition = position;
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
                
                position += moveDirection * movementSpeed;
                
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
        
    }
}