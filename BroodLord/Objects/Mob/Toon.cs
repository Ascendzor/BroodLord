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
            this.interactRange = 100;
            this.attackDamage = 200;
            this.interactionCooldown = 1000;

            xTileCoord = (int)position.X / Map.GetTileSize();
            yTileCoord = (int)position.Y / Map.GetTileSize();

            lastInteractionTimestamp = DateTime.Now;
            interactionOffCooldown = DateTime.Now;

            Data.AddGameObject(this);
        }

        protected override void Interact(GameObject gameObject)
        {
            if (DateTime.Now.CompareTo(interactionOffCooldown) == -1)
            {
                return;
            }
            lastInteractionTimestamp = DateTime.Now;

            base.Interact(gameObject);

            goalGameObject = null; //<-- this is to stop you from interacting every frame, this may be removed once a cooldown has been introduced
            goalPosition = position;

            if (gameObject is Tree)
            {
                InteractWithTree((Tree)gameObject);
            }
        }

        private void InteractWithTree(Tree tree)
        {
            interactionOffCooldown = DateTime.Now.AddMilliseconds(interactionCooldown); //<--- this allows the interaction to define the cooldown, ie chopping may take longer than attacking
            base.Interact(tree);
            Console.WriteLine("Toon chopped");
            Client.SendEvent(new ChopEvent(id));
            tree.GotChopped(this);
        }

        public double GetAttackDamage()
        {
            return attackDamage;
        }
    }
}