using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using ImGuiNET;
using Catalyst.XNA;
using Microsoft.Xna.Framework.Input;
using System.Drawing;
using System.IO;
using Catalyst.Engine;
using System.Reflection;
using CatalystEditor;
using Microsoft.Xna.Framework;
using Vector2 = System.Numerics.Vector2;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Catalyst.XNA
{
    public class ImGuiLayout
    {
        private float f = 0.0f;

        private bool show_test_window = false;
        private bool new_project_window = false;
        //public System.Numerics.Vector3 clear_color = new System.Numerics.Vector3(114f / 255f, 144f / 255f, 154f / 255f);
        public IntPtr ImGuiTexture;
        private byte[] _textBuffer = new byte[100];
        private string _currentScene = "";

        public static ImFontPtr DefaultFont;
        public static ImFontPtr HeadingFont;
        public static ImFontPtr SubHeadingFont;
        public static ImFontPtr SlightlyLargerFontThanNormal;

        private Vector2 _windowSize;
        private Vector2 _menuSize;

        public Vector2 ViewBounds = Vector2.Zero;
        public Rectangle ViewRect = Rectangle.Empty;

        float scene_size = 0;
        float right_dock_size = 0;
        float view_size = 0;

        public void Initialize()
        {
            DefaultFont = ImGui.GetIO().Fonts.AddFontFromFileTTF("Fonts/segoeui.ttf", 18.0f);
            HeadingFont = ImGui.GetIO().Fonts.AddFontFromFileTTF("Fonts/segoeuib.ttf", 26.0f);
            SubHeadingFont = ImGui.GetIO().Fonts.AddFontFromFileTTF("Fonts/segoeui.ttf", 22.0f);
            SlightlyLargerFontThanNormal = ImGui.GetIO().Fonts.AddFontFromFileTTF("Fonts/segoeui.ttf", 20.0f);
            ImGui.GetIO().ConfigWindowsResizeFromEdges = true;
            ImGuiBackendFlags f = 0;
            f |= ImGuiBackendFlags.HasMouseCursors;
            ImGui.GetIO().BackendFlags = f;
            StyleManager.LoadDark();
            //ProjectManager.Current = ProjectManager.LoadTestWorld();
            //world_loaded = true;
        }

        public void SetStyle()
        {
            ImGui.PushFont(DefaultFont); //Set default font
            ImGui.PushStyleVar(ImGuiStyleVar.WindowRounding, 0);
            ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, 1);
            ImGui.PushStyleVar(ImGuiStyleVar.ScrollbarRounding, 0);
            ImGui.PushStyleColor(ImGuiCol.ResizeGrip, 0);
            ImGui.PushStyleColor(ImGuiCol.ResizeGripHovered, 0);
            ImGui.PushStyleColor(ImGuiCol.ResizeGripActive, 0);
        }



        public void Render(GameTime gameTime)
        {

            _windowSize = new Vector2(CatalystEditor.Instance.GraphicsDevice.Viewport.Width, CatalystEditor.Instance.GraphicsDevice.Viewport.Height+1);

            SetStyle();
            RenderMenuBar();

            //Left Bar Window
            if (ProjectManager.scene_loaded)
            {
                ImGuiWindowFlags window_flags = 0;
                window_flags |= ImGuiWindowFlags.NoMove;
                window_flags |= ImGuiWindowFlags.NoCollapse;
                window_flags |= ImGuiWindowFlags.NoBringToFrontOnFocus;
                window_flags |= ImGuiWindowFlags.NoTitleBar;

                ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, 0);
                ImGui.SetNextWindowSizeConstraints(new Vector2(200, _windowSize.Y - _menuSize.Y), new Vector2(_windowSize.X - right_dock_size - 100, _windowSize.Y - _menuSize.Y));

                bool t = false;
                if (ImGui.Begin("Scene Window", ref t, window_flags))
                {
                    ImGui.SetWindowPos(new Vector2(0, _menuSize.Y));
                    scene_size = ImGui.GetWindowSize().X;

                    ImGui.SetWindowCollapsed(false);



                    ImGui.End();
                }

                ImGui.SetNextWindowSizeConstraints(new Vector2(200, _windowSize.Y - _menuSize.Y), new Vector2(_windowSize.X - scene_size - 100, _windowSize.Y - _menuSize.Y));

                t = false;
                if (ImGui.Begin("Right Dock", ref t, window_flags))
                {
                    right_dock_size = ImGui.GetWindowSize().X;
                    ImGui.SetWindowPos(new Vector2(_windowSize.X-right_dock_size, _menuSize.Y));


                    ImGui.SetWindowCollapsed(false);

                    RightDock.RenderRightDock();

                    ImGui.End();
                }

                ImGuiWindowFlags view_flags = 0;
                view_flags |= ImGuiWindowFlags.NoTitleBar;
                view_flags |= ImGuiWindowFlags.NoMove;
                view_flags |= ImGuiWindowFlags.NoCollapse;
                view_flags |= ImGuiWindowFlags.NoBringToFrontOnFocus;

                bool oof = false;

                ImGui.PushStyleColor(ImGuiCol.WindowBg, new System.Numerics.Vector4(0.11f, 0.11f, 0.11f, 1.00f));

                if (ImGui.Begin("Game Viewport", ref oof, view_flags))
                {
                    ImGui.SetWindowPos(new Vector2(scene_size, _menuSize.Y));
                    ImGui.SetWindowSize(new Vector2(_windowSize.X - scene_size-right_dock_size, _windowSize.Y - _menuSize.Y));

                    view_size = ImGui.GetWindowSize().X;

                    ImGui.SetWindowCollapsed(false);

                    ViewBounds = CalculateViewBounds((int)(_windowSize.X - scene_size - right_dock_size), (int)(_windowSize.Y - _menuSize.Y-50));

                    ViewRect = new Rectangle((int)scene_size, (int)_menuSize.Y, (int)ViewBounds.X, (int)ViewBounds.Y);

                    ViewportRenderer.RenderViewPort(gameTime, ViewBounds, ViewRect);

                    ImGui.End();
                }

                ImGui.PushStyleColor(ImGuiCol.WindowBg, new System.Numerics.Vector4(0.23f, 0.23f, 0.25f, 1.00f));

                
                ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, 1);
            }

            // 3. Show the ImGui test window. Most of the sample code is in ImGui.ShowTestWindow()
            if (show_test_window)
            {
                ImGui.SetNextWindowPos(new System.Numerics.Vector2(650, 20), ImGuiCond.FirstUseEver);
                ImGui.ShowDemoWindow(ref show_test_window);
            }

            if (new_project_window)
            {
                ImGui.OpenPopup("New Scene");
                NewProjectWindow();
            }


        }

        private void RenderMenuBar()
        {
            ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, 0);


            if (ImGui.BeginMainMenuBar())
            {
                _menuSize = ImGui.GetWindowSize();
                if (ImGui.BeginMenu("File"))
                {
                    if (!LoadingProject && ImGui.MenuItem("New"))
                    {
                        new_project_window = true;
                    }
                    if (ImGui.MenuItem("Open", "Ctrl+O")) {
                        System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
                        openFileDialog1.Filter = "Catalyst Scene File (*.chroma)|*" +ProjectManager.Extension;
                        openFileDialog1.DefaultExt = ProjectManager.Extension;
                        openFileDialog1.AddExtension = true;
                        openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                        if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            ProjectManager.Open(openFileDialog1.FileName);
                        }
                    }
                    if (ImGui.MenuItem("Save", "Ctrl+S", false, ProjectManager.scene_loaded))
                    {
                        ProjectManager.Save();
                    }
                    if (ImGui.MenuItem("Save As..", ProjectManager.scene_loaded))
                    {

                    }
                    ImGui.Separator();

                    if(ImGui.MenuItem("Load Test Scene", true))
                    {
                        ProjectManager.Current = ProjectManager.LoadTestWorld();
                    }

                    if (ImGui.MenuItem("Test Window")) show_test_window = !show_test_window;

                    if (ImGui.MenuItem("Quit", "Alt+F4"))
                    { 
                        CatalystEditor.Instance.Exit();
                    }
                    ImGui.EndMenu();
                }
                if (ImGui.BeginMenu("Edit"))
                {
                    if (ImGui.MenuItem("Undo", "CTRL+Z")) { }
                    if (ImGui.MenuItem("Redo", "CTRL+Y", false, false)) { }  // Disabled item
                    ImGui.Separator();
                    if (ImGui.MenuItem("Cut", "CTRL+X")) { }
                    if (ImGui.MenuItem("Copy", "CTRL+C")) { }
                    if (ImGui.MenuItem("Paste", "CTRL+V")) { }
                    ImGui.EndMenu();
                }

                if (ImGui.BeginMenu("Window"))
                {
                    if (ImGui.MenuItem("Entities"))
                    {
                        RightDock.EntityWindowOpen = true;
                    }

                    if (ImGui.MenuItem("Sprites"))
                    {
                        RightDock.SpriteWindowOpen = true;
                    }
                    ImGui.EndMenu();
                }


                ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, 1);
                ImGui.EndMainMenuBar();
            }


        }

        

        private enum WindowEdge
        {
            Left,
            Right,
            Up,
            Down
        }


        private byte[] buff = new byte[40];
        private string projectDir;
        private bool customDir = false;
        public bool LoadingProject = false;
        private void NewProjectWindow()
        {
            ImGuiWindowFlags window_flags = 0;
            window_flags |= ImGuiWindowFlags.AlwaysAutoResize;
            window_flags |= ImGuiWindowFlags.NoMove;
            window_flags |= ImGuiWindowFlags.NoCollapse;
            window_flags |= ImGuiWindowFlags.NoDecoration;
            window_flags |= ImGuiWindowFlags.NoCollapse;

            bool r = true;

            if (ImGui.BeginPopupModal("New Scene", ref r, window_flags))
            {
                
                ImGui.PushFont(HeadingFont);
                ImGui.Text("Create a new Scene");
                ImGui.PushFont(DefaultFont);

                ImGui.Text("Enter a name: ");

                ImGui.InputText("", buff, 40);
                string str = Encoding.Default.GetString(buff);

                if (!customDir)
                {
                    ImGui.Text(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), RemoveInvalidChars(str)));
                } else
                {
                    ImGui.Text(projectDir + "\\" + str);
                }
                

                if(ImGui.Button("Choose Project Directory"))
                {
                    System.Windows.Forms.FolderBrowserDialog openFileDialog1 = new System.Windows.Forms.FolderBrowserDialog();
                    openFileDialog1.RootFolder = Environment.SpecialFolder.UserProfile;
                    if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        projectDir = openFileDialog1.SelectedPath;
                        customDir = true;
                    }
                }

                ImGui.Text("");

                ImGui.AlignTextToFramePadding();

                if (ImGui.Button("Next"))
                {
                    ImGui.CloseCurrentPopup();
                    if (customDir)
                    {
                        ProjectManager.ProjectPath = Path.Combine(projectDir, RemoveInvalidChars(str));
                        ProjectManager.CreateNew(str);
                    } else
                    {
                        ProjectManager.ProjectPath = Path.Combine(Directory.GetCurrentDirectory(),"Projects");
                        ProjectManager.CreateNew(str);
                    }

                    new_project_window = false;
                    LoadingProject = true;
                    buff = new byte[40];
                    _currentScene = "";
                }
                ImGui.SameLine();


                if (ImGui.Button("Cancel"))
                {
                    ImGui.CloseCurrentPopup();
                    new_project_window = false;
                    buff = new byte[40];
                }

                ImGui.SameLine();

                ImGui.EndPopup();
            }

        }

        public static void HelpMarker(string desc)
        {
            ImGui.TextDisabled("(?)");
            if (ImGui.IsItemHovered())
            {
                ImGui.BeginTooltip();
                ImGui.PushTextWrapPos(ImGui.GetFontSize() * 35.0f);
                ImGui.TextUnformatted(desc);
                ImGui.PopTextWrapPos();
                ImGui.EndTooltip();
            }
        }

        public static string RemoveInvalidChars(string filename)
        {
            return string.Concat(filename.Split(Path.GetInvalidFileNameChars()));
        }

        public Vector2 CalculateViewBounds(int width, int height)
        {
            float ratio = (float)ProjectManager.Current.Width / (float)ProjectManager.Current.Height;
            float actual = (float)width / (float)height;

            Vector2 size;

            if (actual > ratio)
            {
                size = new Vector2((height * (ratio)), height);
            }
            else if (actual < ratio)
            {
                size = new Vector2(width, (int)(width * 1 / ratio));
            }
            else
            {
                size = new Vector2(width, height);
            }

            return size;
        }
    }
}
