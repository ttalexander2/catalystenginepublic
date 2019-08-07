using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System.Reflection;
using System;

namespace Chroma
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Engine : Microsoft.Xna.Framework.Game
    {
        // Instances
        public static Engine instance { get; private set;  }


        // Screen
        public static int width { get; private set; }
        public static int height { get; private set; }
        public static int pixelWidth { get; private set; }
        public static int pixelHeight { get; private set; }
        public static bool fullscreen { get; private set; }
        public static string title { get; private set; }

        // View
        public Viewport viewport { get; private set; }

        // Time
        public static float deltaTime { get; private set; }
        public static float rawDeltaTime { get; private set; }
        public static float TimeRate = 1f;
        public static int FPS;

        // Scene
        private Scene scene;
        private Scene nextScene;
        
        // Directories

        #if !CONSOLE
        private static string AssemblyDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        #endif

        public static string contentDirectory
        {
        #if PS4
            get { return Path.Combine("/app0/", Instance.Content.RootDirectory); }
        #elif NSWITCH
            get { return Path.Combine("rom:/", Instance.Content.RootDirectory); }
        #elif XBOXONE
            get { return Instance.Content.RootDirectory; }
        #else
            get { return Path.Combine(AssemblyDirectory, instance.Content.RootDirectory); }
        #endif
        }

        public Engine(int width, int height, int pixelWidth, int pixelHeight, string windowTitle, bool fullscreen)
        {
            Engine.instance = this;

            Engine.title = windowTitle;
            Engine.width = width;
            Engine.height = height;
            Engine.pixelWidth = pixelWidth;
            Engine.pixelHeight = pixelHeight;
            Engine.fullscreen = fullscreen;

            Global.Graphics = new GraphicsDeviceManager(this);
            Global.Graphics.SynchronizeWithVerticalRetrace = true;
            Global.Graphics.PreferMultiSampling = false;
            Global.Graphics.GraphicsProfile = GraphicsProfile.HiDef;
            Global.Graphics.PreferredBackBufferFormat = SurfaceFormat.Color;
            Global.Graphics.PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8;
            Global.Graphics.ApplyChanges();

        #if PS4 || XBOXONE
            Global.Graphics.PreferredBackBufferWidth = 1920;
            Global.Graphics.PreferredBackBufferHeight = 1080;
        #elif NSWITCH
            Global.Graphics.PreferredBackBufferWidth = 1280;
            Global.Graphics.PreferredBackBufferHeight = 720;
        #else
            Window.AllowUserResizing = false;

            if (fullscreen)
            {
                Global.Graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                Global.Graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                Global.Graphics.IsFullScreen = true;
            }
            else
            {
                Global.Graphics.PreferredBackBufferWidth = Engine.width;
                Global.Graphics.PreferredBackBufferHeight = Engine.height;
                Global.Graphics.IsFullScreen = false;
            }
        #endif

            Content.RootDirectory = @"Content";
            //renderTarget = new RenderTarget2D(Global.Graphics.GraphicsDevice, width, height);
            //Global.Graphics.GraphicsDevice.SetRenderTarget(renderTarget);
            Global.Graphics.ApplyChanges();


        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            Global.SpriteBatch = new SpriteBatch(Global.Graphics.GraphicsDevice);
            scene = new Scene(500,500);
            scene.GetLayerList().Add(new BackgroundLayer("Background"));
            TextureAtlas atlas = new TextureAtlas();
            Texture2D test = Content.Load<Texture2D>(contentDirectory + "/test");
            atlas.Textures.Add(test);
            Entity testEntity = new Entity();
            Sprite sprite = new Sprite("test", 0, 0, atlas, Sprite.Origin.TopLeft);
            scene.GetLayerList()[0].AddSpriteComponent(testEntity.UID, sprite);
            
            scene.GetLayerList()[0].AddEntity(testEntity);
            World.Scenes.Add(scene);
            World.currentScene = World.Scenes[0];
            // TODO: use this.Content to load your game content here
        }
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                scene.GetLayerList()[0].GetSpriteComponent(0).pos = new Vector2(scene.GetLayerList()[0].GetSpriteComponent(0).pos.X, scene.GetLayerList()[0].GetSpriteComponent(0).pos.Y-5);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                scene.GetLayerList()[0].GetSpriteComponent(0).pos = new Vector2(scene.GetLayerList()[0].GetSpriteComponent(0).pos.X - 5, scene.GetLayerList()[0].GetSpriteComponent(0).pos.Y);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                scene.GetLayerList()[0].GetSpriteComponent(0).pos = new Vector2(scene.GetLayerList()[0].GetSpriteComponent(0).pos.X, scene.GetLayerList()[0].GetSpriteComponent(0).pos.Y + 5);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                scene.GetLayerList()[0].GetSpriteComponent(0).pos = new Vector2(scene.GetLayerList()[0].GetSpriteComponent(0).pos.X + 5, scene.GetLayerList()[0].GetSpriteComponent(0).pos.Y);
            }


            World.BeforeUpdate(gameTime);
            World.Update(gameTime);
            World.AfterUpdate(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            Global.Graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            Global.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
            //Global.spriteBatch.Draw(Content.Load<Texture2D>(contentDirectory + "/test"), new Vector2(0, 0), Color.White);
            World.BeforeRender(gameTime);
            World.Render(gameTime);
            World.AfterRender(gameTime);
            

            // End
            Global.SpriteBatch.End();
            base.Draw(gameTime);

        }
    }
}
