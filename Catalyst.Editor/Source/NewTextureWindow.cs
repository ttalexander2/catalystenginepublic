using Catalyst.Editor;
using Catalyst.Engine.Utilities;
using CatalystEditor.Widgets;
using FMOD;
using ImGuiNET;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CatalystEditor
{
    public static class NewTextureWindow
    {

        public static bool WindowOpen = false;
        public static bool SelectingFile = false;
        private static bool _bindTexture = false;

        public static FileBrowser fileBrowser;

        public static readonly string PopupId = "Add textures to atlas...##New Textures Wizard";

        public static string[] FilesToAdd;

        private static List<Texture2D> _textures = new List<Texture2D>();
        private static Dictionary<string, IntPtr> _pointers = new Dictionary<string, IntPtr>();

        private static int _mode = 0;
        private static string _name = "";
        private static int _atlas = 0;
        private static string _atlasName = "";

        public static void RenderWindow(float width)
        {
            ImGuiWindowFlags window_flags = 0;
            window_flags |= ImGuiWindowFlags.NoCollapse;
            window_flags |= ImGuiWindowFlags.Modal;
            window_flags |= ImGuiWindowFlags.NoResize;
            window_flags |= ImGuiWindowFlags.NoDecoration;

            ImGui.SetNextWindowSize(new System.Numerics.Vector2(width, 500));

            if (ImGui.BeginPopupModal(PopupId, ref WindowOpen, window_flags))
            {
                unsafe
                {
                    ImGui.PushStyleColor(ImGuiCol.ChildBg, *ImGui.GetStyleColorVec4(ImGuiCol.FrameBg));
                }

                if (!_bindTexture)
                {
                    _mode = 0;
                    foreach (string s in FilesToAdd)
                    {
                        using (FileStream f = new FileStream(s, FileMode.Open))
                        {
                            Texture2D t = Texture2D.FromStream(Catalyst.Editor.CatalystEditor.Instance.GraphicsDevice, f);
                            _textures.Add(t);
                            _pointers[s] = Catalyst.Editor.CatalystEditor.Instance.Renderer.BindTexture(t);
                        }
                    }

                    _bindTexture = true;
                    _atlas = 0;
                    _name = "";
                    _atlasName = "";


                }
                
                if (ImGui.BeginChild("##Drag and Drop Image Preview", System.Numerics.Vector2.UnitY*175f, true, ImGuiWindowFlags.AlwaysHorizontalScrollbar))
                {
                    bool[] selected = new bool[FilesToAdd.Length];

                    float cursorPos = 10;

                    for (int n = 0; n < FilesToAdd.Length; n++)
                    {
                        string item = FilesToAdd[n];
                        Texture2D texture = _textures[n];

                        System.Numerics.Vector2 size = new System.Numerics.Vector2(150f * ((float)_textures[n].Width / (float)_textures[n].Height), 150f);

                        ImGui.Image(_pointers[FilesToAdd[n]], size);
                        
                        ImGui.SameLine();
                        ImGui.SetCursorPosX(cursorPos);

                        ImGui.Selectable(Path.GetFileName(FilesToAdd[n]), selected[n], ImGuiSelectableFlags.None, size);
                        if (ImGui.IsItemHovered())
                        {
                            ImGui.SetTooltip("Drag images to reorder");
                        }
                        cursorPos += 150f * ((float)_textures[n].Width / (float)_textures[n].Height);



                        if (ImGui.IsItemActive() && !ImGui.IsItemHovered())
                        {
                            int n_next = n + (ImGui.GetMouseDragDelta(0).X < 0 ? -1 : 1);
                            if (n_next >= 0 && n_next < FilesToAdd.Length)
                            {
                                FilesToAdd[n] = FilesToAdd[n_next];
                                FilesToAdd[n_next] = item;

                                _textures[n] = _textures[n_next];
                                _textures[n_next] = texture;

                                ImGui.ResetMouseDragDelta();
                            }
                        }

                        ImGui.SameLine();
                    }

                    ImGui.EndChild();
                    ImGui.NewLine();

                }

                ImGui.PopStyleColor();



                string[] items = { "Add To Atlas", "Add to Atlas as Animated Texture", "Add as individual textures (not reccomended)" };
                ImGui.Text("Texture Mode: ");
                ImGui.SameLine(150f);

                ImGui.SetNextItemWidth(250f);
                ImGui.Combo("##Texture add method", ref _mode, items, items.Length);

                if (ImGui.IsItemHovered()) ImGui.SetTooltip("Select how to add textures");

                if (_mode == 1)
                {

                    ImGui.Text("Texture name: ");
                    ImGui.SameLine(150f);
                    ImGui.SetNextItemWidth(250f);
                    ImGui.InputText("##animated_texture_name", ref _name, 128);
                    _name = ProjectManager.RemoveInvalidChars(_name);
                    if (string.IsNullOrWhiteSpace(_name))
                    {
                        ImGui.SameLine();
                        ImGui.PushStyleColor(ImGuiCol.Text, System.Numerics.Vector4.UnitX + System.Numerics.Vector4.UnitW);
                        ImGui.Text("Please give this animated texture a name.");
                        ImGui.PopStyleColor();
                    }

                }

                if (_mode != 2)
                {
                    string[] atlases = { "(Add a new Atlas)" };
                    ImGui.Text("Select Atlas: ");
                    ImGui.SameLine(150f);
                    ImGui.SetNextItemWidth(250f);
                    ImGui.Combo("##atlas_add_mode", ref _atlas, atlases, atlases.Length);

                    if (_atlas == 0)
                    {
                        ImGui.Text("Atlas Name: ");
                        ImGui.SameLine(150f);
                        ImGui.SetNextItemWidth(250f);
                        ImGui.InputText("##atlas_name", ref _atlasName, 128);
                        _atlasName = ProjectManager.RemoveInvalidChars(_atlasName);
                        if (string.IsNullOrWhiteSpace(_atlasName))
                        {
                            ImGui.SameLine();
                            ImGui.PushStyleColor(ImGuiCol.Text, System.Numerics.Vector4.UnitX + System.Numerics.Vector4.UnitW);
                            ImGui.Text("Please give the atlas a name.");
                            ImGui.PopStyleColor();
                        }
                    }
                }

                if (ImGui.Button("Add"))
                {
                    if (_mode == 2 || (_mode == 0 && !string.IsNullOrWhiteSpace(_atlasName)) || (_mode == 1 && !string.IsNullOrWhiteSpace(_atlasName) && !string.IsNullOrWhiteSpace(_name)))
                    {
                        AddTextures(_mode, FilesToAdd, _atlasName, _name);
                        WindowOpen = false;
                    }
                }
                ImGui.SameLine();

                if (ImGui.Button("Cancel##Closing New Texture Window"))
                {
                    WindowOpen = false;
                }
                ImGui.EndPopup();
            }


        }

        public static void RemoveTextures()
        {
            if (!WindowOpen && _bindTexture)
            {
                foreach (IntPtr p in _pointers.Values)
                {
                    Catalyst.Editor.CatalystEditor.Instance.Renderer.UnbindTexture(p);
                }
                _pointers.Clear();
                foreach (Texture2D t in _textures)
                {
                    t.Dispose();
                }
                _textures.Clear();
                _bindTexture = false;

                Console.WriteLine("Removed Textures");
            }

        }


        private static void AddTextures(int mode, string[] files, string atlas, string name)
        {
            switch (mode)
            {
                case 1:         //Add to atlas as animated texture
                {
                    Directory.CreateDirectory(Path.Combine(ProjectManager.BuildTexturePath, atlas));
                    Directory.CreateDirectory(Path.Combine(ProjectManager.BuildTexturePath, atlas, name));
                    for (int i = 0; i < files.Length; i++)
                    {
                        File.Copy(files[i], Path.Combine(ProjectManager.BuildTexturePath, atlas, name, $"{name}_{i}.png"));
                    }
                    break;
                }
                case 2:         //Add as individual texture
                {
                    foreach (string f in files)
                    {
                        File.Copy(f, Path.Combine(ProjectManager.BuildTexturePath, Path.GetFileName(f)));
                    }
                    break;
                }
                default:        //Add to atlas
                {
                    Directory.CreateDirectory(Path.Combine(ProjectManager.BuildTexturePath, atlas));
                    for (int i = 0; i < files.Length; i++)
                    {
                        File.Copy(files[i], Path.Combine(ProjectManager.BuildTexturePath, atlas, Path.GetFileName(files[i])));
                    }
                    break;
                }
            }

            Task.Run(() => { ProjectManager.BuildAtlases(false); });
        }
    }
}
