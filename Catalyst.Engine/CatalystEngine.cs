using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Catalyst.Engine.Rendering;
using Catalyst.Engine.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Catalyst.Engine.Audio;
using FMOD.Studio;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Color = Microsoft.Xna.Framework.Color;

namespace Catalyst.Engine
{

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public partial class CatalystEngine : Microsoft.Xna.Framework.Game
    {

        // Instances
        public static CatalystEngine Instance { get; private set; }
        public Scene CurrentScene { get; set; }


        // Screen
        public static int Width { get; private set; }
        public static int Height { get; private set; }
        public static bool Fullscreen { get; private set; }
        public static string Title { get; private set; }
        public Camera2D Camera
        {
            get { return CurrentScene.Camera; }
            private set { }
        }

        // Time
        public static FrameCounter Time;
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
        internal Utilities.Vector2 ScreenOffset { get; private set; }

        //Audio
        public AudioManager Audio;
        public CatalystEngine(Scene scene, int width, int height, string windowTitle, bool fullscreen) : this(width, height, windowTitle, fullscreen)
        {
            CurrentScene = scene;
        }
        public CatalystEngine(int width, int height, string windowTitle, bool fullscreen)
        {
#if OSX
            LibraryLoader.LoadPosixLibrary(Path.Combine(AssemblyDirectory, "libfmod.dylib"));
            LibraryLoader.LoadPosixLibrary(Path.Combine(AssemblyDirectory, "libfmodstudio.dylib"));
#elif LINUX32
            LibraryLoader.LoadPosixLibrary(Path.Combine(AssemblyDirectory, "x86", "libfmod.so"));
            LibraryLoader.LoadPosixLibrary(Path.Combine(AssemblyDirectory, "x86", "libfmodstudio.so"));
#elif LINUX64
            LibraryLoader.LoadPosixLibrary(Path.Combine(AssemblyDirectory, "x64", "libfmod.so"));
            LibraryLoader.LoadPosixLibrary(Path.Combine(AssemblyDirectory, "x64", "libfmodstudio.so"));
#endif
            CurrentScene = new Scene(1920, 1080);

            CatalystEngine.Instance = this;
            CatalystEngine.Title = windowTitle;
            CatalystEngine.Width = width;
            CatalystEngine.Height = height;
            CatalystEngine.Fullscreen = fullscreen;
            Time = new FrameCounter();

            Graphics.GraphicsDevice = new GraphicsDeviceManager(this);
            Graphics.GraphicsDevice.SynchronizeWithVerticalRetrace = true;
            Graphics.GraphicsDevice.PreferMultiSampling = false;
            Graphics.GraphicsDevice.GraphicsProfile = GraphicsProfile.HiDef;
            Graphics.GraphicsDevice.PreferredBackBufferFormat = SurfaceFormat.Color;
            Graphics.GraphicsDevice.PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8;
            Graphics.GraphicsDevice.ApplyChanges();

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
                Graphics.GraphicsDevice.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                Graphics.GraphicsDevice.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                Graphics.GraphicsDevice.IsFullScreen = true;
            }
            else
            {
                Graphics.GraphicsDevice.PreferredBackBufferWidth = Graphics. PreferredWindowWidth;
                Graphics.GraphicsDevice.PreferredBackBufferHeight = Graphics.PreferredWindowHeight;
                Graphics.GraphicsDevice.IsFullScreen = false;
            }
#endif

            Content.RootDirectory = @"Content";
            NativeRenderTarget = new RenderTarget2D(Graphics.GraphicsDevice.GraphicsDevice, width, height);
            Graphics.GraphicsDevice.GraphicsDevice.SetRenderTarget(NativeRenderTarget);
            Graphics.GraphicsDevice.ApplyChanges();

            float ratio = (float)Graphics.Width / (float)Graphics.Height;
            float actual = (float)Graphics.GraphicsDevice.PreferredBackBufferWidth / (float)Graphics.GraphicsDevice.PreferredBackBufferHeight;

            if (actual>ratio)
            {
                ScreenOffset = new Utilities.Vector2((Graphics.GraphicsDevice.PreferredBackBufferWidth - (int)(Graphics.GraphicsDevice.PreferredBackBufferHeight * (ratio))) / 2, 0);
                Screen = new Rectangle((int)ScreenOffset.X, (int)ScreenOffset.Y, (int)(Graphics.GraphicsDevice.PreferredBackBufferHeight*(ratio)), Graphics.GraphicsDevice.PreferredBackBufferHeight);
            }
            else if (actual<ratio)
            {
                ScreenOffset = new Utilities.Vector2(0, (Graphics.GraphicsDevice.PreferredBackBufferHeight - (int)(Graphics.GraphicsDevice.PreferredBackBufferWidth * (1 / ratio))) / 2);
                Screen = new Rectangle((int)ScreenOffset.X, (int)ScreenOffset.Y, Graphics.GraphicsDevice.PreferredBackBufferWidth, (int)(Graphics.GraphicsDevice.PreferredBackBufferWidth * 1/ratio));
            }
            else
            {
                Screen = new Rectangle(0, 0, Graphics.GraphicsDevice.PreferredBackBufferWidth, Graphics.GraphicsDevice.PreferredBackBufferHeight);
                ScreenOffset = Utilities.Vector2.Zero;
            }

            Graphics.ScreenOffset = ScreenOffset;

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
            Graphics.SpriteBatch = new SpriteBatch(Graphics.GraphicsDevice.GraphicsDevice);

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
            Graphics.gameTime = gameTime;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad2))
                Camera.MoveTowards(new Utilities.Vector2(0, 5));
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad8))
                Camera.MoveTowards(new Utilities.Vector2(0, -5));
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad4))
                Camera.MoveTowards(new Utilities.Vector2(-5, 0));
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad6))
                Camera.MoveTowards(new Utilities.Vector2(5, 0));


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
            Graphics.GraphicsDevice.GraphicsDevice.SetRenderTarget(NativeRenderTarget);
            Graphics.GraphicsDevice.GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            Graphics.SpriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: Camera.GetTransformation(Graphics.GraphicsDevice.GraphicsDevice));

            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Time.Update(deltaTime);

            var fps = string.Format("FPS: {0}", Time.AverageFramesPerSecond);

            //Console.WriteLine(fps);


            CurrentScene.PreRender(gameTime);
            CurrentScene.Render(gameTime);
            CurrentScene.PostRender(gameTime);
            

            // End
            Graphics.SpriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);

            Graphics.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
            Graphics.SpriteBatch.Draw(NativeRenderTarget, Screen, Color.White);
            Graphics.SpriteBatch.End();

            Graphics.SpriteBatch.Begin(transformMatrix: Camera.GetScaledTransformation(Graphics.GraphicsDevice.GraphicsDevice));
            CurrentScene.RenderNative(gameTime);
            CurrentScene.RenderUI(gameTime);
            Graphics.SpriteBatch.End();
            base.Draw(gameTime);

        }
    }
}
