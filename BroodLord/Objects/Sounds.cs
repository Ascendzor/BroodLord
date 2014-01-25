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
        public static void PlaySound(SoundEffect Sound)
        {
            SoundEffectInstance soundEngineInstance;

            soundEngineInstance = Sound.CreateInstance();
            soundEngineInstance.Play();
        }
    }
}
