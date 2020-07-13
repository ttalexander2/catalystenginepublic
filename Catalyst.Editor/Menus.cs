using System;
using System.Linq;
using Catalyst.Engine;
using Catalyst.Engine.Rendering;
using Catalyst.Engine.Utilities;
using CatalystEditor;
using ImGuiNET;
using Microsoft.Xna.Framework.Input;
using static Catalyst.Engine.FileTree<Catalyst.Engine.GameObject>;

namespace Catalyst.Editor
{
    public static class Menus
    {
        public static bool EntityWindowOpen = true;
        public static bool InspectorWindowOpen = true;
        public static bool SpriteWindowOpen = true;
        public static bool CameraWindowOpen = true;

        public static GameObject Inspecting { get; set; }

        private static bool _group = false;
        private static bool _rename = false;
        private static bool _removeSelected = false;
        private static bool _contextOpen = false;
        private static bool _duplicateSelected = false;
        private static Node _start = null;

        public static void RenderRightDock()
        {
            if (ImGui.BeginTabBar("LeftDock"))
            {
                if (ImGui.BeginTabItem("Inspector", ref InspectorWindowOpen, ImGuiTabItemFlags.NoCloseWithMiddleMouseButton))
                {
                    RenderInspector();
                    ImGui.EndTabItem();
                }
                ImGui.EndTabBar();
            }
            
        }

        public static void RenderInspector()
        {
            if (ImGui.BeginChild("Inspector", ImGui.GetWindowSize() * new System.Numerics.Vector2(0, 0.96f), true, ImGuiWindowFlags.None))
            {
                if (Inspecting != null)
                {
                    ImGui.PushFont(ImGuiLayout.HeadingFont);
                    ImGui.Text(Inspecting.Name);
                    RenderGameObjectIcon(Inspecting, ImGui.GetWindowSize().X-32);
                    ImGui.PushFont(ImGuiLayout.DefaultFont);

                    CatalystPropertyParser.RenderObjectProperties(Inspecting);

                    if (Inspecting is Entity)
                    {
                        Entity selected = (Entity)Inspecting;
                        
                        var dict = ProjectManager.Current.Manager.GetComponentDictionary();
                        ImGui.BeginGroup();



                        foreach (string t in dict.Keys.OrderBy<string, string>(s => Type.GetType(s).Name))
                        {
                            if (dict[t].ContainsKey(selected.UID))
                            {
                                Component c = dict[t][selected.UID];

                                ImGui.PushFont(ImGuiLayout.SlightlyLargerFontThanNormal);

                                ImGui.PushStyleColor(ImGuiCol.HeaderActive, new System.Numerics.Vector4(0.26f, 0.59f, 0.98f, 0.31f));
                                ImGui.PushStyleColor(ImGuiCol.HeaderHovered, new System.Numerics.Vector4(0.26f, 0.59f, 0.98f, 0.31f));

                                if (ImGui.CollapsingHeader(c.GetType().Name, ImGuiTreeNodeFlags.OpenOnArrow))
                                {
                                    ImGui.PushFont(ImGuiLayout.DefaultFont);

                                    ImGui.PopStyleColor(2);

                                    ImGui.SetItemAllowOverlap();

                                    ImGui.SameLine(ImGui.GetWindowWidth() - 36);


                                    bool visible = c.Visible;

                                    if (visible)
                                    {
                                        if (ImGui.ImageButton(IconLoader.Visible, IconLoader.Icon16Size)){
                                            c.Visible = !visible;
                                        }
                                    } else
                                    {
                                        if (ImGui.ImageButton(IconLoader.NotVisible, IconLoader.Icon16Size)){
                                            c.Visible = !visible;
                                        }
                                    }

                                    //ImGui.SameLine(ImGui.GetWindowWidth() - 20);



                                    if (ImGui.BeginPopupContextItem(string.Format("{0}_context", t)))
                                    {
                                        if (ImGui.Selectable("Remove"))
                                        {
                                            ProjectManager.Current.Manager.RemoveComponent(c);
                                            ImGui.EndPopup();
                                            break;
                                        }
                                        if (ImGui.BeginMenu("Add Component"))
                                        {
                                            foreach (string ypt in ProjectManager.Current.Manager.CreatableTypes)
                                            {
                                                if (ImGui.MenuItem(Type.GetType(ypt).Name))
                                                {
                                                    ProjectManager.Current.Manager.GetEntity(selected.UID).AddComponent(Type.GetType(ypt));
                                                    break;
                                                }
                                            }
                                            ImGui.EndMenu();
                                        }
                                        ImGui.EndPopup();
                                    }
                                    ImGui.AlignTextToFramePadding();
                                    CatalystPropertyParser.RenderObjectProperties(c);
                                }



                            }
                        }

                        ImGui.EndGroup();

                        //ImGui.BeginChild("Component_Buttons", new System.Numerics.Vector2(0, 26f), false, ImGuiWindowFlags.None);

                        float currentwidth = ImGui.GetWindowSize().X;
                        ImGui.SetNextItemWidth(currentwidth * 2);

                        if (ImGui.Button("Add Component"))
                        {
                            ImGui.OpenPopup("Add_Component_Menu");
                        }



                        if (ImGui.BeginPopup("Add_Component_Menu"))
                        {
                            foreach (string t in ProjectManager.Current.Manager.CreatableTypes)
                            {
                                if (ImGui.MenuItem(Type.GetType(t).Name))
                                {
                                    ((Entity)Inspecting).AddComponent(Type.GetType(t));
                                    break;
                                }
                            }
                            ImGui.EndPopup();
                        }



                    }

                }

            }
            ImGui.EndChild();
            ImGui.PopStyleColor();
        }

