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

        public static Dictionary<Guid, Toon> allToons;

        public static GraphicsDevice graphicsDevice;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            Data.Initialize();

            Data.FindTexture = new Dictionary<string, Texture2D>();
            Data.FindTexture.Add("link", Content.Load<Texture2D>("link"));
            Data.FindTexture.Add("tree", Content.Load<Texture2D>("tree"));
            Data.FindTexture.Add("rock", Content.Load<Texture2D>("rock"));
            Data.FindTexture.Add("treeOutline", Content.Load<Texture2D>("treeOutline"));

            for (int x = 1; x < 9; x++)
            {
                Data.FindTexture.Add("snow" + x, Content.Load<Texture2D>("snow" + x));
            }

            map = new Map(Data.TileSize, Data.MapSize, 5); //the renderWidth should be dynamic to the resolution
            client = new Client(map);
            dude = new Toon(Guid.NewGuid(), new Vector2(100, 100), "link", map, client);
            input = new Input(dude, client, map);
            camera = new Camera();

            Tree bob = new Tree(new Vector2(200, 200), "tree", map, client);
            Tree bob1 = new Tree(new Vector2(400, 450), "tree", map, client);
            Tree bob2 = new Tree(new Vector2(500, 200), "tree", map, client);

            Rock rock = new Rock(new Vector2(700, 800), map);

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

            map.Draw(spriteBatch, dude.GetGridCoordX(), dude.GetGridCoordY());

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
