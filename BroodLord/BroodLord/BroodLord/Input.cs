using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BroodLord
{
    class Input
    {
        private MouseState oldState;
        private Toon dude;
        private Client client;
        

        public Input(Toon dude, Client client)
        {
            oldState = Mouse.GetState();
            this.dude = dude;
            this.client = client;
        }

        public void Update(GraphicsDevice gd)
        {
            MouseState nowState = Mouse.GetState();
            if (oldState.LeftButton != ButtonState.Pressed)
            {
                if (nowState.LeftButton == ButtonState.Pressed)
                {
                    Vector2 clickPosition = new Vector2(nowState.X - (gd.Viewport.Width * 0.5f), nowState.Y - (gd.Viewport.Height * 0.5f));
                    clickPosition = dude.Position + clickPosition;
                    Console.WriteLine(clickPosition);
                    Event LeftClickEvent = new Event(dude.GetId(), clickPosition);
                    dude.ReceiveEvent(LeftClickEvent);
                    client.SendEvent(LeftClickEvent);
                }
            }

            oldState = nowState;
        }
    }
}
