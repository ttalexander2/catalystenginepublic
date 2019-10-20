using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Chroma.Engine.Utilities
{
    public class DialogBoxWindow : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        internal static DialogBoxWindow Instance;

        // Allow the current and previous keyboard states to be accessed outside this class (read-only)
        public KeyboardState KeyState { get; private set; }
        public KeyboardState PreviousKeyState { get; private set; }

        // Allow the gamepad state to be accessed outside this class (read-only)
        public GamePadState GamePadState { get; private set; }

        // Build a default font for the game
        public SpriteFont DialogFont { get; private set; }

        // Shortcut for finding center point of screen
        public Vector2 CenterScreen
            => new Vector2(graphics.GraphicsDevice.Viewport.Width / 2f, graphics.GraphicsDevice.Viewport.Height / 2f);

        private DialogBox _dialogBox;

        private string text;

        public DialogBoxWindow(string text)
        {
            Instance = this;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.text = text;


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

            // TODO: use this.Content to load your game content here

            // Don't forget to initialize your sprite font!
            DialogFont = Content.Load<SpriteFont>("dialog");

            // Build a new dialog box and give it some text

                        _dialogBox = new DialogBox
            {
                Text = text
            };

            // Initialize the dialog box (this also calls the Show() method)
            _dialogBox.Initialize();

            graphics.PreferredBackBufferWidth = (int)_dialogBox.Size.X;
            graphics.PreferredBackBufferHeight = (int)_dialogBox.Size.Y;
            graphics.ApplyChanges();
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
            if (GamePadState.Buttons.Back == ButtonState.Pressed || KeyState.IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            // NOTE: You may want to disable player movement and other standard actions while the dialog box is displayed
            // or maybe not if you want it to just show up as the player is going along

            // Update the dialog box (essentially, process key/button input)
            _dialogBox.Update();

            // Debug key to show opening a new dialog box on demand

            // Update input states
            PreviousKeyState = KeyState;
            KeyState = Keyboard.GetState();
            GamePadState = GamePad.GetState(PlayerIndex.One);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            // TODO: Add your drawing code here

            // NOTE: The NonPremultiplied blendstate is used to make the dialog box partially transparent
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

            // Draw the dialog box to the screen
            _dialogBox.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
