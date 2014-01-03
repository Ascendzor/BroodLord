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
        private int attackDamage;
        private double interactionCooldown;
        private DateTime lastInteractionTimestamp;

        public Toon(Guid id, Vector2 position, string textureKey, Map map, Client client)
        {
            this.id = id;
            this.position = position;
            this.textureKey = textureKey;
            this.movementSpeed = 10;
            this.goalPosition = position;
            this.map = map;
            this.origin = new Vector2(Data.FindTexture[textureKey].Width / 2, Data.FindTexture[textureKey].Height * 0.85f);
            this.interactRange = 100;
            this.client = client;
            this.attackDamage = 100;
            this.interactionCooldown = 1000;

            xTileCoord = (int)position.X / map.GetTileSize();
            yTileCoord = (int)position.Y / map.GetTileSize();

            lastInteractionTimestamp = DateTime.Now;

            Data.AddGameObject(this);
        }

        protected override void Interact(GameObject gameObject)
        {
            if ((DateTime.Now - lastInteractionTimestamp).TotalMilliseconds < interactionCooldown)
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
            base.Interact(tree);
            Console.WriteLine("Toon chopped");
            client.SendEvent(new ChopEvent(id));
            tree.GotChopped(this);
        }

        public int GetAttackDamage()
        {
            return attackDamage;
        }
    }
}