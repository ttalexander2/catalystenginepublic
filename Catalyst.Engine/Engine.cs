using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Catalyst.Engine.Rendering;
using Catalyst.Engine.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Catalyst.Engine.Audio;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Color = Microsoft.Xna.Framework.Color;

namespace Catalyst.Engine
{

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Engine : Microsoft.Xna.Framework.Game
    {

        // Instances
        public static Engine Instance { get; private set; }
        public Scene CurrentScene { get; set; }


        // Screen
        public static int Width { get; private set; }
        public static int Height { get; private set; }
        public static bool Fullscreen { get; private set; }
        public static string Title { get; private set; }
        public Camera Camera
        {
            get { return CurrentScene.Camera; }
            private set { }
        }

        // Time
        internal static GameTime GameTime;
        public static float TimeScale { get; set; }

        // DebugDrawQueue
        public static Queue<Action> DebugDrawQueue = new Queue<Action>();

        // Directories

        #if !CONSOLE
        public static readonly string AssemblyDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        #endif

        public static string ContentDirectory
        {
        #if PS4
            get { return Path.Combine("/app0/", Instance.Content.RootDirectory); }
        #elif NSWITCH
            get { return Path.Combine("rom:/", Instance.Content.RootDirectory); }
        #elif XBOXONE
            get { return Instance.Content.RootDirectory; }
        #else
            get { return Path.Combine(AssemblyDirectory, Instance.Content.RootDirectory); }
        #endif
        }

        //Rendering
        private RenderTarget2D NativeRenderTarget { get; set; }
        internal Rectangle Screen { get;  private set; }
        internal Catalyst.Engine.Utilities.Vector2 ScreenOffset { get; private set; }

        //Audio
        public AudioManager Audio;
        public Engine(Scene scene, int width, int height, string windowTitle, bool fullscreen) : this(width, height, windowTitle, fullscreen)
        {
            CurrentScene = scene;
        }

        public Engine()
        {

        }
        public Engine(int width, int height, string windowTitle, bool fullscreen)
        {
            CurrentScene = new Scene(1920, 1080);

            Engine.Instance = this;
            Engine.Title = windowTitle;
            Engine.Width = width;
            Engine.Height = height;
            Engine.Fullscreen = fullscreen;

            Graphics.DeviceManager = new GraphicsDeviceManager(this);
            Graphics.DeviceManager.SynchronizeWithVerticalRetrace = true;
            Graphics.DeviceManager.PreferMultiSampling = false;
            Graphics.DeviceManager.GraphicsProfile = GraphicsProfile.HiDef;
            Graphics.DeviceManager.PreferredBackBufferFormat = SurfaceFormat.Color;
            Graphics.DeviceManager.PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8;
            Graphics.DeviceManager.ApplyChanges();

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
                Graphics.DeviceManager.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                Graphics.DeviceManager.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                Graphics.DeviceManager.IsFullScreen = true;
            }
            else
            {
                Graphics.DeviceManager.PreferredBackBufferWidth = Graphics. PreferredWindowWidth;
                Graphics.DeviceManager.PreferredBackBufferHeight = Graphics.PreferredWindowHeight;
                Graphics.DeviceManager.IsFullScreen = false;
            }
#endif

            Content.RootDirectory = @"Content";
            NativeRenderTarget = new RenderTarget2D(Graphics.DeviceManager.GraphicsDevice, width, height);
            Graphics.DeviceManager.GraphicsDevice.SetRenderTarget(NativeRenderTarget);
            Graphics.DeviceManager.ApplyChanges();

            Graphics.Content = Content;

            float ratio = (float)Graphics.Width / (float)Graphics.Height;
            float actual = (float)Graphics.DeviceManager.PreferredBackBufferWidth / (float)Graphics.DeviceManager.PreferredBackBufferHeight;

            if (actual>ratio)
            {
                ScreenOffset = new Catalyst.Engine.Utilities.Vector2((Graphics.DeviceManager.PreferredBackBufferWidth - (int)(Graphics.DeviceManager.PreferredBackBufferHeight * (ratio))) / 2, 0);
                Screen = new Rectangle((int)ScreenOffset.X, (int)ScreenOffset.Y, (int)(Graphics.DeviceManager.PreferredBackBufferHeight*(ratio)), Graphics.DeviceManager.PreferredBackBufferHeight);
            }
            else if (actual<ratio)
            {
                ScreenOffset = new Catalyst.Engine.Utilities.Vector2(0, (Graphics.DeviceManager.PreferredBackBufferHeight - (int)(Graphics.DeviceManager.PreferredBackBufferWidth * (1 / ratio))) / 2);
                Screen = new Rectangle((int)ScreenOffset.X, (int)ScreenOffset.Y, Graphics.DeviceManager.PreferredBackBufferWidth, (int)(Graphics.DeviceManager.PreferredBackBufferWidth * 1/ratio));
            }
            else
            {
                Screen = new Rectangle(0, 0, Graphics.DeviceManager.PreferredBackBufferWidth, Graphics.DeviceManager.PreferredBackBufferHeight);
                ScreenOffset = Catalyst.Engine.Utilities.Vector2.Zero;
            }

            Graphics.ScreenOffset = ScreenOffset;

            IsFixedTimeStep = false;
            Graphics.DeviceManager.SynchronizeWithVerticalRetrace = false;

            Audio = new AudioManager();


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
            CurrentScene.Initialize();
            Audio.Initialize(false);

            
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            Graphics.SpriteBatch = new SpriteBatch(Graphics.DeviceManager.GraphicsDevice);
            CurrentScene.LoadContent();
            //World = SceneLoader.LoadScene(ContentDirectory);
        }
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            Audio.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            GameTime = gameTime;

            Time.RawDeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Time.DeltaTime = Time.RawDeltaTime * Time.TimeRate;

            CurrentScene.PreUpdate(gameTime);
            CurrentScene.Update(gameTime);
            CurrentScene.PostUpdate(gameTime);
            Audio.Update();
            base.Update(gameTime);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            Graphics.DeviceManager.GraphicsDevice.SetRenderTarget(NativeRenderTarget);
            Graphics.DeviceManager.GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            Graphics.SpriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: Camera.GetTransformation(Graphics.DeviceManager.GraphicsDevice));

            CurrentScene.PreRender(gameTime);
            CurrentScene.Render(gameTime);
            CurrentScene.PostRender(gameTime);

            CurrentScene.RenderUI(gameTime);


            Graphics.SpriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);

            Graphics.DeviceManager.GraphicsDevice.Clear(Color.Black);
            Graphics.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
            Graphics.SpriteBatch.Draw(NativeRenderTarget, Screen, Color.White);
            Graphics.SpriteBatch.End();

            base.Draw(gameTime);

        }
    }
}
