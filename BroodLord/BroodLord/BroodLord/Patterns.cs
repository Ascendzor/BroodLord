using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Objects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BroodLord
{
    public static class Patterns
    {
        public static List<Pattern> patterns;
        public static Vector2 camPos;
        public static Rectangle boundsOnScreen;

        public static void Initilize(Vector2 camPos_)
        {
            camPos = camPos_;
            patterns = new List<Pattern>();
            patterns.Add(new Pattern(new List<String>(new String[]{"Rock", "Club"}), "HammerItem"));
            patterns.Add(new Pattern(new List<String>(new String[] { "Rock", "Club" }), "HammerItem"));
            patterns.Add(new Pattern(new List<String>(new String[] { "Rock", "Club" }), "HammerItem"));
            boundsOnScreen = new Rectangle(0, 0, (int)Data.FindTextureSize["EmptyInventorySlot"].X, (int)Data.FindTextureSize["EmptyInventorySlot"].Y * patterns.Count);
        }

        public static void Draw(SpriteBatch sb, Vector2 drawPosition)
        {
            Vector2 posi = new Vector2(boundsOnScreen.X, boundsOnScreen.Y);
            foreach (Pattern pat in patterns)
            {
                //pat.Draw(sb, posi + cameraPosition);
                sb.Draw(Data.FindTexture["EmptyInventorySlot"], drawPosition, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0004f);
                drawPosition.Y += Data.FindTextureSize["EmptyInventorySlot"].Y;
                //posi.Y += Data.FindTextureSize["EmptyInventorySlot"].Y;
            }
        }

        public static bool Click(MouseState nowState, Toon dude)
        {
            // If click not within patterns return
            if (!boundsOnScreen.Contains(new Point(nowState.X, nowState.Y)))
                return false;

            // Else work out where the click was and then which index that is inside slots
            int clickedPattern = (nowState.Y - boundsOnScreen.Y) / 90;

            Console.WriteLine(clickedPattern);

            return true;
        }
    }
}
