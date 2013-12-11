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
        Camera camera;
        Map map;

        public static Dictionary<string, Texture2D> findTexture;
        public static Dictionary<Guid, Toon> allToons;

        public static GraphicsDevice graphicsDevice;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            findTexture = new Dictionary<string, Texture2D>();
            findTexture.Add("link", Content.Load<Texture2D>("link"));
            findTexture.Add("tree", Content.Load<Texture2D>("tree"));

            allToons = new Dictionary<Guid, Toon>();
           
            client = new Client();
            dude = new Toon(new Vector2(100, 100), "link");
            input = new Input(dude, client);
            camera = new Camera();
            map = new Map(this.Content, true);

            graphicsDevice = graphics.GraphicsDevice;
            IsMouseVisible = true;

            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();

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

            foreach (Guid key in allToons.Keys)
            {
                allToons[key].Update();
            }

            dude.Update();

            camera.update(dude.Position);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.BackToFront,
                        BlendState.AlphaBlend,
                        null,
                        null,
                        null,
                        null,
                        camera.getTransformation(graphics.GraphicsDevice));

            foreach (Guid key in allToons.Keys)
            {
                allToons[key].Draw(spriteBatch, findTexture["link"]);
            }
            dude.Draw(spriteBatch, findTexture["link"]);

            map.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
