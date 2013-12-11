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

        //tree
        Texture2D leTree;

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
            allToons = new Dictionary<Guid, Toon>();
            //treee
            leTree = Content.Load<Texture2D>("tree");

            Console.WriteLine("ASD");
            client = new Client();
            Console.WriteLine("ASD");
            dude = new Toon(new Vector2(100, 100), "link");
            input = new Input(dude, client);
            camera = new Camera();

            graphicsDevice = graphics.GraphicsDevice;
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
            spriteBatch.Draw(leTree, new Vector2(0, 0), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
