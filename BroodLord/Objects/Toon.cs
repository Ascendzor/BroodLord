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
    public class Toon
    {
        private Guid id;
        private Vector2 position;
        private string textureKey;
        private float movementSpeed;
        private Vector2 goalPosition;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
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

        public void Draw(SpriteBatch sb, Texture2D texture)
        {
            sb.Draw(texture, position, Color.White);
        }

        public Guid GetId()
        {
            return id;
        }
    }
}