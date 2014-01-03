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


            //NOT YET IMPLEMENTED: find all gameObjects that you have clicked on, find which one you are closest to the center of and click on that one, this allows you to click on something behind something else
            foreach (Tile tile in tiles)
            {
                foreach (GameObject gameObject in tile.GetObjects())
                {
                    //if you clicked on a game object, go to that game object
                    if (gameObject.GetHitbox().Contains((int)clickPosition.X, (int)clickPosition.Y))
                    {
                        Event LeftClickEventz = new MoveToGameObjectEvent(dude.GetId(), gameObject.GetId());
                        dude.ReceiveEvent(LeftClickEventz);
                        client.SendEvent(LeftClickEventz);
                        return;
                    }
                }
            }

            Event LeftClickEvent = new MoveToPositionEvent(dude.GetId(), clickPosition);
            dude.ReceiveEvent(LeftClickEvent);
            client.SendEvent(LeftClickEvent);
        }
    }
}
