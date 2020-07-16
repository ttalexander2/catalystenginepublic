using System;
using System.Text;
using ImGuiNET;
using System.IO;
using CatalystEditor;
using Microsoft.Xna.Framework;
using Vector2 = System.Numerics.Vector2;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Catalyst.Game.Source;
using CatalystEditor.Widgets;
using System.Drawing;
using FMOD;
using System.Linq;
using Catalyst.Engine.Utilities;
using CatalystEditor.Source;

namespace Catalyst.Editor
{
    public class ImGuiLayout
    {

        private bool show_test_window = false;
        private bool new_project_window = false;
        private bool file_picker = false;
        private bool log_window = true;

        public IntPtr ImGuiTexture;

        public static ImFontPtr DefaultFont;
        public static ImFontPtr HeadingFont;
        public static ImFontPtr SubHeadingFont;
        public static ImFontPtr SlightlyLargerFontThanNormal;

        private Vector2 _windowSize;

        public Vector2 ViewBounds = Vector2.Zero;
        public Rectangle ViewRect = Rectangle.Empty;


        private FileBrowser fileBrowser;
        public void Initialize()
        {
            ImGui.GetIO().Fonts.Clear();
            DefaultFont = ImGui.GetIO().Fonts.AddFontFromFileTTF("Fonts/segoeui.ttf", 16.0f);
            HeadingFont = ImGui.GetIO().Fonts.AddFontFromFileTTF("Fonts/segoeuib.ttf", 22.0f);
            SubHeadingFont = ImGui.GetIO().Fonts.AddFontFromFileTTF("Fonts/segoeui.ttf", 20.0f);
            SlightlyLargerFontThanNormal = ImGui.GetIO().Fonts.AddFontFromFileTTF("Fonts/segoeui.ttf", 18.0f);
            ImGui.GetIO().ConfigWindowsResizeFromEdges = true;
            ImGuiBackendFlags f = 0;
            f |= ImGuiBackendFlags.HasMouseCursors;
            ImGui.GetIO().BackendFlags = f;
            StyleManager.LoadDark();
            ImGui.GetIO().ConfigFlags |= ImGuiConfigFlags.DockingEnable;
            ImGui.GetIO().ConfigWindowsMoveFromTitleBarOnly = true;
            ImGui.GetIO().ConfigDockingAlwaysTabBar = true;
            LogWindow.Init();
        }

        public void PushStyle()
        {
            ImGui.PushFont(DefaultFont); //Set default font
            ImGui.PushStyleVar(ImGuiStyleVar.WindowRounding, 0);
            ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, 1);
            ImGui.PushStyleVar(ImGuiStyleVar.ScrollbarRounding, 0);
            ImGui.PushStyleColor(ImGuiCol.ResizeGrip, 0);
            ImGui.PushStyleColor(ImGuiCol.ResizeGripHovered, 0);
            ImGui.PushStyleColor(ImGuiCol.ResizeGripActive, 0);
        }

        public void PopStyle()
        {
            ImGui.PopFont();
            ImGui.PopStyleVar(3);
            ImGui.PopStyleColor(3);
        }



