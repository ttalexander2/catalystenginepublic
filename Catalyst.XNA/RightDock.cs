using System;
using System.Reflection;
using System.Text;
using Chroma.Engine;
using ImGuiNET;

namespace Catalyst.XNA
{
    public static class RightDock
    {
        private static int _entitySelected = -1;

        public static void RenderRightDock()
        {
            ImGuiTabBarFlags flags = ImGuiTabBarFlags.Reorderable;

            bool[] opened = { true, true };

            if (ImGui.BeginTabBar("RightDock", flags))
            {
                if (ImGui.BeginTabItem("Entity", ref opened[0]))
                {
                    RenderEntityWindow();
                    ImGui.EndTabItem();
                }
                if (ImGui.BeginTabItem("Sprites", ref opened[1]))
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
            ImGui.SameLine(ImGui.GetWindowWidth() - 80);
            if (ImGui.Button("Add Entity"))
            {
                _entitySelected = ProjectManager.Current.Manager.NewEntity().UID;
            }

            ImGui.BeginChild("Entity_List", ImGui.GetWindowSize() * new System.Numerics.Vector2(0, 0.3f), true, ImGuiWindowFlags.None);
            if (ProjectManager.Current != null)
            {
                foreach (Entity e in ProjectManager.Current.Manager.GetEntities().Values)
                {
                    if (ImGui.Selectable(e.Name, _entitySelected == e.UID))
                    {
                        _entitySelected = e.UID;
                    }
                }
            }
            ImGui.EndChild();
            ImGui.PopStyleColor();



            ImGui.PushFont(ImGuiLayout.SubHeadingFont);
            ImGui.Text("Components");
            ImGui.PushFont(ImGuiLayout.DefaultFont);

            ImGui.BeginChild("Component_List", System.Numerics.Vector2.Zero, true, ImGuiWindowFlags.None);

            if (_entitySelected > -1)
            {
                var dict = ProjectManager.Current.Manager.GetComponentDictionary();
                foreach (String t in dict.Keys)
                {
                    if (dict[t].ContainsKey(_entitySelected))
                    {


                        AComponent c = dict[t][_entitySelected];
                        ImGui.PushFont(ImGuiLayout.SlightlyLargerFontThanNormal);
                        ImGui.Text(c.GetType().Name);
                        ImGui.PushFont(ImGuiLayout.DefaultFont);
                        ImGui.Text("");
                        CatalystPropertyParser.RenderComponentProperties(c);
                        ImGui.Separator();
                    }

                }
            }
            ImGui.EndChild();
            ImGui.PopStyleColor();
        }
    }
}
