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
        private Client client;

        public Environment()
        {
            Data.Initialize();
            Map.Initialize(Data.TileSize, Data.MapSize, 5);

            new Tree(new Vector2(200, 200), "tree", client);
            new Tree(new Vector2(400, 450), "tree", client);
            new Tree(new Vector2(500, 200), "tree", client);

            new Rock(new Vector2(700, 800));
        }

        public void Play()
        {
        }
    }
}
