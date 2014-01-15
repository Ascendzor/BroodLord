using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Threading;

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
            Console.WriteLine("I am le playing");

            while (true)
            {
                Thread.Sleep(1000);
            }
        }
    }
}
