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
using Objects;

namespace BroodLord
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Client client;
        Input input;
        Toon dude;

        public static Dictionary<string, Texture2D> findTexture;
        public static Dictionary<Guid, Toon> allToons;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            findTexture = new Dictionary<string, Texture2D>();
            findTexture.Add("link", Content.Load<Texture2D>("link"));
            allToons = new Dictionary<Guid, Toon>();

            client = new Client();
            dude = new Toon(new Vector2(100, 100), "link");
            input = new Input(dude, client);

            IsMouseVisible = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            input.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            foreach (Guid key in allToons.Keys)
            {
                allToons[key].Draw(spriteBatch, findTexture["link"]);
            }
            dude.Draw(spriteBatch, findTexture["link"]);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
