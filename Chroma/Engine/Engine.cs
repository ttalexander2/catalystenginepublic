﻿using System;
using System.IO;
using System.Reflection;
using Chroma.Engine.Graphics;
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
    public class ChromaGame : Microsoft.Xna.Framework.Game
    {

        Actor testEntity;
        // Instances
        public static ChromaGame Instance { get; private set;  }


        // Screen
        public static int Width { get; private set; }
        public static int Height { get; private set; }
        public static int PixelWidth { get; private set; }
        public static int PixelHeight { get; private set; }
        public static bool Fullscreen { get; private set; }
        public static string Title { get; private set; }

        // View
        public Viewport Viewport { get; private set; }

        // Time
        public static float DeltaTime { get; private set; }
        public static float RawDeltaTime { get; private set; }
        public static float TimeRate = 1f;
        public static int Fps;

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

        public ChromaGame(int width, int height, int pixelWidth, int pixelHeight, string windowTitle, bool fullscreen)
        {
            ChromaGame.Instance = this;

            ChromaGame.Title = windowTitle;
            ChromaGame.Width = width;
            ChromaGame.Height = height;
            ChromaGame.PixelWidth = pixelWidth;
            ChromaGame.PixelHeight = pixelHeight;
            ChromaGame.Fullscreen = fullscreen;

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
                Global.Graphics.PreferredBackBufferWidth = ChromaGame.Width;
                Global.Graphics.PreferredBackBufferHeight = ChromaGame.Height;
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
            EntityManager entityManager = new EntityManager();

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
            // TODO: use this.Content to load your game content here

            var scene = new Scene(500, 500);
            var atlas = new Texture2D[] {
                Content.Load<Texture2D>(ContentDirectory + "/Sprites/Player/s_player_stationary/s_player_stationary_1"),
                Content.Load<Texture2D>(ContentDirectory + "/Sprites/Player/s_player_stationary/s_player_stationary_2"),
                Content.Load<Texture2D>(ContentDirectory + "/Sprites/Player/s_player_stationary/s_player_stationary_3"),
                Content.Load<Texture2D>(ContentDirectory + "/Sprites/Player/s_player_stationary/s_player_stationary_4"),
                Content.Load<Texture2D>(ContentDirectory + "/Sprites/Player/s_player_stationary/s_player_stationary_5"),
                Content.Load<Texture2D>(ContentDirectory + "/Sprites/Player/s_player_stationary/s_player_stationary_6"),
                Content.Load<Texture2D>(ContentDirectory + "/Sprites/Player/s_player_stationary/s_player_stationary_7") };
            testEntity = new Actor();
            var sprite = new Sprite(testEntity.UID, "test", 0, 0, atlas, Origin.TopLeft) {animationSpeed = 6.0f};
            scene.Sprites.Add(testEntity.UID, sprite);
            scene.Colliders.Add(testEntity.UID, new BoxCollider(testEntity.UID, sprite.pos, sprite.dims));

            scene.Actors.Add(testEntity.UID, testEntity);

            Alarm timer = new Alarm(new TestScript(), new object[] { "poop" }, false, true, true, 6.0f);
            scene.Alarms.Add(testEntity.UID, timer);

            var entity2 = new Solid();
            var sprite2 = new Sprite(entity2.UID, "test2", 800, 800, new Texture2D[] { Content.Load<Texture2D>(ContentDirectory + "/Sprites/Player/s_player_stationary/s_player_stationary_1") }, Origin.TopLeft);
            scene.Colliders.Add(entity2.UID, new BoxCollider(entity2.UID, sprite2.pos, sprite2.dims));

            scene.Sprites.Add(entity2.UID, sprite2);
            scene.Solids.Add(entity2.UID, entity2);



            World.Scenes.Add(scene);
            World.currentScene = World.Scenes[0];

            


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