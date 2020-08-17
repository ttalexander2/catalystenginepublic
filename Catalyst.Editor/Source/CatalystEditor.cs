using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Num = System.Numerics;
using ImGuiNET;
using Catalyst;
using System.Reflection;
using Microsoft.Xna.Framework.Input;
using System.IO;
using CatalystEditor;
using Catalyst.Engine;
using CatalystEditor.Source;
using System.Threading;

namespace Catalyst.Editor
{
    /// <summary>
    /// Simple FNA + ImGui example
    /// </summary>
    public class CatalystEditor : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager Graphics;
        public ImGuiRenderer Renderer;

        private Texture2D _xnaTexture;

        public static CatalystEditor Instance { get; private set; }

        public static readonly string AssemblyDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

        private ImGuiLayout _layout;

        public RenderTarget2D RenderTarget;

        public volatile RenderTarget2D GridTarget;

        public volatile RenderTarget2D BackgroundTarget;

        private Color _backgroundColor;

        private Texture2D _pixel;

        public IntPtr RenderTargetPointer;

        public KeyboardState keyboardState;

        public CatalystEditor()
        {
            Instance = this;



            Graphics = new GraphicsDeviceManager(this);
            Graphics.PreferredBackBufferWidth = 1920;
            Graphics.PreferredBackBufferHeight = 1080;
            Graphics.PreferMultiSampling = true;

            Catalyst.Engine.Graphics.DeviceManager = Graphics;

            IsMouseVisible = true;


            _layout = new ImGuiLayout();

            Catalyst.Engine.Graphics.Content = Content;

            Window.IsBorderless = true;
            Window.AllowUserResizing = true;

            //InactiveSleepTime = new TimeSpan(0);

            IsFixedTimeStep = true;
            Graphics.SynchronizeWithVerticalRetrace = false;
            TargetElapsedTime = TimeSpan.FromSeconds(1d / Engine.Graphics.FPSCap);

        }

        protected override void Initialize()
        {
            Renderer = new ImGuiRenderer(this);
            _layout.Initialize();
            Renderer.RebuildFontAtlas();
            Input.Update();

            base.Initialize(); 
        }

