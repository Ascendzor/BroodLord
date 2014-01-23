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

            //inventorySlotSize = Data.FindTexture["InventorySlot"].Height; //y u no work
            inventorySlotSize = 90;
            selectedSlot = -1;

            // 720 needs to be screen height :((((
            boundsOnScreen = new Rectangle(0, 720 - inventorySlotSize * inventoryRows, inventorySlotSize * inventoryCols, inventorySlotSize * inventoryRows);
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
                slots[clickedSlot].dropSlot(dude.Position);

            return true;
        }

        /// <summary>
        /// Draws inventory slots
        /// Image = empty slot or item in bag image
        /// </summary>
        /// <param name="drawPosition">Bottom left of the drawing position</param>
        public void Draw(SpriteBatch sb, Vector2 drawPosition, SpriteFont spriteFont)
        {
            drawPosition.Y -= (inventorySlotSize + 1) * inventoryRows;

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
                drawPosition.X += inventorySlotSize + 1;

                count++;
                // May be bad to modulo here
                if (count % inventoryCols == 0)
                {
                    drawPosition.X -= (inventorySlotSize + 1) * inventoryCols;
                    drawPosition.Y += inventorySlotSize + 1;
                }
            }
        }

    
    }
}
