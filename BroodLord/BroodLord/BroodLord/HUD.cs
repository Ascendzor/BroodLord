﻿using System;
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
        private Vector2 bottomRightPosition;
        private Vector2 bottomMiddlePosition;

        public HUD(ContentManager content, Toon dude, int screenHeight, int screenWidth)
        {
            spriteFont = content.Load<SpriteFont>("TestFont");
            this.dude = dude;
            topLeftPosition = new Vector2(-screenWidth / 2, -screenHeight / 2);
            bottomLeftPosition = new Vector2(-screenWidth / 2, screenHeight / 2);
            bottomRightPosition = new Vector2(screenWidth / 2, screenHeight / 2);
            bottomMiddlePosition = new Vector2(0, screenHeight / 2);
        }

        public void Draw(SpriteBatch sb, Vector2 cameraPosition)
        {
            Vector2 drawPosition = new Vector2(0, 0);
            drawPosition = cameraPosition + topLeftPosition;
            String life = dude.Health.ToString();
            sb.DrawString(spriteFont, "Life: " + life +
                ", Energy Shield: 9001, Hunger: " + dude.Hunger +
                ", Thirst: " + dude.Thirst, drawPosition, Color.White);


            // Draw Health
            drawPosition = cameraPosition + bottomMiddlePosition;
            drawPosition.Y -= Data.FindTextureSize["health1"].Y;
            drawPosition.X -= Data.FindTextureSize["health1"].X * (dude.MaxHealth / 2);
            for (int i = 1; i < dude.Health + 1; i++)
            {
                sb.Draw(Data.FindTexture["health" + i.ToString()], drawPosition, Color.White);
                drawPosition.X += Data.FindTextureSize["health" + i.ToString()].X;
            }

            // Draw Hunger
            drawPosition = cameraPosition + bottomMiddlePosition;
            drawPosition.Y -= Data.FindTextureSize["hunger1"].Y*2;
            int numberOfSegments = (dude.Hunger / (dude.MaxHunger / 10)) + 1;
            for (int i = 1; i < numberOfSegments; i++)
            {
                sb.Draw(Data.FindTexture["hunger" + i.ToString()], drawPosition, Color.White);
                drawPosition.X += Data.FindTextureSize["hunger" + i.ToString()].X;
            }

            // Draw Hunger
            drawPosition = cameraPosition + bottomMiddlePosition;
            drawPosition.Y -= Data.FindTextureSize["hydration1"].Y * 2;
            drawPosition.X -= Data.FindTextureSize["hydration1"].Y * 2;
            numberOfSegments = (dude.Hunger / (dude.MaxHunger / 10)) + 1;
            for (int i = 1; i < numberOfSegments; i++)
            {
                sb.Draw(Data.FindTexture["hydration" + i.ToString()], drawPosition, Color.White);
                drawPosition.X += Data.FindTextureSize["hydration" + i.ToString()].X;
            }

            drawPosition = cameraPosition + bottomLeftPosition;

            // Draw inventory
            dude.Inventory.Draw(sb, drawPosition, spriteFont);

        }


    }
}
