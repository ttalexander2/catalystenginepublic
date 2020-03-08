using Catalyst.Engine;
using Catalyst.Engine.Physics;
using Catalyst.Engine.Rendering;
using Catalyst.XNA;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vector2 = System.Numerics.Vector2;

namespace CatalystEditor
{
    public static class ViewportRenderer
    {
        public static float MaxZoom = 5;
        public static float MinZoom = 0.01f;
        public static float Zoom = 1;

        public static bool Grid = true;
        public static int GridSize = 64;

        public static bool Playing = false;

        public static Catalyst.Engine.Utilities.Vector2 Position = Catalyst.Engine.Utilities.Vector2.Zero;
        private static Catalyst.Engine.Utilities.Vector2 _dPos = Catalyst.Engine.Utilities.Vector2.Zero;

        public static void RenderViewPort(GameTime gameTime, Vector2 view_bounds, Rectangle bounds)
        {
            if (!Playing)
            {
                if (ImGui.Button("Update: Off"))
                {
                    Playing = !Playing;
                    ProjectManager.Backup = Catalyst.Engine.Utilities.Utility.DeepClone<Scene>(ProjectManager.Current);
                    Grid = false;
                }
            }
            else
            {
                if (ImGui.Button("Update: On"))
                {
                    Playing = !Playing;
                    ProjectManager.Current = null;
                    ProjectManager.Current = ProjectManager.Backup;
                    ProjectManager.Backup = null;
                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    var dict = ProjectManager.Current.Manager.GetComponentDictionary();
                    foreach (string s in dict.Keys)
                    {
                        if (typeof(Loadable).IsAssignableFrom(Type.GetType(s)))
                        {
                            foreach (Component c in dict[s].Values)
                            {
                                ((Loadable)c).LoadContent();
                            }
                        }
                    }

                    Grid = true;
                }
            }

            ImGui.SameLine();

            if (ImGui.Button("Reset Position"))
            {
                Position = Catalyst.Engine.Utilities.Vector2.Zero;
            }

            ImGui.SameLine();

            ImGui.PushItemWidth(200);
            float prev_zoom = Zoom;


            MouseState m = Mouse.GetState();
            KeyboardState k = Keyboard.GetState();

            _ = ImGui.SliderFloat("Zoom", ref Zoom, MinZoom, MaxZoom);

            //ImGui.SameLine();
            if(bounds.Contains(m.Position))
            {
                Zoom += ImGui.GetIO().MouseWheel / 20;
            }

            if (Zoom < MinZoom)
            {
                Zoom = MinZoom;
            }

            if (Zoom > MaxZoom)
            {
                Zoom = MaxZoom;
            }

            ImGui.SameLine();

            ImGui.Checkbox("Grid", ref Grid);

            ImGui.SameLine();

            int grid = GridSize;


            ImGui.SliderInt("Grid Size", ref GridSize, 1, 256);
            ImGui.SameLine();
            ImGuiLayout.HelpMarker("CTRL+click to input value.");

            if (grid != GridSize)
            {
                ProjectManager.ChangeGrid = true;
            }


            if (bounds.Contains(m.Position) && (m.MiddleButton == ButtonState.Pressed || (k.IsKeyDown(Keys.LeftControl) && m.LeftButton == ButtonState.Pressed)))
            {
                if (_dPos == Catalyst.Engine.Utilities.Vector2.Zero)
                {
                    _dPos = new Catalyst.Engine.Utilities.Vector2(m.Position.X + Position.X, m.Position.Y + Position.Y);
                }
                Position = new Catalyst.Engine.Utilities.Vector2(_dPos.X - m.Position.X, _dPos.Y - m.Position.Y);
            }
            else if (m.MiddleButton == ButtonState.Released)
            {
                _dPos = Catalyst.Engine.Utilities.Vector2.Zero;
            }

            IntPtr p = Catalyst.XNA.CatalystEditor.Instance.Renderer.BindRenderTarget(Catalyst.XNA.CatalystEditor.Instance.RenderTarget);
            ImGui.Image(p, view_bounds);


        }

        public static Matrix GetTransformMatrix()
        {
            Matrix Transform =
                             Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
                             Matrix.CreateRotationZ(0) *
                             Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                             Matrix.CreateTranslation(new Vector3(0, 0, 0));
            return Transform;
        }

    }
}
