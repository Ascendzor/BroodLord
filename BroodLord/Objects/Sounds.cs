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
using Objects;
using System.Threading;

namespace Objects
{
    public static class Sounds
    {
        static int check = 0;
        public static void PlaySound(SoundEffect Sound)
        {
            Sound.Play();
        }

        public static void PlayBGSound(SoundEffect Sound)
        {
            if (check == 0) Sound.Play();
            check++;
            if (check == 5500) check = 0;
        }
    }
}
