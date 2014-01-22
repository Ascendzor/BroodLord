﻿using System;
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
        protected List<Item> itemsInSlot;

        public InventorySlot()
        {
            itemsInSlot = new List<Item>();
        }

        public List<Item> ItemsInSlot
        {
            get { return itemsInSlot; }
        }

        public int Quantity
        {
            get { return itemsInSlot.Count; }
        }

        public void addItemToSlot(Item item)
        {
            itemsInSlot.Add(item);
        }

        /// <summary>
        /// Return item type only if there is an item in slot
        /// </summary>
        public Type ItemType
        {
            get
            {
                if (itemsInSlot.Count > 0)
                    return itemsInSlot[0].GetType();
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
            if (itemsInSlot.Count != 0)
                return itemsInSlot[0].TextureKey;
            else
                return "EmptyInventorySlot";
            
        }

        public void dropSlot(Vector2 position, Guid dudeId)
        {
            //foreach (Item item in itemsInSlot)
            //{
            //    if (item is WoodItem)
            //        Client.SendEvent(new SpawnWoodEvent(item.Id, position));
            //    if (item is RockItem)
            //        Client.SendEvent(new SpawnRockEvent(item.Id, position));
            //}

            foreach (Item item in itemsInSlot)
            {
                Client.SendEvent(new DroppedItemEvent(dudeId, item.Id));
            }

            //
            //Drop loot event
            //

            //itemsInSlot.Clear();            
        }

        public void removeItem(Guid itemId)
        {
            foreach (Item item in itemsInSlot)
            {
                if (item.Id.Equals(itemId))
                    itemsInSlot.Remove(item);
            }
        }




    }
}
