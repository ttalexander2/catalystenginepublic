using Catalyst.Engine;
using Catalyst.Engine.Physics;
using Catalyst.Engine.Rendering;
using Catalyst.Editor;
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
using Catalyst.Engine.Utilities;
using Matrix = Microsoft.Xna.Framework.Matrix;
using Vector3 = Microsoft.Xna.Framework.Vector3;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Catalyst.Editor
{
    public static class Viewport
    {
        public static bool ViewportWindowOpen = true;

        public static float MaxZoom = 5;
        public static float MinZoom = 0.01f;
        public static float Zoom = 1;

        public static bool Grid = true;
        public static int GridSize = 64;

        public static bool SnapToCamera = true;

        public static bool Playing = false;

        public static Catalyst.Engine.Utilities.Vector2 Position = Catalyst.Engine.Utilities.Vector2.Zero;
        private static Catalyst.Engine.Utilities.Vector2 _dPos = Catalyst.Engine.Utilities.Vector2.Zero;

        public static Vector2 WindowSize = Vector2.Zero;

        public static void Render(GameTime gameTime)
        {
            WindowSize = ImGui.GetWindowSize() - Vector2.UnitY*(2*ImGui.GetStyle().ItemSpacing.Y + 16) * 2.7f;
            WindowSize = WindowSize - Vector2.UnitX* 10;
            System.Numerics.Vector4 color = System.Numerics.Vector4.Zero;
            unsafe
            {
                color = *ImGui.GetStyleColorVec4(ImGuiCol.Button);
            }

            ImGui.PushStyleColor(ImGuiCol.Button, System.Numerics.Vector4.Zero);

            if (!Playing)
            {
                if (ImGui.ImageButton(IconLoader.RunButton, new Vector2(16,16)))
                {
                    Playing = !Playing;
                    ProjectManager.Backup = Catalyst.Engine.Utilities.Utility.DeepClone<Scene>(ProjectManager.Current);
                    ProjectManager.Current.Initialize();
                    Log.WriteLine($"Scene [{ProjectManager.Current.Name}] running. Changes made while the game is running will not be saved.");
                }
                if (ImGui.IsItemHovered()) ImGui.SetTooltip("Run");

            }
            else
            {
                if (ImGui.ImageButton(IconLoader.StopButton, new Vector2(16, 16)))
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
                        if (typeof(ILoad).IsAssignableFrom(Type.GetType(s)))
                        {
                            foreach (Component c in dict[s].Values)
                            {
                                ((ILoad)c).LoadContent();
                            }
                        }
                    }
                }
                if (ImGui.IsItemHovered()) ImGui.SetTooltip("Stop");


            }


            ImGui.SameLine();

            Vector2 v = ImGui.GetStyle().ItemSpacing;

            ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, new Vector2(0,v.Y));

            if (Grid)
                ImGui.PushStyleColor(ImGuiCol.Button, color);

            if (ImGui.ImageButton(IconLoader.GridButton, new Vector2(16, 16)))
            {
                if (Grid)
                    ImGui.PopStyleColor();
                else
                    ImGui.PushStyleColor(ImGuiCol.Button, color);
                Grid = !Grid;
            }

            if (Grid)
                ImGui.PopStyleColor();

            if (ImGui.IsItemHovered()) ImGui.SetTooltip("Toggle Grid");



            ImGui.SameLine();

            if (ImGui.ImageButton(IconLoader.DownArrow, new Vector2(6, 16)))
            {
                ImGui.OpenPopup("Grid_size");
            }

            if (ImGui.IsItemHovered()) ImGui.SetTooltip("Configure Grid");

            ImGui.PopStyleVar();

            if (ImGui.BeginPopup("Grid_size"))
            {
                int grid = GridSize;


                ImGui.SliderInt("Grid Size", ref GridSize, 1, 256);
                ImGui.SameLine();
                ImGuiLayout.HelpMarker("CTRL+click to input value.");

                if (grid != GridSize)
                {
                    ProjectManager.ChangeGrid = true;
                }

                ImGui.EndPopup();

            }



            ImGui.SameLine();


            if (ImGui.ImageButton(IconLoader.ResetViewButton, new Vector2(16, 16)))
            {
                Position = Catalyst.Engine.Utilities.Vector2.Zero;
            }

            if (ImGui.IsItemHovered()) ImGui.SetTooltip("Reset Position");

            ImGui.SameLine();

            if (ImGui.ImageButton(IconLoader.ZoomButton, new Vector2(16, 16)))
            {
                ImGui.OpenPopup("Zoom_amount");
            }

            if (ImGui.IsItemHovered()) ImGui.SetTooltip("Set Zoom");





            float prev_zoom = Zoom;

            MouseState m = Mouse.GetState();
            KeyboardState k = Keyboard.GetState();

            if (ImGui.BeginPopup("Zoom_amount"))
            {

                _ = ImGui.SliderFloat("##hidelabel Zoom boiiiiiiiaiskdughsa", ref Zoom, MinZoom, MaxZoom);
                ImGui.EndPopup();

            }

            ImGui.SameLine();

            if (SnapToCamera)
                ImGui.PushStyleColor(ImGuiCol.Button, color);

            if (ImGui.ImageButton(IconLoader.Camera, new Vector2(16, 16)))
            {
                if (SnapToCamera)
                    ImGui.PopStyleColor();
                else
                    ImGui.PushStyleColor(ImGuiCol.Button, color);
                SnapToCamera = !SnapToCamera;
            }

            if (SnapToCamera)
                ImGui.PopStyleColor();

            if (ImGui.IsItemHovered()) ImGui.SetTooltip("Snap Viewport to Grid");


            Rectangle bounds = new Rectangle(new Point((int)ImGui.GetWindowPos().X, (int)ImGui.GetWindowPos().Y), new Point((int)ImGui.GetWindowSize().X, (int)ImGui.GetWindowSize().Y));
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

            IntPtr p = Catalyst.Editor.CatalystEditor.Instance.Renderer.BindRenderTarget(Catalyst.Editor.CatalystEditor.Instance.RenderTarget);

            float ratio = (float)Catalyst.Engine.Graphics.Width / (float)Catalyst.Engine.Graphics.Height;
            float actual = (float)WindowSize.X / (float)WindowSize.Y;

            Vector2 size;

            if (actual > ratio)
            {
                size = new Vector2(WindowSize.Y * ratio, WindowSize.Y);
            }
            else if (actual < ratio)
            {
                size = new Vector2(WindowSize.X, (int)(WindowSize.X * 1 / ratio));
            }
            else
            {
                size = new Vector2(WindowSize.X, WindowSize.Y);
            }

            ImGui.Image(p, size);

            ImGui.PopStyleColor();


        }

        public static Matrix GetTransformMatrix()
        {
            if (SnapToCamera)
                return Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
                       Matrix.CreateRotationZ(0) *
                       Matrix.CreateScale(new Vector3(1, 1, 1)) *
                       Matrix.CreateTranslation(new Vector3(0, 0, 0));
            else
                return 
                       Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
                       Matrix.CreateRotationZ(0) *
                       Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                       Matrix.CreateTranslation(new Vector3(0, 0, 0));
        }

    }
}
