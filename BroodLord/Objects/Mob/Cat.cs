using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Objects
{
    [Serializable()]
    public class Cat : Mob
    {
        public Cat(Guid id, Vector2 position)
        {
            this.id = id;
            this.position = position;
            this.textureKey = "cat";
            this.origin = new Vector2(Data.GetTextureSize(textureKey).X / 2, Data.GetTextureSize(textureKey).Y * 0.85f);
            this.movementSpeed = 1;
            this.goalPosition = position;
            this.interactRange = 100;
            this.interactionCooldown = 5000;
            this.attackDamage = 60;
            this.health = 100;

            Map.InsertGameObject(this);
        }

        protected override void Interact(GameObject gameObject)
        {
            if (DateTime.Now.CompareTo(interactionOffCooldown) < 0)
            {
                return;
            }

            goalGameObject = null; //<-- this  is to stop you from interacting every frame, this may be removed once a cooldown has been introduced
            goalPosition = position;

            if (gameObject is Toon)
            {
                InteractWithObject((Toon)gameObject);
            }
        }

        private void InteractWithObject(Toon toon)
        {
            Console.WriteLine("cat smacking the bitch: " + toon.GetId());
            interactionOffCooldown = DateTime.Now.AddMilliseconds(interactionCooldown); //<--- this allows the interaction to define the cooldown, ie chopping may take longer than attacking
            Client.SendEvent(new TookDamageEvent(toon.GetId(), attackDamage));
        }
    }
}
