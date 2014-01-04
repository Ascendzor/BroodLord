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

        public static Dictionary<Guid, Toon> allToons;

        public static GraphicsDevice graphicsDevice;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            Data.Initialize(Content);

            Map.Initialize(Data.TileSize, Data.MapSize, 5); //the renderWidth should be dynamic to the resolution
            client = new Client();
            dude = new Toon(Guid.NewGuid(), new Vector2(100, 100), "link", client);
            input = new Input(dude, client);
            camera = new Camera();

            new Tree(new Vector2(200, 200), "tree", client);
            new Tree(new Vector2(400, 450), "tree", client);
            new Tree(new Vector2(500, 200), "tree", client);

            new Rock(new Vector2(700, 800));

            graphicsDevice = graphics.GraphicsDevice;
            IsMouseVisible = true;

            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();

            spriteBatch = new SpriteBatch(GraphicsDevice);

            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            input.Update();

            foreach (Mob mob in Data.FindMob.Values)
            {
                mob.Update();
            }

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

            Map.Draw(spriteBatch, dude.GetGridCoordX(), dude.GetGridCoordY());

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
