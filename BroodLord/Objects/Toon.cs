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

        public Toon(Vector2 position, string textureKey)
        {
            this.id = Guid.NewGuid();
            this.position = position;
            this.textureKey = textureKey;
        }

        public void ReceiveEvent(Event leEvent)
        {
            position = (Vector2)leEvent.value;
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
