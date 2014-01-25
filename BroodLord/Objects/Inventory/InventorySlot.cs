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
    [Serializable()]
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
            if(!items.ContainsKey(item.Id))
            {
                items.Add(item.Id, item);
            }
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
        /// <returns>Name of texture in Data as String</returns>
        public String getTextureKey()
        {
            if (items.Count != 0)
                return items.Values.ToList<Item>()[0].TextureKey;
            else
                return "EmptyInventorySlot";
            
        }

        public void useSlot(Toon dude)
        {
            if (items.Count != 0)
            {
                if (items.First().Value.Use(dude))
                    Client.SendEvent(new DestroyItemEvent(dude.GetId(), items.First().Value.Id));
            }
        }

        /// <summary>
        /// Drops all items out of this inventory slot by sending DroppedItemEvent to server
        /// </summary>
        /// <param name="position">Position to drop item at</param>
        /// <param name="dudeId">Id of person to drop item from</param>
        public void dropSlot(Vector2 position, Guid dudeId)
        {
            if (items.Count > 0)
                Client.SendEvent(new DroppedItemEvent(dudeId, items.First().Value.Id));
            //foreach (Item item in items.Values.ToList<Item>())
            //{
            //    Client.SendEvent(new DroppedItemEvent(dudeId, item.Id));
            //}         
        }

        /// <summary>
        /// Removes item from slot (dictionary) then returns it
        /// </summary>
        /// <param name="itemId">Guid of item to remove</param>
        /// <returns>Item that was removed</returns>
        public Item removeItem(Guid itemId)
        {
            if (items.ContainsKey(itemId))
            {
                Item item = items[itemId];
                items.Remove(itemId);
                return item;
            }
            else return null;
        }

        /// <summary>
        /// Returns item with Guid id
        /// </summary>
        /// <param name="id">Guid of item to get</param>
        /// <returns>Item with id</returns>
        public Item GetItem(Guid id)
        {
            if (items.ContainsKey(id))
            {
                return items[id];
            }
            return null;
        }
    }
}
