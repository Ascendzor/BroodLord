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
        protected List<InventorySlot> slots;
        protected int inventorySize;
        protected int inventorySlotSize;


        public int InventorySize
        {
            get { return inventorySize; }
        }

        public int InventorySlotSize
        {
            get { return inventorySlotSize; }
        }
        
        public Inventory()
        {
            slots = new List<InventorySlot>();
            inventorySize = 10;
            for (int i = 0; i<inventorySize; i++)
            {
                slots.Add(new InventorySlot());
            }
            inventorySlotSize = 91;
        }

        /// <summary>
        /// Attempt to add an item to inventory
        /// </summary>
        /// <param name="itemToAdd">Loot that is being picked up</param>
        /// <returns>True if picked up else false (full inventory)</returns>
        public bool addToInventory(Item itemToAdd)
        {
            bool itemAddedToInventory = false;
            Console.WriteLine(slots.Count);
            foreach (InventorySlot slot in slots)
            {
                if (slot.Quantity == 0)
                {
                    slot.addItemToSlot(itemToAdd);
                    itemAddedToInventory = true;
                    break;
                }
            }

            // Testing only
            if (!itemAddedToInventory)
                Console.WriteLine(fullInventoryMessage());

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

            // Try stack item in a slot
            foreach (InventorySlot slot in slots)
            {
                if (slot.Quantity != 0)
                {
                    if (itemToAdd.GetType() == slot.ItemType)
                    {
                        slot.addItemToSlot(itemToAdd);
                        itemAddedToInventory = true;
                    }
                }
            }

            // Try add item into empty slot
            if (!itemAddedToInventory)
            {
                foreach (InventorySlot slot in slots)
                {
                    if (slot.Quantity == 0)
                    {
                        slot.addItemToSlot(itemToAdd);
                        itemAddedToInventory = true;
                        break;
                    }
                }

                // Testing only
                if (!itemAddedToInventory)
                    Console.WriteLine(fullInventoryMessage());
            }

            return itemAddedToInventory;
        }

        private int getSlotsInUse()
        {
            int count = 0;
            foreach (InventorySlot slot in slots)
            {
                if (slot.Quantity == 0) count++;
            }
            return count;
        }

        private String fullInventoryMessage()
        {
            return "Inventory is full!";
        }

        public void Draw(SpriteBatch sb, Vector2 drawPosition, SpriteFont spriteFont)
        {
            // Find number of rows
            int rows = 3;
            drawPosition.Y -= (Data.FindTexture["InventorySlot"].Height) * rows;

            int count = 0;
            foreach (InventorySlot invSlot in slots)
            {
                sb.Draw(Data.FindTexture["InventorySlot"], drawPosition, Color.White);
                sb.Draw(Data.FindTexture[invSlot.getTextureKey()], drawPosition, Color.White);
                sb.DrawString(spriteFont, invSlot.Quantity.ToString(), drawPosition, Color.White);
                drawPosition.X += inventorySlotSize;
                count++;
                if (count == 4)
                {
                    drawPosition.X -= inventorySlotSize * 4;
                    drawPosition.Y += inventorySlotSize;
                    count = 0;
                }
            }

        }
    }
}
