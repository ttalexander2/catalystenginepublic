using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Catalyst.Engine;
using ImGuiNET;

namespace Catalyst.XNA
{
    public static class RightDock
    {
        public static bool EntityWindowOpen = true;
        public static bool SpriteWindowOpen = true;
        private static int _entitySelected = -1;
        private static int _editingEntity = -1;
        private static  List<int> _toRemove = new List<int>();
        private static List<int> _toDuplicate = new List<int>();

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

                ImGui.EndTabBar();
            }
        }
        public static void RenderEntityWindow()
        {
            ImGui.PushFont(ImGuiLayout.SubHeadingFont);
            ImGui.Text("Entities");
            ImGui.PushFont(ImGuiLayout.DefaultFont);
            ImGui.SameLine(ImGui.GetWindowWidth() - 92);
            if (ImGui.Button("+ Add Entity"))
            {
                int id = ProjectManager.Current.Manager.NewEntity().UID;
                _editingEntity = id;
                _entitySelected = id;
            }

            ImGui.BeginChild("Entity_List", ImGui.GetWindowSize() * new System.Numerics.Vector2(0, 0.2f), true, ImGuiWindowFlags.None);
            if (ProjectManager.Current != null)
            {
                foreach (Entity e in ProjectManager.Current.Manager.GetEntities().Values)
                {
                    if (_editingEntity == -1 && _entitySelected == e.UID && ImGui.BeginPopupContextWindow())
                    {
                        if (ImGui.Selectable("Rename"))
                        {
                            _editingEntity = e.UID;

                        }
                        if (ImGui.Selectable("Remove"))
                        {
                            _entitySelected = -1;
                            _editingEntity = -1;
                            _toRemove.Add(e.UID);
                            continue;
                        }
                        ImGui.Separator();
                        if (ImGui.Selectable("Duplicate"))
                        {
                            _toDuplicate.Add(e.UID);
                        }
                        ImGui.EndPopup();
                    }

                    if (_entitySelected == e.UID && _editingEntity == -1 && ImGui.IsMouseDoubleClicked(0) && ImGui.IsItemFocused())
                    {
                        _editingEntity = e.UID;
                    }

                    if (e.UID != _editingEntity)
                    {
                        if (ImGui.Selectable(e.Name, _entitySelected == e.UID))
                        {
                            _entitySelected = e.UID;
                        }
                    }
                    else
                    {
                        string buff = e.Name.Trim();
                        ImGui.InputText("", ref buff, 256);
                        ImGui.SetKeyboardFocusHere();
                        ImGui.SetScrollHereY();
                        e.Name = buff.Trim();
                    }

                    if (_editingEntity != -1 && ImGui.IsKeyDown(ImGui.GetKeyIndex(ImGuiKey.Enter)))
                    {
                        bool valid = e.Name.Trim().Length > 0;
                        foreach (Entity ee in ProjectManager.Current.Manager.GetEntities().Values)
                        {
                            if (ee.Name.Equals(e.Name) && ee.UID != e.UID)
                            {
                                valid = false;
                            }
                        }
                        if (valid)
                        {
                            _editingEntity = -1;
                        }
                        else
                        {
                            _entitySelected = e.UID;
                            _editingEntity = e.UID;
                        }
                    }
                }
                foreach(int t in _toRemove)
                {
                    ProjectManager.Current.Manager.GetEntities().Remove(t);
                }
                _toRemove.Clear();
                foreach(int t in _toDuplicate)
                {
                    Entity e = ProjectManager.Current.Manager.Duplicate(t);
                    _editingEntity = e.UID;
                    _entitySelected = e.UID;
                }
                _toDuplicate.Clear();
            }
            ImGui.EndChild();
            ImGui.PopStyleColor();



            ImGui.PushFont(ImGuiLayout.SubHeadingFont);
            ImGui.Text("Components");
            ImGui.PushFont(ImGuiLayout.DefaultFont);

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
                        CatalystPropertyParser.RenderComponentProperties(c);
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
    }
}
