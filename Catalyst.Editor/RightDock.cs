using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Catalyst.Engine;
using CatalystEditor;
using ImGuiNET;
using Microsoft.Xna.Framework.Input;
using static Catalyst.Engine.FileTree<int>;

namespace Catalyst.Editor
{
    public static class RightDock
    {
        public static bool EntityWindowOpen = true;
        public static bool SpriteWindowOpen = true;
        public static bool CameraWindowOpen = true;

        private static bool _group = false;
        private static bool _rename = false;
        private static bool _removeSelected = false;
        private static bool _contextOpen = false;
        private static bool _duplicateSelected = false;
        private static Node _start = null;

        public static void RenderRightDock()
        {



            if (ImGui.BeginTabBar("RightDock"))
            {
                if (ImGui.BeginTabItem("Entity", ref EntityWindowOpen))
                {
                    RenderEntityWindow();
                    ImGui.EndTabItem();
                }
                if (ImGui.BeginTabItem("Sprites", ref SpriteWindowOpen))
                {
                    ImGui.EndTabItem();
                }
                if (ImGui.BeginTabItem("Camera", ref CameraWindowOpen))
                {
                    RenderCameraWindow();
                    ImGui.EndTabItem();
                }

                ImGui.EndTabBar();
            }
        }

        private static void RenderTree(FolderNode root)
        {
            ImGuiTreeNodeFlags base_flags = ImGuiTreeNodeFlags.OpenOnArrow | ImGuiTreeNodeFlags.OpenOnDoubleClick;
            if (ProjectManager.Current.Manager.EntityTree.Selected.Count <= 0)
            {
                _start = null;
            }

            foreach (Node n in root.Values)
            {
                if (!n.Editing)
                {
                    if (n is FolderNode)
                    {
                        ImGuiTreeNodeFlags node_flags = base_flags;
                        if (n.Selected)
                        {
                            node_flags |= ImGuiTreeNodeFlags.Selected;
                        }

                        bool node_open = ImGui.TreeNodeEx(((FolderNode)n).Name, node_flags);

                        if (node_open)
                        {
                            if (ImGui.IsItemClicked())
                            {
                                if (_start != null && CatalystEditor.Instance.keyboardState.IsKeyDown(Keys.LeftShift) || CatalystEditor.Instance.keyboardState.IsKeyDown(Keys.RightShift))
                                {
                                    ProjectManager.Current.Manager.EntityTree.SelectBetween(_start, n);
                                }
                                else if (CatalystEditor.Instance.keyboardState.IsKeyDown(Keys.LeftControl) || CatalystEditor.Instance.keyboardState.IsKeyDown(Keys.RightControl))
                                {
                                    ProjectManager.Current.Manager.EntityTree.AddToSelection(n);
                                }
                                else
                                {
                                    ProjectManager.Current.Manager.EntityTree.Select(n);
                                    _start = n;
                                }
                            }

                            RenderTree((FolderNode)n);
                            ImGui.TreePop();
                        }
                        else
                        {
                            if(n.Selected)
                            {
                                ProjectManager.Current.Manager.EntityTree.Deselect();
                            }
                        }
                    }
                    else
                    {
                        Entity e = ProjectManager.Current.Manager.GetEntity(((FileNode)n).Value);


                        ImGuiTreeNodeFlags node_flags = ImGuiTreeNodeFlags.Leaf | ImGuiTreeNodeFlags.NoTreePushOnOpen; // ImGuiTreeNodeFlags_Bullet

                        if (n.Selected)
                            node_flags |= ImGuiTreeNodeFlags.Selected;

                        _ = ImGui.TreeNodeEx(((FileNode)n).Name, node_flags);

                        if (ImGui.IsItemClicked())
                        {
                            if (_start != null && _start != n && CatalystEditor.Instance.keyboardState.IsKeyDown(Keys.LeftShift) || CatalystEditor.Instance.keyboardState.IsKeyDown(Keys.RightShift))
                            {
                                ProjectManager.Current.Manager.EntityTree.SelectBetween(_start, n);
                            }
                            else if (CatalystEditor.Instance.keyboardState.IsKeyDown(Keys.LeftControl) || CatalystEditor.Instance.keyboardState.IsKeyDown(Keys.RightControl))
                            {
                                ProjectManager.Current.Manager.EntityTree.AddToSelection(n);
                            }
                            else
                            {
                                ProjectManager.Current.Manager.EntityTree.Select(n);
                                _start = n;
                            }
                        }
                    }
                }
                else
                {
                    string buff = n.Name.Trim();
                    ImGui.InputText("", ref buff, 256);
                    ImGui.SetKeyboardFocusHere();
                    ImGui.SetScrollHereY();
                    if (n is FileNode)
                    {
                        ProjectManager.Current.Manager.GetEntity(((FileNode)n).Value).Rename(buff.Trim());
                    }
                    else
                    {
                        n.Name = buff.Trim();
                    }

                }
                if (n.Editing && ImGui.IsKeyDown(ImGui.GetKeyIndex(ImGuiKey.Enter)))
                {
                    bool valid = n.Name.Trim().Length > 0;
                    if (n is FileNode)
                    {
                        Entity e = ProjectManager.Current.Manager.GetEntity(((FileNode)n).Value);

                        foreach (Entity ee in ProjectManager.Current.Manager.GetEntities().Values)
                        {
                            if (ee.Name.Equals(e.Name) && ee.UID != e.UID)
                            {
                                valid = false;
                            }
                        }
                    }

                    if (valid)
                    {
                        n.Editing = false;
                        ProjectManager.Current.Manager.EntityTree.Select(n);
                        _rename = true;
                    }
                }

                if (!ViewportRenderer.Playing && n.Selected && !_contextOpen && ImGui.BeginPopupContextWindow())
                {
                    _contextOpen = true;
                    if (!ProjectManager.Current.Manager.EntityTree.MultiSelection && ImGui.Selectable("Rename"))
                    {
                        n.Editing = true;
                    }
                    if (ImGui.Selectable("Group"))
                    {
                        _group = true;
                    }
                    if (ImGui.Selectable("Remove"))
                    {
                        _removeSelected = true;
                    }
                    if (ImGui.Selectable("Duplicate"))
                    {
                        _duplicateSelected = true;
                    }
                    ImGui.EndPopup();
                }
            }
            _contextOpen = false;
        }
        public static void RenderEntityWindow()
        {
            ImGui.PushFont(ImGuiLayout.SubHeadingFont);
            ImGui.Text("Entities");
            ImGui.PushFont(ImGuiLayout.DefaultFont);
            ImGui.SameLine(ImGui.GetWindowWidth() - 92);
            
            //Add entity button
            if (ImGui.Button("+ Add Entity"))
            {
                ProjectManager.Current.Manager.NewEntity();
            }

            //List of entities
            ImGui.BeginChild("Entity_List", ImGui.GetWindowSize() * new System.Numerics.Vector2(0, 0.2f), true, ImGuiWindowFlags.None);
            if (ProjectManager.Current != null)
            {
                //Render selectable file tree
                RenderTree(ProjectManager.Current.Manager.EntityTree.Root);

                //If the user elected to group the selected items
                if (_group)
                {
                    Console.WriteLine(string.Join(", _", ProjectManager.Current.Manager.EntityTree.Selected));
                    ProjectManager.Current.Manager.EntityTree.GroupSelected();
                    _group = false;
                }

                if (_removeSelected)
                {
                    foreach (int t in ProjectManager.Current.Manager.EntityTree.RemoveSelected())
                    {
                        ProjectManager.Current.Manager.GetEntity(t).DestroyEntity();
                    }

                    ProjectManager.Current.Manager.EntityTree.Deselect();
                    _removeSelected = false;

                }

                if (_duplicateSelected)
                {
                    foreach (Node t in ProjectManager.Current.Manager.EntityTree.Selected)
                    {
                        if (t is FileNode)
                            ProjectManager.Current.Manager.Duplicate(((FileNode)t).Value);
                    }
                }

                //Sort items by name if the user renamed or grouped something
                if (_rename || _group || _duplicateSelected)
                {
                    ProjectManager.Current.Manager.EntityTree.SortFolders();
                    _rename = false;
                    _group = false;
                    _duplicateSelected = false;

                }
            }


            ImGui.EndChild();
            ImGui.PopStyleColor();



            ImGui.PushFont(ImGuiLayout.SubHeadingFont);
            ImGui.Text("Components");
            ImGui.PushFont(ImGuiLayout.DefaultFont);

            int _entitySelected = -1;
            if (ProjectManager.Current.Manager.EntityTree.Selected.Count == 1 && ProjectManager.Current.Manager.EntityTree.Selected[0] is FileNode)
            {
                _entitySelected = ((FileNode)ProjectManager.Current.Manager.EntityTree.Selected[0]).Value;
            }

            ImGui.BeginChild("Component_List", System.Numerics.Vector2.Zero - new System.Numerics.Vector2(0, 31f), true, ImGuiWindowFlags.None);
            if (ImGui.BeginPopupContextItem("Component_add"))
            {
                if (ImGui.BeginMenu("Add Component"))
                {
                    foreach (string ypt in ProjectManager.Current.Manager.GetComponentDictionary().Keys)
                    {
                        if (ImGui.MenuItem(Type.GetType(ypt).Name))
                        {
                            ProjectManager.Current.Manager.GetEntity(_entitySelected).AddComponent(Type.GetType(ypt));
                            break;
                        }
                    }
                    ImGui.EndMenu();
                }
                ImGui.EndPopup();
            }

            if (_entitySelected > -1)
            {
                var dict = ProjectManager.Current.Manager.GetComponentDictionary();
                foreach (string t in dict.Keys)
                {
                    if (dict[t].ContainsKey(_entitySelected))
                    {
                        Component c = dict[t][_entitySelected];
                        ImGui.PushFont(ImGuiLayout.SlightlyLargerFontThanNormal);
                        ImGui.Text(c.GetType().Name);
                        if(ImGui.BeginPopupContextItem(String.Format("{0}_context", t)))
                        {
                            if (ImGui.Selectable("Remove"))
                            {
                                ProjectManager.Current.Manager.RemoveComponent(c);
                                ImGui.EndPopup();
                                break;
                            }
                            if (ImGui.BeginMenu("Add Component"))
                            {
                                foreach (string ypt in ProjectManager.Current.Manager.GetComponentDictionary().Keys)
                                {
                                    if (ImGui.MenuItem(Type.GetType(ypt).Name))
                                    {
                                        ProjectManager.Current.Manager.GetEntity(_entitySelected).AddComponent(Type.GetType(ypt));
                                        break;
                                    }
                                }
                                ImGui.EndMenu();
                            }
                            ImGui.EndPopup();
                        }
                        ImGui.PushFont(ImGuiLayout.DefaultFont);
                        ImGui.SameLine(ImGui.GetWindowWidth() - 80);


                        ImGui.Text("");
                        CatalystPropertyParser.RenderObjectProperties(c);
                        ImGui.Text("");
                        ImGui.Separator();
                    }

                }
            }
            ImGui.EndChild();
            ImGui.PopStyleColor();

            ImGui.PushStyleColor(ImGuiCol.ChildBg, new System.Numerics.Vector4(0.23f, 0.23f, 0.25f, 1.00f));
            ImGui.BeginChild("Component_Buttons", new System.Numerics.Vector2(0, 26f), false, ImGuiWindowFlags.None);

            ImGui.PushItemWidth(100);

            if (ImGui.Button("Add Component") && _entitySelected != -1)
            {
                ImGui.OpenPopup("Add_Component_Menu");
            }

            if (ImGui.BeginPopup("Add_Component_Menu"))
            {
                foreach (string t in ProjectManager.Current.Manager.GetComponentDictionary().Keys)
                {
                    if (ImGui.MenuItem(Type.GetType(t).Name))
                    {
                        ProjectManager.Current.Manager.GetEntity(_entitySelected).AddComponent(Type.GetType(t));
                        break;
                    }
                }

                ImGui.EndPopup();
            }

            ImGui.EndChild();
            ImGui.PopStyleColor();

        }

        public static void RenderCameraWindow()
        {
            ImGui.Text("Camera");
            CatalystPropertyParser.RenderObjectProperties(ProjectManager.Current.Camera);
        }
    }
}
