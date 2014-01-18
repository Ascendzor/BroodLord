using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Objects
{
    [Serializable()]
    public class Inventory
    {
        protected List<Item> items;
        protected int inventorySize;

        public int InventorySize
        {
            get { return inventorySize; }
        }

        public List<Item> Items
        {
            get { return items; }
        }

        public Inventory()
        {
            items = new List<Item>();
            inventorySize = 2;
        }

        /// <summary>
        /// Attempt to add an item to inventory
        /// </summary>
        /// <param name="itemToAdd">Loot that is being picked up</param>
        /// <returns>True if picked up else false (full inventory)</returns>
        public bool addToInventory(Item itemToAdd)
        {
            bool itemAddedToInventory = false;

            // Try add item into empty slot
            if (items.Count < inventorySize)
            {
                items.Add(itemToAdd);
                itemAddedToInventory = true;
            }
            else Console.WriteLine(fullInventoryMessage());

            return itemAddedToInventory;
        }

        /// <summary>
        /// Attempt to add an item to inventory and stack it with existing items of same type
        /// </summary>
        /// <param name="itemToAdd">Loot that is being picked up</param>
        /// <returns>True if picked up else false (full inventory)</returns>
        public bool addToInventory(Item itemToAdd, bool stackItem)
        {
            bool itemAddedToInventory = false;

            // Try stack item
            foreach (Item item in items)
            {
                if (itemToAdd.GetType() == item.GetType())
                {
                    item.Quantity += itemToAdd.Quantity;
                    itemAddedToInventory = true;
                }
            }

            // Try add item into empty slot
            if (!itemAddedToInventory)
            {
                if (items.Count < inventorySize)
                { 
                    items.Add(itemToAdd);
                    itemAddedToInventory = true;
                }
                else Console.WriteLine(fullInventoryMessage());
            }

            return itemAddedToInventory;
        }

        private String fullInventoryMessage()
        {
            return "Inventory is full!";
        }
    }
}
