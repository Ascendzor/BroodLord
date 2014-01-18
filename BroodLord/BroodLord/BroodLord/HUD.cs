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
    public class HUD
    {
        private SpriteFont spriteFont;
        private Toon dude;
        private Vector2 topLeftPosition;
        private Vector2 bottomLeftPosition;        

        public HUD(ContentManager content, Toon dude, int halfScreenHeight, int halfScreenWidth)
        {
            spriteFont = content.Load<SpriteFont>("TestFont");
            this.dude = dude;
            topLeftPosition = new Vector2(-halfScreenWidth, -halfScreenHeight);
            bottomLeftPosition = new Vector2(-halfScreenWidth, halfScreenHeight-25);
        }

        public void Draw(SpriteBatch sb, Vector2 cameraPosition)
        {
            Vector2 drawPosition = new Vector2(0, 0);
            drawPosition = cameraPosition + topLeftPosition;
            sb.DrawString(spriteFont, "Life: 1, Energy Shield: 9001", drawPosition, Color.White);

            drawPosition = cameraPosition + bottomLeftPosition;
            sb.DrawString(spriteFont, inventoryItems(), drawPosition, Color.White);

            drawPosition.Y -= (Data.FindTexture["inventorySlots"].Height);
            sb.Draw(Data.FindTexture["inventorySlots"], drawPosition, Color.White);

            foreach (Item l in dude.Inventory.Items)
            {
                sb.Draw(Data.FindTexture[l.TextureKey], drawPosition, Color.White);
                sb.DrawString(spriteFont, l.Quantity.ToString(), drawPosition, Color.White);
                drawPosition.X += 75;
            }
        }

        /// <summary>
        /// Testing only
        /// </summary>
        /// <returns>returns players items as a string</returns>
        private String inventoryItems()
        {
            String itemsAsText = "Inventory: ";
            foreach (Item l in dude.Inventory.Items)
            {
                itemsAsText += " (" + l.Quantity + ") " + l.GetType() + ", ";
            }
            return itemsAsText;
        }
    }
}
