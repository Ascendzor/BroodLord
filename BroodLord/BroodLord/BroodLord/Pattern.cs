using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BroodLord
{
    public class Pattern
    {
        public List<String> ingredients;
        public Dictionary<String, String> patternResult;
        public string item;

        public Pattern(List<String> ingredients, string item)
        {
            this.ingredients = ingredients;
            this.item = item;
        }

        public void Draw(SpriteBatch sb, Vector2 position)
        {
            sb.Draw(Data.FindTexture["EmptyInventorySlot"], position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0004f);
        }

        public bool HaveIngredients(Inventory inv)
        {
            bool haveAllIngredients = true;

            foreach (String ingred in ingredients)
            {
                if (!inv.ContainsItemType(ingred))
                    haveAllIngredients = false;
                
            }

            return haveAllIngredients;
        }



        
    }
}
