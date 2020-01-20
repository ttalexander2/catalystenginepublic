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
using Chroma.Engine;
using Chroma.Game;

namespace Catalyst
{
    public class ImGuiLayout
    {
        private float f = 0.0f;

        private bool show_test_window = false;
        private bool world_loaded = false;
        private bool new_project_window = false;
        //public System.Numerics.Vector3 clear_color = new System.Numerics.Vector3(114f / 255f, 144f / 255f, 154f / 255f);
        public IntPtr ImGuiTexture;
        private byte[] _textBuffer = new byte[100];
        private string _currentScene = "";

        private ImFontPtr _defaultFont;
        private ImFontPtr _headingFont;

        private Vector2 _windowSize;
        private Vector2 _menuSize;

        public void Initialize()
        {
            _defaultFont = ImGui.GetIO().Fonts.AddFontFromFileTTF("Fonts/segoeui.ttf", 18.0f);
            _headingFont = ImGui.GetIO().Fonts.AddFontFromFileTTF("Fonts/segoeuib.ttf", 26.0f);
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
            ImGui.PushFont(_defaultFont); //Set default font
            ImGui.PushStyleVar(ImGuiStyleVar.WindowRounding, 0);
            ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, 1);
            ImGui.PushStyleVar(ImGuiStyleVar.ScrollbarRounding, 0);
            ImGui.PushStyleColor(ImGuiCol.ResizeGrip, 0);
            ImGui.PushStyleColor(ImGuiCol.ResizeGripHovered, 0);
            ImGui.PushStyleColor(ImGuiCol.ResizeGripActive, 0);
        }



        public void Render()
        {

            _windowSize = new Vector2(SampleGame.Instance.GraphicsDevice.Viewport.Width, SampleGame.Instance.GraphicsDevice.Viewport.Height+1);

            SetStyle();
            RenderMenuBar();

            //Left Bar Window
            if (world_loaded){
                ImGuiWindowFlags window_flags = 0;
                window_flags |= ImGuiWindowFlags.NoTitleBar;
                window_flags |= ImGuiWindowFlags.NoMove;
                window_flags |= ImGuiWindowFlags.NoCollapse;
                window_flags |= ImGuiWindowFlags.NoBringToFrontOnFocus;

                ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, 0);
                ImGui.SetNextWindowSizeConstraints(new Vector2(50, _windowSize.Y - _menuSize.Y), new Vector2(_windowSize.X, _windowSize.Y - _menuSize.Y));

                bool t = false;
                if (ImGui.Begin("Scene Window", ref t, window_flags))
                {
                    ImGui.SetWindowPos(new Vector2(0, _menuSize.Y));

                    ImGui.SetWindowCollapsed(false);



                    ImGui.End();
                }

                ImGui.SetNextWindowSizeConstraints(new Vector2(50, _windowSize.Y - _menuSize.Y), new Vector2(_windowSize.X, _windowSize.Y - _menuSize.Y));
                t = false;
                if (ImGui.Begin("Entity Window", ref t, window_flags))
                {
                    ImGui.SetWindowPos(new Vector2(_windowSize.X-ImGui.GetWindowSize().X, _menuSize.Y));

                    ImGui.SetWindowCollapsed(false);

                    EntityWindow.RenderEntityWindow();

                    ImGui.End();
                }
                
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
                ImGui.OpenPopup("New Project");
                NewProjectWindow();
            }


        }

        private void RenderMenuBar()
        {
            ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, 0);
            ImGui.BeginMainMenuBar();


            if (ImGui.BeginMainMenuBar())
            {
                _menuSize = ImGui.GetWindowSize();
                if (ImGui.BeginMenu("File"))
                {
                    if (ImGui.MenuItem("New"))
                    {
                        new_project_window = true;
                    }
                    if (ImGui.MenuItem("Open", "Ctrl+O")) {
                        System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
                        openFileDialog1.Filter = "Chroma Scene File (*.chroma)|*" +ProjectManager.Extension;
                        openFileDialog1.DefaultExt = ProjectManager.Extension;
                        openFileDialog1.AddExtension = true;
                        openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                        if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            ProjectManager.Open(openFileDialog1.FileName);
                        }
                    }
                    if (ImGui.BeginMenu("Open Recent"))
                    {
                        ImGui.MenuItem("fish_hat.c");
                        ImGui.MenuItem("fish_hat.inl");
                        ImGui.MenuItem("fish_hat.h");
                        ImGui.EndMenu();
                    }
                    if (ImGui.MenuItem("Save", "Ctrl+S", false, world_loaded))
                    {
                        ProjectManager.Save();
                    }
                    if (ImGui.MenuItem("Save As..", world_loaded))
                    {

                    }
                    ImGui.Separator();

                    if(ImGui.MenuItem("Load Test Scene", true))
                    {
                        ProjectManager.Current = ProjectManager.LoadTestWorld();
                    }

                    if (ImGui.BeginMenu("Options"))
                    {
                        bool enabled = true;
                        ImGui.MenuItem("Enabled", "", enabled);
                        ImGui.BeginChild("child", new Vector2(0, 60), true);
                        for (int i = 0; i < 10; i++)
                            ImGui.Text("Scrolling Text " + i);
                        ImGui.EndChild();
                        float f = 0.5f;
                        int n = 0;
                        bool b = true;
                        ImGui.SliderFloat("Value", ref f, 0.0f, 1.0f);
                        ImGui.InputFloat("Input", ref f, 0.1f);
                        ImGui.Combo("Combo", ref n, "Yes\0No\0Maybe\0\0");
                        ImGui.Checkbox("Check", ref b);
                        ImGui.EndMenu();
                    }

                    if (ImGui.MenuItem("Quit", "Alt+F4")) { SampleGame.Instance.Exit(); }
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

                if (ImGui.Button("Test Window")) show_test_window = !show_test_window;
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
                
                ImGui.PushFont(_headingFont);
                ImGui.Text("Create a new project");
                ImGui.PushFont(_defaultFont);

                ImGui.Text("Enter a name: ");

                ImGui.InputText("", buff, 40);
                string str = Encoding.Default.GetString(buff);

                if (!customDir)
                {
                    ImGui.Text(Path.Combine(Directory.GetCurrentDirectory(),"Projects"));
                } else
                {
                    ImGui.Text(Path.Combine(projectDir, str));
                }
                

                if(ImGui.Button("Choose Project Directory"))
                {
                    System.Windows.Forms.FolderBrowserDialog openFileDialog1 = new System.Windows.Forms.FolderBrowserDialog();
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
                        ProjectManager.ProjectPath = Path.Combine(projectDir, str);
                        ProjectManager.CreateNew(str);
                    } else
                    {
                        ProjectManager.ProjectPath = Path.Combine(Directory.GetCurrentDirectory(),"Projects");
                        ProjectManager.CreateNew(str);
                    }

                    new_project_window = false;
                    world_loaded = true;
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
    }
}
