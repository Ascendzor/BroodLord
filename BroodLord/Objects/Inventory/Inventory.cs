using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Objects
{   
    /// <summary>
    /// Dude has inventory, inventory has a list of InventorySlots
    /// Inventory has size (number of slots)
    /// Inventory draws each slot on to the screen (draw call from HUD draw)
    /// </summary>
    [Serializable()]
    public class Inventory
    {
        protected List<InventorySlot> slots;
        
        protected int inventorySlotSize;
        protected int selectedSlot;
        protected Rectangle boundsOnScreen;

        // Used for size of inventory. If inventory can be of variable size then these values must change
        private const int inventoryRows = 3;
        private const int inventoryCols = 3;
        private const int inventoryCapacity = 9;

        public int InventorySlotSize
        {
            get { return inventorySlotSize; }
        }

        public Rectangle BoundsOnScreen
        {
            get { return boundsOnScreen; }
        }
        
        public Inventory()
        {
            slots = new List<InventorySlot>();
            for (int i = 0; i < inventoryCapacity; i++)
            {
                slots.Add(new InventorySlot());
            }

            inventorySlotSize = (int)Data.GetTextureSize("InventorySlot").Y;
            selectedSlot = -1;

            // 1080 needs to be screen height :((((
            boundsOnScreen = new Rectangle(0, 1080 - inventorySlotSize * inventoryRows, inventorySlotSize * inventoryCols, inventorySlotSize * inventoryRows);
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
            if (stackItem)
            {
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

        private String fullInventoryMessage()
        {
            return "Inventory is full!";
        }

        /// <summary>
        /// True if given point is within the inventory bounds
        /// </summary>
        public bool inventoryClick(MouseState nowState, Toon dude)
        {
            // If click not within inventory return
            if (!boundsOnScreen.Contains(new Point(nowState.X, nowState.Y)))
                return false;

            // Else work out where the click was and then which index that is inside slots
            int x = (nowState.X - boundsOnScreen.X) / inventorySlotSize;
            int y = (nowState.Y - boundsOnScreen.Y) / inventorySlotSize;
            int clickedSlot = x + (y * inventoryCols);

            // Left and Right Click
            if (nowState.LeftButton == ButtonState.Pressed)
                selectedSlot = clickedSlot;
            else if (nowState.RightButton == ButtonState.Pressed)
                slots[clickedSlot].dropSlot(dude.Position, dude.GetId());
            else if (nowState.MiddleButton == ButtonState.Pressed)
                slots[clickedSlot].useSlot(dude);

            return true;
        }

        /// <summary>
        /// Gets and removes an item out of an inventory slot.
        /// </summary>
        /// <param name="itemId">Guid of item to remove</param>
        /// <returns>Item that was removed, or null if not found</returns>
        public Item removeItem(Guid itemId)
        {
            foreach (InventorySlot slot in slots)
            {
                Item itemRemoved;
                if ((itemRemoved = slot.removeItem(itemId)) != null)
                    return itemRemoved;
            }
            // Not sure about this return, "should" be no case where item is not found
            return null;
        }
        
        /// <summary>
        /// Gets item in an inventory slot
        /// </summary>
        /// <param name="id">Guid of item</param>
        /// <returns>Item found in inventory slot</returns>
        public Item GetItem(Guid id)
        {
            foreach (InventorySlot slot in slots)
            {
                if (slot.GetItem(id) != null)
                {
                    return slot.GetItem(id);
                }
            }
            return null;
        }

        /// <summary>
        /// Draws inventory slots
        /// Image = empty slot or item in bag image
        /// </summary>
        /// <param name="drawPosition">Bottom left of the drawing position</param>
        public void Draw(SpriteBatch sb, Vector2 drawPosition, SpriteFont spriteFont)
        {
            drawPosition.Y -= (inventorySlotSize) * inventoryRows;

            // Depth layer, 0 = default, >0 = away from you
            int count = 0;
            foreach (InventorySlot invSlot in slots)
            {
                sb.Draw(Data.FindTexture[invSlot.getTextureKey()], drawPosition, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.2f);
                sb.DrawString(spriteFont, invSlot.Quantity.ToString(), drawPosition, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);

                // Draw around the current slot
                if (selectedSlot == count)
                {
                    sb.Draw(Data.FindTexture["InventorySlot"], drawPosition, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.1f);
                }
                // 1 = offset (gap between images)
                drawPosition.X += inventorySlotSize;

                count++;
                // May be bad to modulo here
                if (count % inventoryCols == 0)
                {
                    drawPosition.X -= (inventorySlotSize) * inventoryCols;
                    drawPosition.Y += inventorySlotSize;
                }
            }
        }    
    }
}
