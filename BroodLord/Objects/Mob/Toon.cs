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
        protected int maxHealth;
        protected int maxHunger;
        protected int hunger;
        protected int maxThirst;
        protected int thirst;

        protected int hungerCounter = 0;
        protected int thirstCounter = 0;
        
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
            this.health = 10;
            this.maxHealth = 10;
            this.hunger = 10080;
            this.maxHunger = 10080;
            this.thirst = 10080;
            this.maxThirst = 10080;
            this.isInteractable = true;

            interactionOffCooldown = DateTime.Now;

            Map.InsertGameObject(this);
        }

        

        public int Hunger
        {
            get { return hunger; }
        }

        public int Thirst
        {
            get { return thirst; }
        }

        public int MaxHealth
        {
            get { return maxHealth; }
        }

        public int MaxHunger
        {
            get { return maxHunger; }
        }

        public int MaxThirst
        {
            get { return maxThirst; }
        }

        public Inventory Inventory
        {
            get { return inventory; }
        }

        public int Health
        {
            get { return health; }
        }

        public void replenishHunger(int amount)
        {
            hunger += amount;
            if (hunger > maxHunger)
                hunger = maxHunger;
        }

        public void replenishThirst(int amount)
        {
            thirst += amount;
            if (thirst > maxThirst)
                thirst = maxThirst;
        }

        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);

            if (Data.IsServer)
            {
                if (health <= 0)
                {
                    Client.SendEvent(new DeathEvent(GetId()));
                }
            }
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
            else if (gameObject is PalmTree)
            {
                InteractWithObject((PalmTree)gameObject);
            }
            else if (gameObject is Loot)
            {
                InteractWithObject((Loot)gameObject);

            }
            else if (gameObject is Mob)
            {
                InteractWithObject((Mob)gameObject);
            }
        }
        
        private void InteractWithObject(Tree tree)
        {
            interactionOffCooldown = DateTime.Now.AddMilliseconds(interactionCooldown); //<--- this allows the interaction to define the cooldown, ie chopping may take longer than attacking
            base.Interact(tree);
            Console.WriteLine("Toon chopped");
            tree.GotChopped(this);
        }

        private void InteractWithObject(PalmTree tree)
        {
            interactionOffCooldown = DateTime.Now.AddMilliseconds(interactionCooldown); //<--- this allows the interaction to define the cooldown, ie chopping may take longer than attacking
            base.Interact(tree);
            Console.WriteLine("Toon chopped");
            tree.GotChopped(this);
        }

        private void InteractWithObject(Mob mob)
        {
            interactionOffCooldown = DateTime.Now.AddMilliseconds(interactionCooldown); //<--- this allows the interaction to define the cooldown, ie chopping may take longer than attacking
            AttackMob(mob);
            //base.Interact(tree);
            //Console.WriteLine("Toon chopped");
           // tree.GotChopped(this);
        }

        /// <summary>
        /// If adding to inventory succeeded (returned true) remove loot from map
        /// </summary>
        /// <param name="loot">Loot to add to inventory</param>
        private void InteractWithObject(Loot loot)
        {
            if (inventory.addToInventory(loot.CreateItem(loot), loot.Stackable))
                Map.RemoveGameObject(loot.GetId());
        }

        public void ReceiveEvent(MoveToPositionEvent leEvent)
        {
            goalPosition = leEvent.Position;
        }

        /// <summary>
        /// Remove item from inventory, then create loot of that item at toons position
        /// </summary>
        /// <param name="leEvent">Drop event recieved</param>
        public void ReceiveEvent(DroppedItemEvent leEvent)
        {
            Item item = inventory.removeItem(leEvent.ItemId);
            if (item != null)
                item.CreateLoot(position);
        }

        public void ReceiveEvent(DestroyItemEvent leEvent)
        {
            Item item = inventory.removeItem(leEvent.ItemId);
        }

        public override void ReceiveEvent(DeathEvent leEvent)
        {
            Map.ErradicateGameObject(leEvent.Id);
        }

        /// <summary>
        /// When you publish a SpawnToonEvent your dude will receive it and this is you telling it to fuck off -Troy
        /// </summary>
        /// <param name="leEvent"></param>
        public void ReceiveEvent(SpawnToonEvent leEvent)
        {
            Console.WriteLine("my id: " + id);
            return;
        }

        public void ReceiveEvent(EvilDudeEvent leEvent)
        {
            Console.WriteLine("RECEIVED EVIL DUDE EVENT");
            Console.WriteLine("given guid: " + leEvent.Id);
            Console.WriteLine("my guid: " + Data.Dude);
            if (Data.Dude != null)
            {
                if (id.Equals(leEvent.Id))
                {
                    textureKey = "evil man medium";
                }
            }
        }
        
        public override void Update()
        {
            hunger--;
            thirst--;
            if (hunger < 0)
            {
                hungerCounter++;
                if (hungerCounter > 1200)
                {
                    hungerCounter = 0;
                    TakeDamage(1);
                }
                hunger++;
            }

            if (thirst < 0)
            {
                thirstCounter++;
                if (thirstCounter > 1200)
                {
                    thirstCounter = 0;
                    TakeDamage(1);
                }
                thirst++;
            }

            base.Update();
        }
    }
}