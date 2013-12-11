using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Objects;

namespace BroodLord
{
    class MapThing
    {
        // can be collidable
        // can be interactable
        Texture2D texture;
        Vector2 position;

        public MapThing(ContentManager Content)
        {
            position = new Vector2(0, 0);
            texture = Content.Load<Texture2D>("tree");
        }

        public MapThing(ContentManager Content, Vector2 pos)
        {
            position = pos;
            texture = Content.Load<Texture2D>("tree");
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, new Vector2(position.X - (texture.Width * 0.5f), position.Y - texture.Height), Color.White);
        }
    }
}
