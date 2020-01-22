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

namespace Catalyst.XNA
{
    /// <summary>
    /// Simple FNA + ImGui example
    /// </summary>
    public class SampleGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private ImGuiRenderer _imGuiRenderer;

        private Texture2D _xnaTexture;

        public static SampleGame Instance { get; private set; }

        public static readonly string AssemblyDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

        private ImGuiLayout _layout;

        public SampleGame()
        {
            Instance = this;



            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1024;
            _graphics.PreferredBackBufferHeight = 768;
            _graphics.PreferMultiSampling = true;

            IsMouseVisible = true;
            Window.AllowUserResizing = true;


            _layout = new ImGuiLayout();


        }

        protected override void Initialize()
        {
            _imGuiRenderer = new ImGuiRenderer(this);
            _layout.Initialize();
            _imGuiRenderer.RebuildFontAtlas();

            base.Initialize();

            Version v = Assembly.GetExecutingAssembly().GetName().Version;

            Window.Title = "Catalyst " + v.Major + "." + v.MajorRevision;

            
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

            base.LoadContent();
        }

        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(new Color(0.14f, 0.14f, 0.14f, 1.00f));

            // Call BeforeLayout first to set things up
            _imGuiRenderer.BeforeLayout(gameTime);

            // Draw our UI
            _layout.Render();
            UpdateMouseCursor();

            // Call AfterLayout now to finish up and draw all the things
            _imGuiRenderer.AfterLayout();

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