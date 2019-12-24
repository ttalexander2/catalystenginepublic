using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Chroma.Engine.Graphics;
using Chroma.Engine.Utilities;
using Chroma.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Chroma.Engine.Audio;
using FMOD.Studio;

namespace Chroma.Engine
{

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class ChromaGame : Microsoft.Xna.Framework.Game
    {

        // Instances
        public static ChromaGame Instance { get; private set; }
        public World World { get; set; }


        // Screen
        public static int Width { get; private set; }
        public static int Height { get; private set; }
        public static bool Fullscreen { get; private set; }
        public static string Title { get; private set; }
        public Camera2D Camera
        {
            get { return World.CurrentScene.Camera; }
            private set { }
        }

        // Time
        public static FrameCounter Time;
        public static float TimeScale { get; set; }

        // DebugDrawQueue
        public static Queue<Action> DebugDrawQueue = new Queue<Action>();

        // Directories

        #if !CONSOLE
        private static readonly string AssemblyDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
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
        internal Vector2 ScreenOffset { get; private set; }

        //Audio
        public AudioManager Audio;

        public ChromaGame(int width, int height, string windowTitle, bool fullscreen)
        {
            ChromaGame.Instance = this;
            World = new World();
            ChromaGame.Title = windowTitle;
            ChromaGame.Width = width;
            ChromaGame.Height = height;
            ChromaGame.Fullscreen = fullscreen;
            Time = new FrameCounter();

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
                Global.Graphics.PreferredBackBufferWidth = Global. PreferredWindowWidth;
                Global.Graphics.PreferredBackBufferHeight = Global.PreferredWindowHeight;
                Global.Graphics.IsFullScreen = false;
            }
        #endif

            Content.RootDirectory = @"Content";
            NativeRenderTarget = new RenderTarget2D(Global.Graphics.GraphicsDevice, width, height);
            Global.Graphics.GraphicsDevice.SetRenderTarget(NativeRenderTarget);
            Global.Graphics.ApplyChanges();

            float ratio = (float)Global.Width / (float)Global.Height;
            float actual = (float)Global.Graphics.PreferredBackBufferWidth / (float)Global.Graphics.PreferredBackBufferHeight;

            if (actual>ratio)
            {
                ScreenOffset = new Vector2((Global.Graphics.PreferredBackBufferWidth - (int)(Global.Graphics.PreferredBackBufferHeight * (ratio))) / 2, 0);
                Screen = new Rectangle((int)ScreenOffset.X, (int)ScreenOffset.Y, (int)(Global.Graphics.PreferredBackBufferHeight*(ratio)), Global.Graphics.PreferredBackBufferHeight);
            }
            else if (actual<ratio)
            {
                ScreenOffset = new Vector2(0, (Global.Graphics.PreferredBackBufferHeight - (int)(Global.Graphics.PreferredBackBufferWidth * (1 / ratio))) / 2);
                Screen = new Rectangle((int)ScreenOffset.X, (int)ScreenOffset.Y, Global.Graphics.PreferredBackBufferWidth, (int)(Global.Graphics.PreferredBackBufferWidth * 1/ratio));
            }
            else
            {
                Screen = new Rectangle(0, 0, Global.Graphics.PreferredBackBufferWidth, Global.Graphics.PreferredBackBufferHeight);
                ScreenOffset = Vector2.Zero;
            }

            Global.ScreenOffset = ScreenOffset;

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
            World.Initialize();
            Audio.Initialize();

            
            base.Initialize();
#if DEBUG
            Console.WriteLine("Initialized");
#endif
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            Global.SpriteBatch = new SpriteBatch(Global.Graphics.GraphicsDevice);

            World = SceneLoader.LoadScene(ContentDirectory);
            FMOD.Studio.EventDescription d;
            Console.WriteLine(Audio.StudioSystem.getEvent("event:/forest_test", out d));
            FMOD.Studio.EventInstance i;
            d.createInstance(out i);
            i.start();


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
            Global.gameTime = gameTime;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad2))
                Camera.MoveTowards(new Vector2(0, 5));
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad8))
                Camera.MoveTowards(new Vector2(0, -5));
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad4))
                Camera.MoveTowards(new Vector2(-5, 0));
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad6))
                Camera.MoveTowards(new Vector2(5, 0));


            World.PreUpdate(gameTime);
            World.Update(gameTime);
            World.PostUpdate(gameTime);
            Audio.Update();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            Global.Graphics.GraphicsDevice.SetRenderTarget(NativeRenderTarget);
            Global.Graphics.GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            Global.SpriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: Camera.GetTransformation(Global.Graphics.GraphicsDevice));

            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Time.Update(deltaTime);

            var fps = string.Format("FPS: {0}", Time.AverageFramesPerSecond);

            //Console.WriteLine(fps);


            World.PreRender(gameTime);
            World.Render(gameTime);
            World.PostRender(gameTime);

            while (DebugDrawQueue.Count > 0)
            {
                DebugDrawQueue.Dequeue().Invoke();
            }
            

            // End
            Global.SpriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);

            Global.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
            Global.SpriteBatch.Draw(NativeRenderTarget, Screen, Color.White);
            Global.SpriteBatch.End();

            Global.SpriteBatch.Begin(transformMatrix: Camera.GetScaledTransformation(Global.Graphics.GraphicsDevice));
            World.RenderNative(gameTime);
            World.RenderUI(gameTime);
            Global.SpriteBatch.End();
            base.Draw(gameTime);

        }
    }
}
