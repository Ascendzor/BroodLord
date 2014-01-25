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
using System.Threading;

namespace BroodLord
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Input input;
        Toon dude;
        Camera camera;
        HUD HUD;

        public static Dictionary<Guid, Toon> allToons;

        public static GraphicsDevice graphicsDevice;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            Thread.Sleep(1000);
            Console.WriteLine("Guid size: " + Guid.NewGuid().ToByteArray().Length);
            Data.Initialize(Content);
            Map.Initialize(12); //the renderWidth should be dynamic to the resolution
            Client.Initialize();
            dude = new Toon(Guid.NewGuid(), new Vector2(100, 100), "link");
            Data.Dude = dude;
            input = new Input(dude);
            camera = new Camera();

            graphicsDevice = graphics.GraphicsDevice;
            IsMouseVisible = true;

            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;
            graphics.ApplyChanges();
            HUD = new HUD(this.Content, dude, graphics.PreferredBackBufferHeight, graphics.PreferredBackBufferWidth);

            spriteBatch = new SpriteBatch(GraphicsDevice);

            Console.WriteLine("published SpawnToonEvent");
            Client.SendEvent(new SpawnToonEvent(dude.GetId()));

            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            input.Update();

            Map.Update();
            camera.update(dude.Position);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.BackToFront,
                        BlendState.AlphaBlend,
                        null,
                        null,
                        null,
                        null,
                        camera.getTransformation(graphics.GraphicsDevice));

            Map.Draw(spriteBatch, dude.Position);
            
            HUD.Draw(spriteBatch, camera.Position);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
