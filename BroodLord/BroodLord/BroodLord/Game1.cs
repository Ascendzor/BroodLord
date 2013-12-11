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
            Data.findTexture = new Dictionary<string, Texture2D>();
            Data.findTexture.Add("link", Content.Load<Texture2D>("link"));
            Data.findTexture.Add("tree", Content.Load<Texture2D>("tree"));

            for(int x = 1;x<9;x++)
                Data.findTexture.Add("snow"+x, Content.Load<Texture2D>("snow" + x));

            allToons = new Dictionary<Guid, Toon>();
           
            client = new Client();
            map = new Map(this.Content, 84,20);
            dude = new Toon(new Vector2(100, 100), "link",map,28);
            input = new Input(dude, client);
            camera = new Camera();

            Tree bob = new Tree(new Vector2(200, 200), "tree", map, 56);
            Tree bob1 = new Tree(new Vector2(400, 450), "tree", map, 56);
            Tree bob2 = new Tree(new Vector2(500, 200), "tree", map, 56);

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
            dude.CheckGrid();

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

            
            foreach (Guid key in allToons.Keys) //probably shoudlnt be here, Currently here for testing but also a bigger problem. Note: How to draw other players without using grid system? by ZacJ
            {
                allToons[key].Draw(spriteBatch);
            }
            
            map.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