        protected override void LoadContent()
        {
            // Texture loading example

			// First, load the texture as a Texture2D (can also be done using the XNA/FNA content pipeline)
			_xnaTexture = CreateTexture(GraphicsDevice, 300, 150, pixel =>
			{
				var red = (pixel % 300) / 2;
				return new Color(red, 1, 1);
			});

			// Then, bind it to an ImGui-friendly pointer, that we can use during regular ImGui.** calls (see below)
			_layout.ImGuiTexture = Renderer.BindTexture(_xnaTexture);

            Catalyst.Engine.Graphics.SpriteBatch = new SpriteBatch(Graphics.GraphicsDevice);

            _backgroundColor = new Color(new Vector4(0.11f, 0.11f, 0.11f, 1.00f));

            _pixel = new Texture2D(Catalyst.Engine.Graphics.DeviceManager.GraphicsDevice, 1, 1);
            _pixel.SetData(new Color[] { Color.White });

            IconLoader.LoadIcons();

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            Input.Update();
            Time.RawDeltaTime = gameTime.ElapsedGameTime.TotalSeconds;
            Time.DeltaTime = Time.RawDeltaTime * Time.TimeRate;

            keyboardState = Keyboard.GetState();
            if (ProjectManager.Current != null && global::Catalyst.Editor.Viewport.Playing)
            {
                ProjectManager.Current.PreUpdate(gameTime);
                ProjectManager.Current.Update(gameTime);
                ProjectManager.Current.PostUpdate(gameTime);
            }
            else if (ProjectManager.Current != null && !global::Catalyst.Editor.Viewport.Playing)
            {

                foreach (Engine.System s in ProjectManager.Current.Systems)
                {
                    if (s is RenderSystem)
                    {
                        s.PreUpdate(gameTime);
                        s.Update(gameTime);
                        s.PostUpdate(gameTime);
                    }
                }
            }
            NewTextureWindow.RemoveTextures();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Window.Title = String.Format("Catalyst Editor, FPS: {0}", 1 / (float)gameTime.ElapsedGameTime.TotalSeconds);
            GraphicsDevice.Clear(_backgroundColor);

            if (ProjectManager.Current != null)
            {
                if (RenderTarget == null)
                {
                    RenderTarget = new RenderTarget2D(Graphics.GraphicsDevice, ProjectManager.Current.Width, ProjectManager.Current.Height);
                }

                if (RenderTarget.Width != (int)Viewport.WindowSize.X || RenderTarget.Height != (int)Viewport.WindowSize.Y)
                {
                    if (Viewport.WindowSize.X > 0 && Viewport.WindowSize.Y > 0)
                    {
                        RenderTarget.Dispose();
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        RenderTarget = new RenderTarget2D(Graphics.GraphicsDevice, (int)Viewport.WindowSize.X, (int)Viewport.WindowSize.Y);
                    }

                }

                Graphics.GraphicsDevice.SetRenderTarget(RenderTarget);

                GraphicsDevice.Clear(_backgroundColor);

                if (global::Catalyst.Editor.Viewport.SnapToCamera)
                {
                    Catalyst.Engine.Graphics.SpriteBatch.Begin(blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp, transformMatrix: ProjectManager.Current.Camera.GetScaledTransformation(Graphics.GraphicsDevice, 2.985f * Viewport.SnapZoom));
                }
                else
                {
                    Catalyst.Engine.Graphics.SpriteBatch.Begin(blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp, transformMatrix: global::Catalyst.Editor.Viewport.GetTransformMatrix());
                }

                if (BackgroundTarget != null)
                    Engine.Graphics.SpriteBatch.Draw(BackgroundTarget, new Vector2(0, 0), null, Color.White);
                Engine.Graphics.SpriteBatch.End();


                /**
                 * If update occurs, set transformMatrix to camera, else display entire scene and control zoom with transform matrix.
                 */
                if (global::Catalyst.Editor.Viewport.SnapToCamera)
                {
                    Catalyst.Engine.Graphics.SpriteBatch.Begin(blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp, transformMatrix: ProjectManager.Current.Camera.GetScaledTransformation(Graphics.GraphicsDevice, 2.985f*Viewport.SnapZoom));
                }
                else
                {
                    Catalyst.Engine.Graphics.SpriteBatch.Begin(blendState: BlendState.AlphaBlend, samplerState: SamplerState.PointClamp, transformMatrix: global::Catalyst.Editor.Viewport.GetTransformMatrix());
                }




                ProjectManager.Current.PreRender(gameTime);
                ProjectManager.Current.Render(gameTime);
                ProjectManager.Current.PostRender(gameTime);


                //Catalyst.Engine.Graphics.SpriteBatch.Draw(_testTexture, new Vector2(0, 0), null, Color.White);

                MouseState m = Mouse.GetState();

                ProjectManager.Current.RenderUI(gameTime);

                if (GridTarget != null && Viewport.Grid)
                    Engine.Graphics.SpriteBatch.Draw(GridTarget, new Vector2(0, 0), null, Color.White);

                if (Viewport.Debug)
                {
                    Engine.Graphics.SpriteBatch.Draw(_pixel, new Vector2(ProjectManager.Current.Camera.Position.X, ProjectManager.Current.Camera.Position.Y), null, Color.CornflowerBlue, 0, Vector2.Zero, new Vector2(ProjectManager.Current.Camera.Size.X, 3), new SpriteEffects(), 0);
                    Engine.Graphics.SpriteBatch.Draw(_pixel, new Vector2(ProjectManager.Current.Camera.Position.X, ProjectManager.Current.Camera.Position.Y), null, Color.CornflowerBlue, 0, Vector2.Zero, new Vector2(3, ProjectManager.Current.Camera.Size.Y), new SpriteEffects(), 0);
                    Engine.Graphics.SpriteBatch.Draw(_pixel, new Vector2(ProjectManager.Current.Camera.Position.X, ProjectManager.Current.Camera.Position.Y + ProjectManager.Current.Camera.Size.Y), null, Color.CornflowerBlue, 0, Vector2.Zero, new Vector2(ProjectManager.Current.Camera.Size.X + 3, 3), new SpriteEffects(), 0);
                    Engine.Graphics.SpriteBatch.Draw(_pixel, new Vector2(ProjectManager.Current.Camera.Position.X + ProjectManager.Current.Camera.Size.X, ProjectManager.Current.Camera.Position.Y), null, Color.CornflowerBlue, 0, Vector2.Zero, new Vector2(3, ProjectManager.Current.Camera.Size.Y + 3), new SpriteEffects(), 0);
                    ProjectManager.Current.DebugRender(gameTime);
                }


                Catalyst.Engine.Graphics.SpriteBatch.End();

                Graphics.GraphicsDevice.SetRenderTargets(null);


            }

            // Call BeforeLayout first to set things up
            Renderer.BeforeLayout(gameTime);

            // Draw our UI
            _layout.Render(gameTime);

            UpdateMouseCursor();

            // Call AfterLayout now to finish up and draw all the things

            Renderer.AfterLayout();

            /**
            Catalyst.Engine.Graphics.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
            Catalyst.Engine.Graphics.SpriteBatch.Draw(RenderTarget, _layout.ViewBounds, Color.White);
            Catalyst.Engine.Graphics.SpriteBatch.End();
            */

            if (ProjectManager.Current != null && ProjectManager.ChangeGrid)
            {
                UpdateGrid();
                UpdateBackground();
                ProjectManager.ChangeGrid = false;
            }

            base.Draw(gameTime);
        }

