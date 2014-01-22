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

        public HUD(ContentManager content, Toon dude, int screenHeight, int screenWidth)
        {
            spriteFont = content.Load<SpriteFont>("TestFont");
            this.dude = dude;
            topLeftPosition = new Vector2(-screenWidth/2, -screenHeight/2);
            bottomLeftPosition = new Vector2(-screenWidth/2, screenHeight/2);
        }

        public void Draw(SpriteBatch sb, Vector2 cameraPosition)
        {
            Vector2 drawPosition = new Vector2(0, 0);
            drawPosition = cameraPosition + topLeftPosition;
            String life = dude.Health.ToString();
            sb.DrawString(spriteFont, "Life: " + life + ", Energy Shield: 9001", drawPosition, Color.White);

            drawPosition = cameraPosition + bottomLeftPosition;

            // Draw inventory
            dude.Inventory.Draw(sb, drawPosition, spriteFont);

        }

    }
}
