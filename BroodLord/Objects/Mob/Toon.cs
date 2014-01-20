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
            Console.WriteLine("new toon getting mayd");
            this.id = id;
            this.position = position;
            this.textureKey = textureKey;
            this.movementSpeed = 10;
            this.goalPosition = position;
            this.origin = new Vector2(Data.GetTextureSize(textureKey).X / 2, Data.GetTextureSize(textureKey).Y * 0.85f);
            this.interactRange = Data.ToonInteractionRange;
            this.attackDamage = 200;
            this.interactionCooldown = 1000;
            this.interactionCooldown = 200;
            this.inventory = new Inventory();
            this.health = 100;

            interactionOffCooldown = DateTime.Now;

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

            base.Interact(gameObject);

            goalGameObject = null; //<-- this  is to stop you from interacting every frame, this may be removed once a cooldown has been introduced
            goalPosition = position;

            //bad ifs, change it to be more dynamic
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
            //bad implementation but can be improved on later
            if (loot is RockLoot)
            {
                inventory.addToInventory(new RockItem(loot.GetId()));
            }
            else if (loot is WoodLoot)
            {
                inventory.addToInventory(new WoodItem(loot.GetId()));
            }

            Console.WriteLine("looting: " + loot.GetId());
            Client.SendEvent(new LootedLootEvent(loot.GetId()));
        }

        public double GetAttackDamage()
        {
            return attackDamage;
        }

        public void ReceiveEvent(LootedLootEvent leEvent)
        {
            //perform loot animation
            Map.RemoveGameObject(leEvent.Id);
        }

        //when you publish a SpawnToonEvent your dude will receive it and this is you telling it to fuck off -Troy
        public void ReceiveEvent(SpawnToonEvent leEvent)
        {
            return;
        }

        public void ReceiveEvent(TookDamageEvent leEvent)
        {
            Console.WriteLine("I got smacked yo");
            health -= (int)leEvent.DamageTaken;
            if (health <= 0)
            {
                Console.WriteLine("I rip gg");
            }
        }
    }
}