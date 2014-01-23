using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Objects
{
    [Serializable()]
    public class Loot : GameObject
    {
        protected int quantity;

        /// <summary>
        /// All loot is interactable so define as true here
        /// </summary>
        public Loot()
        {
            this.isInteractable = true;
        }

        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(Data.FindTexture[textureKey],
                new Rectangle((int)position.X,
                    (int)position.Y,
                    Data.FindTexture[textureKey].Width, Data.FindTexture[textureKey].Height),
                null,
                Color.White,
                0,   
                origin,
                SpriteEffects.None,
                1 - (position.Y / (Data.MapSize * Data.TileSize)));
            sb.Draw(Data.FindTexture["treeOutline"], hitbox, Color.Red);
        }

        public virtual Item CreateItem(Loot loot) { return new Item(); }

    }
}
