﻿using System;
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
        private Map map;

        private Dictionary<Keys, Action> keyboardKeys;

        public Input(Toon dude, Client client, Map map)
        {
            oldState = Mouse.GetState();
            this.dude = dude;
            this.client = client;
            this.map = map;

            this.keyboardKeys = new Dictionary<Keys, Action>();
        }

        public void Update()
        {
            CheckMouse();
        }

        //disgusting mouse code below
        public void CheckMouse()
        {
            MouseState nowState = Mouse.GetState();

            #region leftClick
            if (oldState.LeftButton != ButtonState.Pressed)
            {
                if (nowState.LeftButton == ButtonState.Pressed)
                {
                    LeftClick(nowState);
                }
            }
            #endregion endOfLeftClick

            oldState = nowState;
        }

        //check if the dude clicked on something
        //if he clicked on something then move him to that thing
        //move him to where he clicked
        private void LeftClick(MouseState nowState)
        {
            Vector2 clickPosition = new Vector2(nowState.X - (Game1.graphicsDevice.Viewport.Width * 0.5f), nowState.Y - (Game1.graphicsDevice.Viewport.Height * 0.5f));
            clickPosition = dude.Position + clickPosition;

            List<Tile> tiles = map.GetRenderedTiles(dude.GetGridCoordX(), dude.GetGridCoordY());
            foreach (Tile tile in tiles)
            {
                foreach (GameObject gameObject in tile.GetObjects())
                {
                    if (gameObject is Loot)
                    {
                        Console.WriteLine("le loot");
                    }
                    if (gameObject.GetHitbox().Contains((int)clickPosition.X, (int)clickPosition.Y))
                    {
                        Event LeftClickEventz = new Event(dude.GetId(), gameObject.Position);
                        dude.ReceiveEvent(LeftClickEventz);
                        client.SendEvent(LeftClickEventz);
                        Console.WriteLine("le click on le game object");
                        return;
                    }
                }
            }

            Event LeftClickEvent = new Event(dude.GetId(), clickPosition);
            dude.ReceiveEvent(LeftClickEvent);
            client.SendEvent(LeftClickEvent);
        }
    }
}
