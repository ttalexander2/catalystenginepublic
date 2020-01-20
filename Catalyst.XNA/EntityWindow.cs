using System;
using System.Reflection;
using System.Text;
using Chroma.Engine;
using ImGuiNET;
namespace Catalyst.XNA
{
    public static class EntityWindow
    {
        private static int selected = -1;
        public static void RenderEntityWindow()
        {
            ImGui.Text("Entities");
            ImGui.BeginChild("Entity_List", ImGui.GetWindowSize() * new System.Numerics.Vector2(0, 0.3f));
            if (ProjectManager.Current != null)
            {
                foreach (Entity e in ProjectManager.Current.Manager.GetEntities().Values)
                {
                    if (ImGui.Selectable(e.Name, selected == e.UID))
                    {
                        selected = e.UID;
                    }
                }
            }
            ImGui.EndChild();
            ImGui.PopStyleColor();

            ImGui.Text("Components");
            ImGui.BeginChild("Component_List");

            if (selected > -1)
            {
                var dict = ProjectManager.Current.Manager.GetComponentDictionary();
                foreach (String t in dict.Keys)
                {
                    if (dict[t].ContainsKey(selected))
                    {
                        ImGui.Separator();
                        ImGui.Text(t);
                        ImGui.Text("");
                        AComponent c = dict[t][selected];
                        var properties = c.GetType().GetProperties();
                        foreach (var p in properties)
                        {
                            ImGui.Text(p.Name);
                        }

                    }

                }
            }
            ImGui.EndChild();
            ImGui.PopStyleColor();
        }
    }
}
