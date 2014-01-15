using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;

namespace Objects
{
    [Serializable()]
    public class Toon : Mob
    {
        public Toon(Guid id, Vector2 position, string textureKey)
        {
            this.id = id;
            this.position = position;
            this.textureKey = textureKey;
            this.movementSpeed = 10;
            this.goalPosition = position;
            this.origin = new Vector2(Data.FindTexture[textureKey].Width / 2, Data.FindTexture[textureKey].Height * 0.85f);
            this.interactRange = Data.ToonInteractionRange;
            this.attackDamage = 200;
            this.interactionCooldown = 1000;
            this.interactionCooldown = 200;
            this.inventory = new Inventory();
            

            xTileCoord = (int)position.X / Map.GetTileSize();
            yTileCoord = (int)position.Y / Map.GetTileSize();

            lastInteractionTimestamp = DateTime.Now;
            interactionOffCooldown = DateTime.Now;

            Data.AddGameObject(this);

            Map.InsertGameObject(this);
        }

        public Inventory Inventory
        {
            get { return inventory; }
        }

        protected override void Interact(GameObject gameObject)
        {
            if (DateTime.Now.CompareTo(interactionOffCooldown) == -1)
            {
                return;
            }
            lastInteractionTimestamp = DateTime.Now;

            base.Interact(gameObject);

            goalGameObject = null; //<-- this  is to stop you from interacting every frame, this may be removed once a cooldown has been introduced
            goalPosition = position;

            if (gameObject is Tree)
            {
                InteractWithObject((Tree)gameObject);
            }
            else if (gameObject is Loot)
            {
                InteractWithObject((Loot)gameObject);
            }
        }
        
        private void InteractWithObject(Tree tree)
        {
            interactionOffCooldown = DateTime.Now.AddMilliseconds(interactionCooldown); //<--- this allows the interaction to define the cooldown, ie chopping may take longer than attacking
            base.Interact(tree);
            Console.WriteLine("Toon chopped");
            Client.SendEvent(new ChopEvent(id));
            tree.GotChopped(this);
        }

        private void InteractWithObject(Loot loot)
        {
            Client.SendEvent(new LootedLootEvent(id, loot.GetId()));
        }

        public double GetAttackDamage()
        {
            return attackDamage;
        }

        public void ReceiveEvent(LootedLootEvent leEvent)
        {
            LootedLootEvent LLE = (LootedLootEvent)leEvent;

            Loot lootedItem = Data.FindLoot[LLE.item];

            if (inventory.addToInventory(lootedItem, true))
            {
                Map.RemoveGameObject(lootedItem);
            }
        }

        //when you publish a SpawnToonEvent you will receive it and this is you telling it to fuck off -Troy
        public void ReceiveEvent(SpawnToonEvent leEvent)
        {
            return;
        }
    }
}