        public static void RenderLeftDock()
        {
            if (ImGui.BeginTabBar("RightDock"))
            {
                if (ImGui.BeginTabItem("Hierarchy", ref EntityWindowOpen))
                {
                    RenderHierarchyWindow();
                    ImGui.EndTabItem();
                }
            }

        }

        private static void RenderTree(FolderNode root)
        {
            ImGuiTreeNodeFlags base_flags = ImGuiTreeNodeFlags.OpenOnArrow | ImGuiTreeNodeFlags.OpenOnDoubleClick;
            if (ProjectManager.Current.HierarchyTree.Selected.Count <= 0)
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
                                    ProjectManager.Current.HierarchyTree.SelectBetween(_start, n);
                                }
                                else if (CatalystEditor.Instance.keyboardState.IsKeyDown(Keys.LeftControl) || CatalystEditor.Instance.keyboardState.IsKeyDown(Keys.RightControl))
                                {
                                    ProjectManager.Current.HierarchyTree.AddToSelection(n);
                                }
                                else
                                {
                                    ProjectManager.Current.HierarchyTree.Select(n);
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
                                ProjectManager.Current.HierarchyTree.Deselect();
                            }
                        }
                    }
                    else
                    {

                        ImGuiTreeNodeFlags node_flags = ImGuiTreeNodeFlags.Leaf | ImGuiTreeNodeFlags.NoTreePushOnOpen; // ImGuiTreeNodeFlags_Bullet

                        if (n.Selected)
                            node_flags |= ImGuiTreeNodeFlags.Selected;

                        _ = ImGui.TreeNodeEx(((FileNode)n).Name, node_flags);


                        if (ImGui.IsItemClicked())
                        {
                            if (_start != null && _start != n && CatalystEditor.Instance.keyboardState.IsKeyDown(Keys.LeftShift) || CatalystEditor.Instance.keyboardState.IsKeyDown(Keys.RightShift))
                            {
                                ProjectManager.Current.HierarchyTree.SelectBetween(_start, n);
                            }
                            else if (CatalystEditor.Instance.keyboardState.IsKeyDown(Keys.LeftControl) || CatalystEditor.Instance.keyboardState.IsKeyDown(Keys.RightControl))
                            {
                                ProjectManager.Current.HierarchyTree.AddToSelection(n);
                            }
                            else
                            {
                                ProjectManager.Current.HierarchyTree.Select(n);
                                _start = n;
                            }
                        }

                        RenderGameObjectIcon(((FileNode)n).Value, ImGui.GetWindowWidth() - 32);
                    }
                }
                else
                {
                    string buff = n.Name.Trim();
                    ImGui.InputText("", ref buff, 64);
                    ImGui.SetKeyboardFocusHere();
                    ImGui.SetScrollHereY();
                    if (n is FileNode)
                    {
                        ((FileNode)n).Value.Rename(buff.Trim());
                        n.Name = buff.Trim();
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
                        if (((FileNode)n).Value is Entity)
                        {
                            Entity e = (Entity)((FileNode)n).Value;
                            foreach (Entity ee in ProjectManager.Current.Manager.GetEntities().Values)
                            {
                                if (ee.Name.Equals(e.Name) && ee.UID != e.UID)
                                {
                                    valid = false;
                                }
                            }
                        }

                    }

                    if (valid)
                    {
                        n.Editing = false;
                        ProjectManager.Current.HierarchyTree.Select(n);
                        _rename = true;
                    }
                }

                if (!ViewportRenderer.Playing && n.Selected && !_contextOpen && ImGui.BeginPopupContextWindow())
                {
                    _contextOpen = true;
                    if (!ProjectManager.Current.HierarchyTree.MultiSelection && ImGui.Selectable("Rename"))
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

        public static void RenderHierarchyWindow()
        {
            //List of entities
            ImGui.BeginChild("Hierarchy Child", ImGui.GetWindowSize() * new System.Numerics.Vector2(0, 0.96f), true, ImGuiWindowFlags.None);

            if (ProjectManager.Current != null)
            {
                //Render selectable file tree
                RenderTree(ProjectManager.Current.HierarchyTree.Root);

                //If the user elected to group the selected items
                if (_group)
                {
                    ProjectManager.Current.HierarchyTree.GroupSelected();
                    _group = false;
                }

                if (_removeSelected)
                {
                    foreach (GameObject t in ProjectManager.Current.HierarchyTree.RemoveSelected())
                    {
                        if (t is Entity)
                            ((Entity)t).DestroyEntity();
                        else if (t is Camera)
                            ProjectManager.Current.Cameras.Remove((Camera)t);
                    }

                    ProjectManager.Current.HierarchyTree.Deselect();
                    _removeSelected = false;

                }

                if (_duplicateSelected)
                {
                    foreach (Node t in ProjectManager.Current.HierarchyTree.Selected)
                    {
                        if (t is FileNode)
                        {
                            if (((FileNode)t).Value is Entity)
                                ProjectManager.Current.Manager.Duplicate(((Entity)((FileNode)t).Value).UID);
                            else if (((FileNode)t).Value is Camera)
                                ProjectManager.Current.Cameras.Add(Utility.DeepClone<Camera>((Camera)((FileNode)t).Value));
                        }
                            
                    }
                }

                //Sort items by name if the user renamed or grouped something
                if (_rename || _group || _duplicateSelected)
                {
                    ProjectManager.Current.HierarchyTree.SortFolders();
                    _rename = false;
                    _group = false;
                    _duplicateSelected = false;

                }

                if (ProjectManager.Current.HierarchyTree.Selected.Count == 1 && ProjectManager.Current.HierarchyTree.Selected[0] is FileNode)
                {
                    Inspecting = ((FileNode)ProjectManager.Current.HierarchyTree.Selected[0]).Value;
                }
                else
                {
                    Inspecting = null;
                }
            }

            ImGui.EndChild();
            ImGui.PopStyleColor();


            ImGui.PushStyleColor(ImGuiCol.ChildBg, new System.Numerics.Vector4(0.23f, 0.23f, 0.25f, 1.00f));


            ImGui.EndChild();
            ImGui.PopStyleColor();

        }

        public static void RenderCameraWindow()
        {
            ImGui.Text("Camera");
            if (ImGui.BeginChild("Camera properties")){
                CatalystPropertyParser.RenderObjectProperties(ProjectManager.Current.Camera);
                ImGui.EndChild();
            }
            

        }

        public static void RenderGameObjectIcon(GameObject obj, float distance)
        {
            if (obj is Entity)
            {
                ImGui.SameLine(distance);
                ImGui.Image(IconLoader.Entity, IconLoader.Icon16Size);
            }
            else if (obj is Camera)
            {
                ImGui.SameLine(distance);
                ImGui.Image(IconLoader.Camera, IconLoader.Icon16Size);
            }
        }
    }
}
