using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Objects
{   
    /// <summary>
    /// InventorySlot has a list of items
    /// If the list is ever greater than size 1, then there are more than one of the same item in the list/slot
    /// List of size 0 is an empty slot
    /// Inventory calls getTextureKey to get either the item texture or empty slot texture
    /// </summary>
    public class InventorySlot
    {
        protected Dictionary<Guid, Item> items;

        public InventorySlot()
        {
            items = new Dictionary<Guid, Item>();
        }
        
        public int Quantity
        {
            get { return items.Count; }
        }

        public void addItemToSlot(Item item)
        {
            items.Add(item.Id, item);
        }

        /// <summary>
        /// Return item type only if there is an item in slot
        /// </summary>
        public Type ItemType
        {
            get
            {
                if (items.Count > 0)
                    return items.Values.ToList<Item>()[0].GetType();
                else
                    return null;
            }
        }

        /// <summary>
        /// Gets item texture or empty slot texture if no items in slot
        /// </summary>
        /// <returns></returns>
        public String getTextureKey()
        {
            if (items.Count != 0)
                return items.Values.ToList<Item>()[0].TextureKey;
            else
                return "EmptyInventorySlot";
            
        }

        public void dropSlot(Vector2 position, Guid dudeId)
        {
            foreach (Item item in items.Values.ToList<Item>())
            {
                Client.SendEvent(new DroppedItemEvent(dudeId, item.Id));
            }         
        }

        public bool removeItem(Guid itemId)
        {
            if (items.ContainsKey(itemId))
            {
                items.Remove(itemId);
                return true;
            }
            else return false;
        }




    }
}
