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

        private Dictionary<Keys, Action> keyboardKeys;

        public Input(Toon dude)
        {
            oldState = Mouse.GetState();
            this.dude = dude;

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

            #region rightClick
            if (oldState.RightButton != ButtonState.Pressed)
            {
                if (nowState.RightButton == ButtonState.Pressed)
                {
                    RightClick(nowState);
                }
            }
            #endregion endOfRightClick

            #region middleClick
            if (oldState.MiddleButton != ButtonState.Pressed)
            {
                if (nowState.MiddleButton == ButtonState.Pressed)
                {
                    MiddleClick(nowState);
                }
            }
            #endregion middleClick

            oldState = nowState;
        }

        private void MiddleClick(MouseState nowState)
        {
            if (dude.Inventory.inventoryClick(nowState, dude))
                return; 
        }

        /// <summary>
        /// 
        /// </summary>
        private void RightClick(MouseState nowState)
        {
            if (dude.Inventory.inventoryClick(nowState, dude))
                return;            
        }

        //check if the dude clicked on something
        //if he clicked on something then move him to that thing
        //move him to where he clicked
        private void LeftClick(MouseState nowState)
        {
            // Do inventory click
            if (dude.Inventory.inventoryClick(nowState, dude))
                return;
            
            Vector2 clickPosition = new Vector2(nowState.X - (Game1.graphicsDevice.Viewport.Width * 0.5f), nowState.Y - (Game1.graphicsDevice.Viewport.Height * 0.5f));
            clickPosition = dude.Position + clickPosition;

            List<Tile> tiles = Map.GetRenderedTiles(dude.Position);

            //NOT YET IMPLEMENTED: find all gameObjects that you have clicked on, find which one you are closest to 
            //the center of and click on that one, this allows you to click on something behind something else
            foreach (Tile tile in tiles)
            {
                foreach (GameObject gameObject in tile.GetGameObjects())
                {
                    if (gameObject.IsInteractable)
                    {
                        //if you clicked on a game object, go to that game object
                        if (gameObject.GetHitBox().Contains((int)clickPosition.X, (int)clickPosition.Y))
                        {
                            Event LeftClickEventz = new MoveToGameObjectEvent(dude.GetId(), gameObject.GetId());
                            Client.SendEvent(LeftClickEventz);
                            return;
                        }
                    }
                }
            }

            Event LeftClickEvent = new MoveToPositionEvent(dude.GetId(), clickPosition);
            Client.SendEvent(LeftClickEvent);
        }
            
    }
}
