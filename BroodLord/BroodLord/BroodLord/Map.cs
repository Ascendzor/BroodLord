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
    class Map
    {
        private List<MapThing> mapThings;
        private ContentManager Content;

        public Map(ContentManager Content)
        {
            this.Content = Content;
            mapThings = new List<MapThing>();
        }

        public Map(ContentManager Content, bool test)
        {
            this.Content = Content;
            mapThings = new List<MapThing>();
            mapThings.Add(new MapThing(Content, new Vector2(50,50)));
            mapThings.Add(new MapThing(Content, new Vector2(-150, -150)));
            mapThings.Add(new MapThing(Content, new Vector2(20, 300)));
        }

        public List<MapThing> MapThings
        {
            get { return mapThings; }
        }

        public void Draw(SpriteBatch sb)
        {
            //draw ground

            foreach (MapThing mt in mapThings)
            {
                mt.Draw(sb);
            }
        } 

    }
}