        public void Render(GameTime gameTime)
        {
            PushStyle();

            _windowSize = new Vector2(CatalystEditor.Instance.GraphicsDevice.Viewport.Width, CatalystEditor.Instance.GraphicsDevice.Viewport.Height + 1);


            RenderMenuBar();


            //Left Bar Window
            if (ProjectManager.scene_loaded)
            {
                ImGuiWindowFlags window_flags = 0;
                window_flags |= ImGuiWindowFlags.NoCollapse;
                window_flags |= ImGuiWindowFlags.NoResize;
                window_flags |= ImGuiWindowFlags.NoBringToFrontOnFocus;
                window_flags |= ImGuiWindowFlags.NoMove;
                window_flags |= ImGuiWindowFlags.NoTitleBar;
                window_flags |= ImGuiWindowFlags.DockNodeHost;

                uint dock_id = 0;

                bool active = true;
                ImGui.SetNextWindowSize(_windowSize - Vector2.UnitY * 20f);
                ImGui.SetNextWindowPos(Vector2.UnitY * 20f);
                ImGui.Begin("Central Dockspace", ref active, window_flags);

                if (active)
                {
                    dock_id = ImGui.GetID("HUB_DockSpace");
                    ImGui.DockSpace(dock_id, Vector2.Zero, ImGuiDockNodeFlags.NoResize & ImGuiDockNodeFlags.PassthruCentralNode);
                }

                ImGui.End();

                window_flags = 0;
                window_flags |= ImGuiWindowFlags.NoCollapse;

                if (SceneWindows.HierarchyWindow)
                {
                    ImGui.SetNextWindowDockID(dock_id, ImGuiCond.FirstUseEver);
                    if (ImGui.Begin("Hierarchy", ref SceneWindows.HierarchyWindow, window_flags))
                    {
                        SceneWindows.RenderHierarchyWindow();
                    }
                    ImGui.End();
                }

                if (SceneWindows.InspectorWindowOpen)
                {
                    ImGui.SetNextWindowDockID(dock_id, ImGuiCond.FirstUseEver);
                    if (ImGui.Begin("Inspector", ref SceneWindows.InspectorWindowOpen, window_flags))
                    {
                        SceneWindows.RenderInspector();
                    }
                    ImGui.End();
                }


                if (LogWindow.WindowOpen)
                {
                    ImGui.SetNextWindowDockID(dock_id, ImGuiCond.FirstUseEver);
                    if (ImGui.Begin("Log", ref SceneWindows.InspectorWindowOpen, window_flags))
                    {
                        LogWindow.Render();
                    }
                    ImGui.End();
                }


                if (Viewport.ViewportWindowOpen)
                {
                    ImGuiWindowFlags window_flags2 = 0;
                    window_flags2 |= ImGuiWindowFlags.NoCollapse;
                    window_flags2 |= ImGuiWindowFlags.NoScrollbar;
                    if (Viewport.Playing)
                        window_flags2 |= ImGuiWindowFlags.UnsavedDocument;
                    ImGui.PushStyleColor(ImGuiCol.WindowBg, new System.Numerics.Vector4(0.11f, 0.11f, 0.11f, 1.00f));
                    ImGui.SetNextWindowDockID(dock_id, ImGuiCond.FirstUseEver);
                    if (ImGui.Begin("Game Viewport", ref Viewport.ViewportWindowOpen, window_flags2))
                    {
                        Viewport.Render(gameTime);
                    }
                    ImGui.End();

                    ImGui.PopStyleColor();
                }

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
            if (file_picker)
            {
                bool result = fileBrowser.OpenModalPopup();
                if (result)
                {
                    file_picker = false;
                    if (fileBrowser.Result != FileBrowser.FileBrowserResult.Canceled)
                        ProjectManager.Open(fileBrowser.SelectedFile);
                }
                ImGui.OpenPopup(fileBrowser.PopupId);
            }


            PopStyle();
        }

        private void RenderMenuBar()
        {
            ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, 0);


            if (ImGui.BeginMainMenuBar())
            {
                if (ImGui.BeginMenu("File"))
                {
                    if (!LoadingProject && ImGui.MenuItem("New"))
                    {
                        new_project_window = true;
                    }
                    if (ImGui.MenuItem("Open", "Ctrl+O")) {
                        fileBrowser = new FileBrowser(Environment.GetFolderPath(Environment.SpecialFolder.Personal), false, false, ProjectManager.Extension);
                        file_picker = true;
                    }
                    if (ImGui.MenuItem("Save", "Ctrl+S", false, ProjectManager.scene_loaded))
                    {
                        ProjectManager.Save();
                    }
                    if (ImGui.MenuItem("Save As..", ProjectManager.scene_loaded))
                    {

                    }
                    ImGui.Separator();

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

                if (ImGui.BeginMenu("Objects"))
                {
                    bool available = ProjectManager.Current != null;
                    if (ImGui.MenuItem("New Entity...", available))
                    {
                        ProjectManager.Current.Manager.NewEntity();
                    }
                    if (ImGui.MenuItem("New Player", available))
                    {
                        _ = new Player(ProjectManager.Current);
                    }
                    if (ImGui.MenuItem("New Camera...", available))
                    {
                        ProjectManager.Current.NewCamera();
                    }
                    ImGui.EndMenu();
                }

                if (ImGui.BeginMenu("Window"))
                {
                    if (ImGui.MenuItem("Hierarchy"))
                    {
                        SceneWindows.HierarchyWindow = true;
                    }

                    if (ImGui.MenuItem("Inspector"))
                    {
                        SceneWindows.InspectorWindowOpen = true;
                    }

                    if (ImGui.MenuItem("Game Viewport"))
                    {
                        Viewport.ViewportWindowOpen = true;
                    }
                    ImGui.EndMenu();
                }

                if (Viewport.Playing)
                {
                    ImGui.PushStyleColor(ImGuiCol.Text, System.Numerics.Vector4.UnitX + System.Numerics.Vector4.UnitW);
                    ImGui.SameLine(ImGui.GetWindowWidth() - 320);
                    ImGui.Text("Game is running. Changes to the scene will not be saved.");
                    ImGui.PopStyleColor();
                }


                ImGui.PopStyleVar();
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
                ImGui.PopFont();

                ImGui.Text("Enter a name: ");

                ImGui.InputText("", buff, 40);
                string str = Encoding.Default.GetString(buff);

                if (!customDir)
                {
                    ImGui.Text(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), ProjectManager.RemoveInvalidChars(str)));
                } else
                {
                    ImGui.Text(projectDir + "\\" + str);
                }


                if (ImGui.Button("Choose Project Directory"))
                {
                    //TODO: MAKE THIS CROSS PLATFORM
                    /**
                    System.Windows.Forms.FolderBrowserDialog openFileDialog1 = new System.Windows.Forms.FolderBrowserDialog();
                    openFileDialog1.RootFolder = Environment.SpecialFolder.UserProfile;
                    if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        projectDir = openFileDialog1.SelectedPath;
                        customDir = true;
                    }
                    */
                }

                ImGui.Text("");

                ImGui.AlignTextToFramePadding();

                if (ImGui.Button("Next"))
                {
                    ImGui.CloseCurrentPopup();

                    if (customDir)
                    {
                        ProjectManager.ProjectPath = Path.Combine(projectDir, ProjectManager.RemoveInvalidChars(str));
                        ProjectManager.CreateNew(ProjectManager.RemoveInvalidChars(str));
                    } else
                    {
                        ProjectManager.ProjectPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), ProjectManager.RemoveInvalidChars(str));
                        ProjectManager.CreateNew(ProjectManager.RemoveInvalidChars(str));
                    }

                    new_project_window = false;
                    LoadingProject = true;
                    buff = new byte[40];

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
    }
}
