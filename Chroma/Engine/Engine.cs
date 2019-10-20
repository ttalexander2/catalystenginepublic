using System;
using System.IO;
using System.Reflection;
using Chroma.Engine.Graphics;
using Chroma.Engine.Input;
using Chroma.Engine.Physics;
using Chroma.Engine.Utilities;
using Chroma.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Chroma.Engine
{

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Engine : Microsoft.Xna.Framework.Game
    {

        // Instances
        public static Engine Instance { get; private set; }
        public World World { get; set; }


        // Screen
        public static int Width { get; private set; }
        public static int Height { get; private set; }
        public static bool Fullscreen { get; private set; }
        public static string Title { get; private set; }

        // Time
        public static FrameCounter Time;

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

        public Engine(int width, int height, string windowTitle, bool fullscreen)
        {
            Engine.Instance = this;
            World = new World();
            Engine.Title = windowTitle;
            Engine.Width = width;
            Engine.Height = height;
            Engine.Fullscreen = fullscreen;
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
                Global.Graphics.PreferredBackBufferWidth = Engine.Width;
                Global.Graphics.PreferredBackBufferHeight = Engine.Height;
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
            World.Initialize();
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

            Scene scene = new Scene(Width, Height);
            scene.Systems.Add(new InputSystem(scene));
            scene.Systems.Add(new PlayerSystem(scene));
            scene.Systems.Add(new GravitySystem(scene));
            scene.Systems.Add(new MovementSystem(scene));
            scene.Systems.Add(new SpriteRenderSystem(scene));

            Texture2D[] atlas = new Texture2D[] {
                Content.Load<Texture2D>(ContentDirectory + "/Sprites/Player/s_player_stationary/s_player_stationary_1"),
                Content.Load<Texture2D>(ContentDirectory + "/Sprites/Player/s_player_stationary/s_player_stationary_2"),
                Content.Load<Texture2D>(ContentDirectory + "/Sprites/Player/s_player_stationary/s_player_stationary_3"),
                Content.Load<Texture2D>(ContentDirectory + "/Sprites/Player/s_player_stationary/s_player_stationary_4"),
                Content.Load<Texture2D>(ContentDirectory + "/Sprites/Player/s_player_stationary/s_player_stationary_5"),
                Content.Load<Texture2D>(ContentDirectory + "/Sprites/Player/s_player_stationary/s_player_stationary_6"),
                Content.Load<Texture2D>(ContentDirectory + "/Sprites/Player/s_player_stationary/s_player_stationary_7") };
            Entity testEntity = scene.Manager.NewEntity();
            var sprite = new CSprite(testEntity, "test", 0, 0, atlas, Origin.TopLeft) { animationSpeed = 6.0f };
            testEntity.AddComponent<CSprite>(sprite);
            testEntity.AddComponents<CInput, CPlayer, CActor>();


            Texture2D[] atlas2 = new Texture2D[] {
                Content.Load<Texture2D>(ContentDirectory + "/Sprites/Player/s_player_stationary/s_player_stationary_1"),
                Content.Load<Texture2D>(ContentDirectory + "/Sprites/Player/s_player_stationary/s_player_stationary_2"),
                Content.Load<Texture2D>(ContentDirectory + "/Sprites/Player/s_player_stationary/s_player_stationary_3"),
                Content.Load<Texture2D>(ContentDirectory + "/Sprites/Player/s_player_stationary/s_player_stationary_4"),
                Content.Load<Texture2D>(ContentDirectory + "/Sprites/Player/s_player_stationary/s_player_stationary_5"),
                Content.Load<Texture2D>(ContentDirectory + "/Sprites/Player/s_player_stationary/s_player_stationary_6"),
                Content.Load<Texture2D>(ContentDirectory + "/Sprites/Player/s_player_stationary/s_player_stationary_7") };
            Entity testEntity2 = scene.Manager.NewEntity();
            var sprite2 = new CSprite(testEntity2, "test", 0, 200, atlas2, Origin.TopLeft) { animationSpeed = 6.0f };
            testEntity2.AddComponent<CSprite>(sprite2);
            testEntity2.AddComponent<CSolid>();

            World.CurrentScene = scene;

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
            Global.gameTime = gameTime;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            


            World.PreUpdate(gameTime);
            World.Update(gameTime);
            World.PostUpdate(gameTime);
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

            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Time.Update(deltaTime);

            var fps = string.Format("FPS: {0}", Time.AverageFramesPerSecond);

            Console.WriteLine(fps);


            World.PreRender(gameTime);
            World.Render(gameTime);
            World.PostRender(gameTime);
            

            // End
            Global.SpriteBatch.End();
            base.Draw(gameTime);

        }
    }
}
