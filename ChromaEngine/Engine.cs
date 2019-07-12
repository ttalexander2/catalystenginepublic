using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System.Reflection;

namespace Chroma
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Engine : Microsoft.Xna.Framework.Game
    {
        // Instances
        public static Engine instance { get; private set;  }
        GraphicsDeviceManager graphics { get; set; }
        RenderTarget2D renderTarget { get; set; }
        SpriteBatch spriteBatch;

        // Screen
        public static int width { get; private set; }
        public static int height { get; private set; }
        public static int viewWidth { get; private set; }
        public static int viewHeight { get; private set; }
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

        public Engine(int width, int height, int viewWidth, int viewHeight, string windowTitle, bool fullscreen)
        {
            Engine.instance = this;

            Engine.title = windowTitle;
            Engine.width = width;
            Engine.height = height;
            Engine.viewWidth = viewWidth;
            Engine.viewHeight = viewHeight;
            Engine.fullscreen = fullscreen;

            graphics = new GraphicsDeviceManager(this);
            graphics.SynchronizeWithVerticalRetrace = true;
            graphics.PreferMultiSampling = false;
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            graphics.PreferredBackBufferFormat = SurfaceFormat.Color;
            graphics.PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8;
            graphics.ApplyChanges();

        #if PS4 || XBOXONE
            Graphics.PreferredBackBufferWidth = 1920;
            Graphics.PreferredBackBufferHeight = 1080;
        #elif NSWITCH
            Graphics.PreferredBackBufferWidth = 1280;
            Graphics.PreferredBackBufferHeight = 720;
        #else
            Window.AllowUserResizing = true;

            if (fullscreen)
            {
                graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                graphics.IsFullScreen = true;
            }
            else
            {
                graphics.PreferredBackBufferWidth = Engine.viewWidth;
                graphics.PreferredBackBufferHeight = Engine.viewHeight;
                graphics.IsFullScreen = false;
            }
        #endif

            Content.RootDirectory = @"Content";
            renderTarget = new RenderTarget2D(graphics.GraphicsDevice, width, height);
            graphics.GraphicsDevice.SetRenderTarget(renderTarget);

            
            
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
            spriteBatch = new SpriteBatch(GraphicsDevice);
            scene = new Scene(2,2);
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
            scene.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            base.Draw(gameTime);

            // End
            GraphicsDevice.SetRenderTarget(null);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            //spriteBatch.Draw(renderTarget);
            spriteBatch.End();
        }
    }
}