        private void DrawBorder(Rectangle rectangleToDraw, int thicknessOfBorder, Color borderColor)
        {
            // Draw top line
            Catalyst.Engine.Graphics.SpriteBatch.Draw(_pixel, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, rectangleToDraw.Width, thicknessOfBorder), borderColor);

            // Draw left line
            Catalyst.Engine.Graphics.SpriteBatch.Draw(_pixel, new Rectangle(rectangleToDraw.X, rectangleToDraw.Y, thicknessOfBorder, rectangleToDraw.Height), borderColor);

            // Draw right line
            Catalyst.Engine.Graphics.SpriteBatch.Draw(_pixel, new Rectangle((rectangleToDraw.X + rectangleToDraw.Width - thicknessOfBorder),
                                            rectangleToDraw.Y,
                                            thicknessOfBorder,
                                            rectangleToDraw.Height), borderColor);
            // Draw bottom line
            Catalyst.Engine.Graphics.SpriteBatch.Draw(_pixel, new Rectangle(rectangleToDraw.X,
                                            rectangleToDraw.Y + rectangleToDraw.Height - thicknessOfBorder,
                                            rectangleToDraw.Width,
                                            thicknessOfBorder), borderColor);

        }

        private void UpdateGrid()
        {
            if (GridTarget != null && (GridTarget.Width != ProjectManager.Current.Width || GridTarget.Height != ProjectManager.Current.Height))
            {
                GridTarget.Dispose();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            GridTarget = new RenderTarget2D(Graphics.GraphicsDevice, ProjectManager.Current.Width + 1, ProjectManager.Current.Height + 1);

            Graphics.GraphicsDevice.SetRenderTarget(GridTarget);
            Catalyst.Engine.Graphics.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap);

            Graphics.GraphicsDevice.Clear(Color.Transparent);


            int tileSize = global::Catalyst.Editor.Viewport.GridSize;
            for (int i = 0; i <= ProjectManager.Current.Width; i += tileSize)
            {
                if (i == 0 || i == ProjectManager.Current.Width)
                {
                    Catalyst.Engine.Graphics.SpriteBatch.Draw(_pixel, new Vector2(i, 0), null, Color.White * 0.7f, 0, Vector2.Zero, new Vector2(3, ProjectManager.Current.Height), new SpriteEffects(), 0);
                }
                else
                {
                    Catalyst.Engine.Graphics.SpriteBatch.Draw(_pixel, new Vector2(i, 0), null, Color.White * 0.5f, 0, Vector2.Zero, new Vector2(1, ProjectManager.Current.Height), new SpriteEffects(), 0);
                }
            }

            Catalyst.Engine.Graphics.SpriteBatch.Draw(_pixel, new Vector2(ProjectManager.Current.Width-3, 0), null, Color.White * 0.7f, 0, Vector2.Zero, new Vector2(3, ProjectManager.Current.Height), new SpriteEffects(), 0);


            for (int i = 0; i <= ProjectManager.Current.Height; i += tileSize)
            {
                if (i == 0 || i == ProjectManager.Current.Height)
                {
                    Catalyst.Engine.Graphics.SpriteBatch.Draw(_pixel, new Vector2(0, i), null, Color.White * 0.7f, 0, Vector2.Zero, new Vector2(ProjectManager.Current.Width, 3), new SpriteEffects(), 0);
                }
                else
                {
                    Engine.Graphics.SpriteBatch.Draw(_pixel, new Vector2(0, i), null, Color.White * 0.5f, 0, Vector2.Zero, new Vector2(ProjectManager.Current.Width, 1), new SpriteEffects(), 0);
                }
            }

            Engine.Graphics.SpriteBatch.Draw(_pixel, new Vector2(0, ProjectManager.Current.Height-3), null, Color.White * 0.7f, 0, Vector2.Zero, new Vector2(ProjectManager.Current.Width, 3), new SpriteEffects(), 0);



            Engine.Graphics.SpriteBatch.End();
            Graphics.GraphicsDevice.SetRenderTargets(null);

            
        }


        private void UpdateBackground()
        {
            if (BackgroundTarget != null && (BackgroundTarget.Width != ProjectManager.Current.Width || BackgroundTarget.Height != ProjectManager.Current.Height))
            {
                BackgroundTarget.Dispose();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            BackgroundTarget = new RenderTarget2D(Graphics.GraphicsDevice, ProjectManager.Current.Width + 1, ProjectManager.Current.Height + 1);

            Graphics.GraphicsDevice.SetRenderTarget(BackgroundTarget);
            Catalyst.Engine.Graphics.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap);

            Graphics.GraphicsDevice.Clear(_backgroundColor);


            bool switchColor = true;

            int tileSize = global::Catalyst.Editor.Viewport.GridSize;
            for (int i = 0; i <= ProjectManager.Current.Height; i += tileSize)
            {
                for (int j = 0; j <= ProjectManager.Current.Width; j += tileSize)
                {
                    if (switchColor)
                        Catalyst.Engine.Graphics.SpriteBatch.Draw(_pixel, new Vector2(j, i), null, Color.Gray*0.5f, 0, Vector2.Zero, new Vector2(tileSize, tileSize), new SpriteEffects(), 0);
                    else
                        Catalyst.Engine.Graphics.SpriteBatch.Draw(_pixel, new Vector2(j, i), null, Color.DarkGray * 0.5f, 0, Vector2.Zero, new Vector2(tileSize, tileSize), new SpriteEffects(), 0);
                    switchColor = !switchColor;
                }

            }


            Engine.Graphics.SpriteBatch.End();
            Graphics.GraphicsDevice.SetRenderTargets(null);
        }




        public static Texture2D CreateTexture(GraphicsDevice device, int width, int height, Func<int, Color> paint)
		{
			//initialize a texture
			var texture = new Texture2D(device, width, height);

			//the array holds the color for each pixel in the texture
			Color[] data = new Color[width * height];
			for(var pixel = 0; pixel < data.Length; pixel++)
			{
				//the function applies the color according to the specified pixel
				data[pixel] = paint( pixel );
			}

			//set the color
			texture.SetData( data );

			return texture;
		}


        private void UpdateMouseCursor()
        {
            if (ImGui.GetMouseCursor().HasFlag(ImGuiMouseCursor.ResizeAll))
            {
                Mouse.SetCursor(MouseCursor.SizeAll);
            }
            else if (ImGui.GetMouseCursor().HasFlag(ImGuiMouseCursor.ResizeEW))
            {
                Mouse.SetCursor(MouseCursor.SizeWE);
            }
            else if (ImGui.GetMouseCursor().HasFlag(ImGuiMouseCursor.ResizeNS))
            {
                Mouse.SetCursor(MouseCursor.SizeNS);
            }
            else if (ImGui.GetMouseCursor().HasFlag(ImGuiMouseCursor.ResizeNESW))
            {
                Mouse.SetCursor(MouseCursor.SizeNESW);
            }
            else if (ImGui.GetMouseCursor().HasFlag(ImGuiMouseCursor.ResizeNWSE))
            {
                Mouse.SetCursor(MouseCursor.SizeNWSE);
            }
            else if (ImGui.GetMouseCursor().HasFlag(ImGuiMouseCursor.Arrow))
            {
                Mouse.SetCursor(MouseCursor.Arrow);
            }
            else if (ImGui.GetMouseCursor().HasFlag(ImGuiMouseCursor.TextInput))
            {
                Mouse.SetCursor(MouseCursor.Crosshair);
            }
            else if (ImGui.GetMouseCursor().HasFlag(ImGuiMouseCursor.Hand))
            {
                Mouse.SetCursor(MouseCursor.Hand);
            }
            else
            {
                Mouse.SetCursor(MouseCursor.Arrow);
            }


        }
    }
}