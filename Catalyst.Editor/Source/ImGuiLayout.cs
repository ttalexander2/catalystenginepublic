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
using Catalyst.Engine.Input;
using Microsoft.Xna.Framework.Input;
using static CatalystEditor.Source.WindowHandler;
using FMOD.Studio;
using Catalyst.Engine;

namespace Catalyst.Editor
{
    public class ImGuiLayout
    {

        private bool show_test_window = false;
        private bool new_project_window = false;
        private bool file_picker = false;

        private bool custom_decorations;

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
            ImGui.GetIO().ConfigDockingAlwaysTabBar = false;
            LogWindow.Init();
            ImGui.GetStyle().WindowMenuButtonPosition = ImGuiDir.Right;
            ImGui.GetStyle().DisplaySafeAreaPadding = Vector2.One * 6;
            PushStyle();
            custom_decorations = CatalystEditor.Instance.Window.IsBorderless;
        }

        public void PushStyle()
        {
            
            ImGui.PushStyleVar(ImGuiStyleVar.WindowRounding, 3);
            ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, 0);
            ImGui.PushStyleVar(ImGuiStyleVar.ScrollbarRounding, 1);
            ImGui.PushStyleVar(ImGuiStyleVar.FrameRounding, 3);
            ImGui.PushStyleColor(ImGuiCol.ResizeGrip, 0);
            ImGui.PushStyleColor(ImGuiCol.ResizeGripHovered, 0);
            ImGui.PushStyleColor(ImGuiCol.ResizeGripActive, 0);
        }

        public void PopStyle()
        {
            ImGui.PopFont();
            ImGui.PopStyleVar(4);
            ImGui.PopStyleColor(3);
        }



        public void Render(GameTime gameTime)
        {
            ImGui.PushFont(DefaultFont); //Set default font

            _windowSize = new Vector2(CatalystEditor.Instance.GraphicsDevice.Viewport.Width, CatalystEditor.Instance.GraphicsDevice.Viewport.Height);

            RenderMenuBar();


            //Left Bar Window
            if (ProjectManager.ProjectLoaded)
            {
                ImGuiWindowFlags window_flags = 0;
                window_flags |= ImGuiWindowFlags.NoCollapse;
                window_flags |= ImGuiWindowFlags.NoResize;
                window_flags |= ImGuiWindowFlags.NoBringToFrontOnFocus;
                window_flags |= ImGuiWindowFlags.NoMove;
                window_flags |= ImGuiWindowFlags.NoTitleBar;
                window_flags |= ImGuiWindowFlags.DockNodeHost;

                ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, 0);
                unsafe { ImGui.PushStyleColor(ImGuiCol.WindowBg, *ImGui.GetStyleColorVec4(ImGuiCol.FrameBg)); }

                uint dock_id = 0;

                bool active = true;
                ImGui.SetNextWindowSize(_windowSize - Vector2.UnitY * 24f);
                ImGui.SetNextWindowPos(Vector2.UnitY * 24f);
                ImGui.Begin("Central Dockspace", ref active, window_flags);

                if (active)
                {
                    dock_id = ImGui.GetID("HUB_DockSpace");
                    ImGui.DockSpace(dock_id, Vector2.Zero, ImGuiDockNodeFlags.NoResize & ImGuiDockNodeFlags.PassthruCentralNode);
                }

                ImGui.End();

                ImGui.PopStyleVar();
                ImGui.PopStyleColor();

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
                    if (ImGui.Begin("Log", ref LogWindow.WindowOpen, window_flags))
                    {
                        LogWindow.Render();
                    }
                    ImGui.End();
                }

                if (PerformanceWindow.Open)
                {
                    ImGui.SetNextWindowDockID(dock_id, ImGuiCond.FirstUseEver);
                    if (ImGui.Begin("Performance", ref PerformanceWindow.Open, window_flags))
                    {
                        PerformanceWindow.Render();
                    }
                    ImGui.End();
                }

                if (SpriteWindow.Open)
                {
                    ImGui.SetNextWindowDockID(dock_id, ImGuiCond.FirstUseEver);
                    if (ImGui.Begin("Sprites", ref SpriteWindow.Open, window_flags))
                    {
                        SpriteWindow.Render();
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

                if (ProjectExplorer.WindowOpen)
                {
                    if(ImGui.Begin("Project Explorer", ref ProjectExplorer.WindowOpen, window_flags))
                    {
                        ProjectExplorer.RenderWindow();
                    }
                    ImGui.End();
                }

                foreach (TextEditor t in TextEditor.Editors)
                {
                    t.RenderWindow();   
                }

                TextEditor.RemoveClosed();

            }

            // 3. Show the ImGui test window. Most of the sample code is in ImGui.ShowTestWindow()
            if (show_test_window)
            {
                ImGui.SetNextWindowPos(new System.Numerics.Vector2(650, 20), ImGuiCond.FirstUseEver);
                ImGui.ShowDemoWindow(ref show_test_window);
            }

            if (new_project_window)
            {
                ImGui.OpenPopup("New Project");
                NewProjectWindow();
            }
            if (file_picker)
            {
                bool result = fileBrowser.OpenModalPopup();
                if (result)
                {
                    file_picker = false;
                    if (fileBrowser.Result != FileBrowser.FileBrowserResult.Canceled)
                    {
                        ProjectManager.ProjectLoaded = ProjectManager.OpenProject(fileBrowser.SelectedFile);
                    }
                }
                ImGui.OpenPopup(fileBrowser.PopupId);
            }

            //PopStyle();
            ImGui.PopFont();
        }

        private void RenderMenuBar()
        {


            ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, 0);
            ImGui.PushStyleVar(ImGuiStyleVar.FramePadding, new Vector2(8, 8));


            if (ImGui.BeginMainMenuBar())
            {
                //ImGui.SetWindowPos(ImGui.GetStyle().DisplaySafeAreaPadding);
                //ImGui.PushStyleVar(ImGuiStyleVar.);
                ImGui.Image(IconLoader.ProgramIcon, IconLoader.Icon32Size);
                //ImGui.PopStyleVar();
                ImGui.SameLine();

                if (ImGui.BeginMenu("File"))
                {
                    if (!LoadingProject && ImGui.MenuItem("New"))
                    {
                        new_project_window = true;
                    }
                    if (ImGui.MenuItem("Open", "Ctrl+O")) {
                        fileBrowser = new FileBrowser(Environment.GetFolderPath(Environment.SpecialFolder.Personal), false, false, ProjectManager.ProjectExtension);
                        file_picker = true;
                    }
                    if (ImGui.MenuItem("Save", "Ctrl+S", false, ProjectManager.ProjectLoaded))
                    {
                        ProjectManager.SaveLevel();
                    }
                    if (ImGui.MenuItem("Save As..", ProjectManager.ProjectLoaded))
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
                    if (ImGui.MenuItem("Hierarchy", ProjectManager.ProjectLoaded))
                    {
                        SceneWindows.HierarchyWindow = true;
                    }

                    if (ImGui.MenuItem("Inspector", ProjectManager.ProjectLoaded))
                    {
                        SceneWindows.InspectorWindowOpen = true;
                    }

                    if (ImGui.MenuItem("Game Viewport", ProjectManager.ProjectLoaded))
                    {
                        Viewport.ViewportWindowOpen = true;
                    }

                    if (ImGui.MenuItem("Log", ProjectManager.ProjectLoaded))
                    {
                        LogWindow.WindowOpen = true;
                    }

                    if (ImGui.MenuItem("Performance", ProjectManager.ProjectLoaded))
                    {
                        PerformanceWindow.Open = true;
                    }

                    if (ImGui.MenuItem("Sprites", ProjectManager.ProjectLoaded))
                    {
                        SpriteWindow.Open = true;
                    }
                    ImGui.EndMenu();
                }

                if (ImGui.BeginMenu("Options"))
                {
                    if (ImGui.MenuItem("Toggle Window Decoration"))
                    {
                        custom_decorations = !custom_decorations;
                        CatalystEditor.Instance.Window.IsBorderless = !CatalystEditor.Instance.Window.IsBorderless;

                        if (WindowHandler.WindowMaximized(CatalystEditor.Instance.Window.Handle))
                        {
                            WindowHandler.SDL_MaximizeWindow(CatalystEditor.Instance.Window.Handle);
                            SDL_Rect rect;
                            unsafe
                            {
                                WindowHandler.SDL_GetDisplayUsableBounds(WindowHandler.SDL_GetWindowDisplayIndex(CatalystEditor.Instance.Window.Handle), &rect);
                            }
                            WindowHandler.SDL_SetWindowPosition(CatalystEditor.Instance.Window.Handle, rect.x, rect.y);
                            WindowHandler.SDL_SetWindowSize(CatalystEditor.Instance.Window.Handle, rect.w, rect.h);
                        }

                    }
                    ImGui.EndMenu();
                }

                if (custom_decorations)
                    RenderCustomWindowDecoration();

                if (Viewport.Playing)
                {
                    ImGui.PushStyleColor(ImGuiCol.Text, System.Numerics.Vector4.UnitX + System.Numerics.Vector4.UnitW);
                    ImGui.SameLine(ImGui.GetWindowWidth() - 420);
                    ImGui.Text("Game is running. Changes to the scene will not be saved.");
                    ImGui.PopStyleColor();
                }


                ImGui.PopStyleVar(2);
                ImGui.EndMainMenuBar();
            }
        }

        private static void RenderCustomWindowDecoration()
        {
            Vector2 size = ImGui.GetWindowSize();
            unsafe { ImGui.PushStyleColor(ImGuiCol.ChildBg, *ImGui.GetStyleColorVec4(ImGuiCol.MenuBarBg)); }

            bool doubleClick = false;

            ImGuiWindowFlags flags = ImGuiWindowFlags.NoResize;
            flags |= ImGuiWindowFlags.NoMove;
            flags |= ImGuiWindowFlags.NoCollapse;
            flags |= ImGuiWindowFlags.NoDecoration;
            flags |= ImGuiWindowFlags.NoDocking;
            flags |= ImGuiWindowFlags.NoNavFocus;
            flags |= ImGuiWindowFlags.NoFocusOnAppearing;
            flags |= ImGuiWindowFlags.NoBringToFrontOnFocus;
            flags |= ImGuiWindowFlags.NoScrollbar;
            flags |= ImGuiWindowFlags.NoTitleBar;
            flags |= ImGuiWindowFlags.AlwaysAutoResize;

            //ImGui.PushStyleColor(ImGuiCol.ChildBg, System.Numerics.Vector4.One);
            if (ImGui.BeginChild("##drag_region", new Vector2(size.X - 410, size.Y + 15), false, flags))
            {
                if (ImGui.IsWindowHovered() && ImGui.IsMouseDoubleClicked(ImGuiMouseButton.Left))
                {
                    doubleClick = true;
                }

                int x;
                int y;
                int x_global;
                int y_global;
                unsafe
                {
                    WindowHandler.SDL_GetMouseState(&x, &y);
                    WindowHandler.SDL_GetGlobalMouseState(&x_global, &y_global);
                }


                if (ImGui.IsMouseDown(ImGuiMouseButton.Left) && !ImGui.IsMouseDoubleClicked(ImGuiMouseButton.Left) && (ImGui.IsWindowHovered() || ImGui.IsWindowFocused()))
                {
                    WindowHandler.SDL_SetWindowPosition(CatalystEditor.Instance.Window.Handle, x_global - x, y_global - y);
                    WindowHandler.SDL_RaiseWindow(CatalystEditor.Instance.Window.Handle);
                }

            }
            ImGui.EndChild();

            //ImGui.PopStyleColor();
            ImGui.PopStyleColor();

            ImGui.PushStyleColor(ImGuiCol.Button, System.Numerics.Vector4.Zero);
            ImGui.PushStyleColor(ImGuiCol.ButtonHovered, System.Numerics.Vector4.One * 0.4f);
            ImGui.PushStyleVar(ImGuiStyleVar.FrameRounding, 0);

            ImGui.SameLine(ImGui.GetWindowWidth() - 120);
            if (ImGui.ImageButton(IconLoader.Minimize, IconLoader.Icon16Size))
            {
                WindowHandler.SDL_MinimizeWindow(CatalystEditor.Instance.Window.Handle);
            }

            ImGui.SameLine();
            if (WindowHandler.WindowMaximized(CatalystEditor.Instance.Window.Handle))
            {
                if (ImGui.ImageButton(IconLoader.RestoreDown, IconLoader.Icon16Size) || doubleClick)
                {
                    WindowHandler.SDL_RestoreWindow(CatalystEditor.Instance.Window.Handle);
                    doubleClick = false;
                }
            }
            else
            {
                if (ImGui.ImageButton(IconLoader.Maximize, IconLoader.Icon16Size) || doubleClick)
                {
                    WindowHandler.SDL_MaximizeWindow(CatalystEditor.Instance.Window.Handle);
                    SDL_Rect rect;
                    unsafe
                    {
                        WindowHandler.SDL_GetDisplayUsableBounds(WindowHandler.SDL_GetWindowDisplayIndex(CatalystEditor.Instance.Window.Handle), &rect);
                    }
                    WindowHandler.SDL_SetWindowPosition(CatalystEditor.Instance.Window.Handle, rect.x, rect.y);
                    WindowHandler.SDL_SetWindowSize(CatalystEditor.Instance.Window.Handle, rect.w, rect.h);
                    doubleClick = false;
                }
            }

            ImGui.SameLine();
            if (ImGui.ImageButton(IconLoader.Close, IconLoader.Icon16Size))
            {
                if (ProjectManager.ProjectLoaded)
                    ProjectManager.SaveLevel();
                CatalystEditor.Instance.Exit();
            }



            ImGui.PopStyleColor(2);
            ImGui.PopStyleVar();
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

            if (ImGui.BeginPopupModal("New Project", ref r, window_flags))
            {

                ImGui.PushFont(HeadingFont);
                ImGui.Text("Create a new project");
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

                    try
                    {
                        if (customDir)
                        {
                            ProjectManager.CreateNewProject(ProjectManager.RemoveInvalidChars(str), projectDir, false);
                        }
                        else
                        {
                            ProjectManager.CreateNewProject(ProjectManager.RemoveInvalidChars(str), Environment.GetFolderPath(Environment.SpecialFolder.Desktop), true);
                        }
                    }
                    catch (DirectoryNotFoundException e)
                    {
                        Console.WriteLine(e);
                        return;
                    }
                    ImGui.CloseCurrentPopup();

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
