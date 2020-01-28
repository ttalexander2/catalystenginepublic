using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Num = System.Numerics;
using ImGuiNET;
using Catalyst;
using System.Reflection;
using Microsoft.Xna.Framework.Input;
using System.Windows.Forms;
using System.IO;
using CatalystEditor;

namespace Catalyst.XNA
{
    /// <summary>
    /// Simple FNA + ImGui example
    /// </summary>
    public class CatalystEditor : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager Graphics;
        private ImGuiRenderer _imGuiRenderer;

        private Texture2D _xnaTexture;

        public static CatalystEditor Instance { get; private set; }

        public static readonly string AssemblyDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

        private ImGuiLayout _layout;

        private RenderTarget2D RenderTarget;

        private Texture2D Circle;

        public CatalystEditor()
        {
            Instance = this;



            Graphics = new GraphicsDeviceManager(this);
            Graphics.PreferredBackBufferWidth = 1024;
            Graphics.PreferredBackBufferHeight = 768;
            Graphics.PreferMultiSampling = true;

            Catalyst.Engine.Graphics.GraphicsDevice = Graphics;

            IsMouseVisible = true;
            Window.AllowUserResizing = true;


            _layout = new ImGuiLayout();



        }

        protected override void Initialize()
        {
            _imGuiRenderer = new ImGuiRenderer(this);
            _layout.Initialize();
            _imGuiRenderer.RebuildFontAtlas();

            Version v = Assembly.GetExecutingAssembly().GetName().Version;

            Window.Title = "Catalyst " + v.Major + "." + v.MajorRevision;

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
			_layout.ImGuiTexture = _imGuiRenderer.BindTexture(_xnaTexture);

            Catalyst.Engine.Graphics.SpriteBatch = new SpriteBatch(Graphics.GraphicsDevice);


            Circle = Engine.Rendering.BasicShapes.GenerateCircleTexture(15, Color.White, 1);

            RenderTarget = new RenderTarget2D(Catalyst.Engine.Graphics.GraphicsDevice.GraphicsDevice, Catalyst.Engine.Graphics.Width, Catalyst.Engine.Graphics.Height, true, SurfaceFormat.Color, DepthFormat.Depth16, 4, RenderTargetUsage.PreserveContents);

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(0.14f, 0.14f, 0.14f, 1.00f));

            if (ProjectManager.scene_loaded)
            {
                Graphics.GraphicsDevice.SetRenderTarget(RenderTarget);
                //Graphics.GraphicsDevice.Clear(Color.White);


                // TODO: Add your drawing code here
                Catalyst.Engine.Graphics.SpriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: ProjectManager.Current.Camera.GetTransformation(Graphics.GraphicsDevice));

                ProjectManager.Current.PreRender(gameTime);
                ProjectManager.Current.Render(gameTime);
                ProjectManager.Current.PostRender(gameTime);

                Catalyst.Engine.Graphics.SpriteBatch.Draw(Circle, new Vector2(50, 50), Color.White);
                GraphicsDevice.Clear(Color.CornflowerBlue);

                ProjectManager.Current.RenderUI(gameTime);

                Catalyst.Engine.Graphics.SpriteBatch.End();

                Graphics.GraphicsDevice.SetRenderTargets(null);

            }

            // Call BeforeLayout first to set things up
            _imGuiRenderer.BeforeLayout(gameTime);

            // Draw our UI
            _layout.Render(gameTime);
            UpdateMouseCursor();

            // Call AfterLayout now to finish up and draw all the things
            _imGuiRenderer.AfterLayout();

            Catalyst.Engine.Graphics.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
            Catalyst.Engine.Graphics.SpriteBatch.Draw(RenderTarget, new Rectangle(_layout.ViewX, _layout.ViewY, Engine.Graphics.Width, Engine.Graphics.Height), Color.White);
            Catalyst.Engine.Graphics.SpriteBatch.End();

            base.Draw(gameTime);
        }

        // Direct port of the example at https://github.com/ocornut/imgui/blob/master/examples/sdl_opengl2_example/main.cpp


        

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