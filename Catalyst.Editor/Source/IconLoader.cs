using ImGuiNET;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace Catalyst.Editor
{
    public static class IconLoader
    {
        public static Vector2 Icon16Size = new Vector2(16, 16);
        public static Vector2 Icon32Size = new Vector2(32, 32);
        public static IntPtr RunButton { get; private set; }
        public static IntPtr PauseButton { get; private set; }
        public static IntPtr StopButton { get; private set; }
        public static IntPtr GridButton { get; private set; }
        public static IntPtr ResetViewButton { get; private set; }
        public static IntPtr DownArrow { get; private set; }
        public static IntPtr ZoomButton { get; private set; }
        public static IntPtr Camera { get; private set; }
        public static IntPtr Entity { get; private set; }
        public static IntPtr MonoEntity { get; private set; }
        public static IntPtr Visible { get; private set; }
        public static IntPtr NotVisible { get; private set; }
        public static IntPtr Folder { get; private set; }
        public static IntPtr ProgramIcon { get; private set; }
        public static IntPtr Close { get; private set; }
        public static IntPtr Minimize { get; private set; }
        public static IntPtr Maximize { get; private set; }
        public static IntPtr RestoreDown { get; private set; }

        public static void LoadIcons()
        {
            using (FileStream fs = new FileStream("Icons/Icon.png", FileMode.Open))
            {
                ProgramIcon = Catalyst.Editor.CatalystEditor.Instance.Renderer.BindTexture(Texture2D.FromStream(Catalyst.Editor.CatalystEditor.Instance.GraphicsDevice, fs));
            }

            using (FileStream fs = new FileStream("Icons/Close_16x.png", FileMode.Open))
            {
                Close = Catalyst.Editor.CatalystEditor.Instance.Renderer.BindTexture(Texture2D.FromStream(Catalyst.Editor.CatalystEditor.Instance.GraphicsDevice, fs));
            }

            using (FileStream fs = new FileStream("Icons/Minimize_16x.png", FileMode.Open))
            {
                Minimize = Catalyst.Editor.CatalystEditor.Instance.Renderer.BindTexture(Texture2D.FromStream(Catalyst.Editor.CatalystEditor.Instance.GraphicsDevice, fs));
            }

            using (FileStream fs = new FileStream("Icons/Maximize_16x.png", FileMode.Open))
            {
                Maximize = Catalyst.Editor.CatalystEditor.Instance.Renderer.BindTexture(Texture2D.FromStream(Catalyst.Editor.CatalystEditor.Instance.GraphicsDevice, fs));
            }

            using (FileStream fs = new FileStream("Icons/RestoreDown_16x.png", FileMode.Open))
            {
                RestoreDown = Catalyst.Editor.CatalystEditor.Instance.Renderer.BindTexture(Texture2D.FromStream(Catalyst.Editor.CatalystEditor.Instance.GraphicsDevice, fs));
            }

            using (FileStream fs = new FileStream("Icons/Run_16x.png", FileMode.Open))
            {
                RunButton = Catalyst.Editor.CatalystEditor.Instance.Renderer.BindTexture(Texture2D.FromStream(Catalyst.Editor.CatalystEditor.Instance.GraphicsDevice, fs));
            }

            using (FileStream fs = new FileStream("Icons/Pause_16x.png", FileMode.Open))
            {
                PauseButton = Catalyst.Editor.CatalystEditor.Instance.Renderer.BindTexture(Texture2D.FromStream(Catalyst.Editor.CatalystEditor.Instance.GraphicsDevice, fs));
            }

            using (FileStream fs = new FileStream("Icons/Stop_16x.png", FileMode.Open))
            {
                StopButton = Catalyst.Editor.CatalystEditor.Instance.Renderer.BindTexture(Texture2D.FromStream(Catalyst.Editor.CatalystEditor.Instance.GraphicsDevice, fs));
            }

            using (FileStream fs = new FileStream("Icons/GridUniform_16x.png", FileMode.Open))
            {
                GridButton = Catalyst.Editor.CatalystEditor.Instance.Renderer.BindTexture(Texture2D.FromStream(Catalyst.Editor.CatalystEditor.Instance.GraphicsDevice, fs));
            }

            using (FileStream fs = new FileStream("Icons/AligntoGrid_16x.png", FileMode.Open))
            {
                ResetViewButton = Catalyst.Editor.CatalystEditor.Instance.Renderer.BindTexture(Texture2D.FromStream(Catalyst.Editor.CatalystEditor.Instance.GraphicsDevice, fs));
            }

            using (FileStream fs = new FileStream("Icons/DownArrow_6x16.png", FileMode.Open))
            {
                DownArrow = Catalyst.Editor.CatalystEditor.Instance.Renderer.BindTexture(Texture2D.FromStream(Catalyst.Editor.CatalystEditor.Instance.GraphicsDevice, fs));
            }

            using (FileStream fs = new FileStream("Icons/ZoomControl_16x.png", FileMode.Open))
            {
                ZoomButton = Catalyst.Editor.CatalystEditor.Instance.Renderer.BindTexture(Texture2D.FromStream(Catalyst.Editor.CatalystEditor.Instance.GraphicsDevice, fs));
            }

            using (FileStream fs = new FileStream("Icons/Camera_16x.png", FileMode.Open))
            {
                Camera = Catalyst.Editor.CatalystEditor.Instance.Renderer.BindTexture(Texture2D.FromStream(Catalyst.Editor.CatalystEditor.Instance.GraphicsDevice, fs));
            }

            using (FileStream fs = new FileStream("Icons/Entity_16x.png", FileMode.Open))
            {
                Entity = Catalyst.Editor.CatalystEditor.Instance.Renderer.BindTexture(Texture2D.FromStream(Catalyst.Editor.CatalystEditor.Instance.GraphicsDevice, fs));
            }

            using (FileStream fs = new FileStream("Icons/MonoEntity_16x.png", FileMode.Open))
            {
                MonoEntity = Catalyst.Editor.CatalystEditor.Instance.Renderer.BindTexture(Texture2D.FromStream(Catalyst.Editor.CatalystEditor.Instance.GraphicsDevice, fs));
            }

            using (FileStream fs = new FileStream("Icons/Visible_16x.png", FileMode.Open))
            {
                Visible = Catalyst.Editor.CatalystEditor.Instance.Renderer.BindTexture(Texture2D.FromStream(Catalyst.Editor.CatalystEditor.Instance.GraphicsDevice, fs));
            }

            using (FileStream fs = new FileStream("Icons/NotVisible_16x.png", FileMode.Open))
            {
                NotVisible = Catalyst.Editor.CatalystEditor.Instance.Renderer.BindTexture(Texture2D.FromStream(Catalyst.Editor.CatalystEditor.Instance.GraphicsDevice, fs));
            }

            using (FileStream fs = new FileStream("Icons/FolderOpened_white_16x.png", FileMode.Open))
            {
                Folder = Catalyst.Editor.CatalystEditor.Instance.Renderer.BindTexture(Texture2D.FromStream(Catalyst.Editor.CatalystEditor.Instance.GraphicsDevice, fs));
            }
        }
    }
}
