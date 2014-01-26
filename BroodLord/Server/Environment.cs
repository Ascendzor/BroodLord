﻿using System;
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
        bool delayThread;
        public Environment()
        {
            delayThread = false;
            MapLoader.LoadMap(@"Assets/TerrainMap.bmp", @"Assets/MobMap.bmp", @"Assets/DoodadMap.bmp", @"Assets/LootMap.bmp");
        }

        public void Play()
        {
            Console.WriteLine("I am le playing");

            while (true)
            {
                Map.Update();
                if (delayThread)
                    Thread.Sleep(17); //something like 62.5 fps
                else
                    Thread.Sleep(16);
                delayThread = !delayThread;
            }
        }

        private void Behave(Cat cat)
        {
            if (cat.GetGoalGameObject() == null)
            {
                foreach (Toon toon in Map.GetToons())
                {
                    if ((cat.Position - toon.Position).Length() < 300)
                    {
                        Console.WriteLine("Event go! cat!");
                        Client.SendEvent(new MoveToGameObjectEvent(cat.GetId(), toon.GetId()));
                    }
                }
            }
        }

        
    }
}
