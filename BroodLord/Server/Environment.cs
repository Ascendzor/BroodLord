using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Server
{
    class Environment
    {

        public Environment()
        {
            MapLoader.LoadMap(@"Assets/map.bmp");
        }

        public void Play()
        {
        }
    }
}
