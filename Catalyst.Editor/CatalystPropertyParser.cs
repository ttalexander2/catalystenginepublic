using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Catalyst;
using Catalyst.Engine;
using Catalyst.Engine.Utilities;
using ImGuiNET;

namespace Catalyst.Editor
{
    public static class CatalystPropertyParser
    {
        public static void RenderObjectProperties(Object c)
        {
            var properties = c.GetType().GetProperties();
            foreach (PropertyInfo p in properties)
            {
                if (p.CanWrite)
                {
                    foreach (object attr in p.GetCustomAttributes(true))
                    {
                        if (attr is GuiInteger)
                        {
                            try
                            {
                                RenderInt(c, p, (GuiInteger)attr);
                            }
                            catch (Exception)
                            {
                                ImGui.Text(string.Format("{0} could not be displayed as an integer.", p.Name));
                            }

                        }
                        else if (attr is GuiFloat)
                        {
                            try
                            {
                                RenderFloat(c, p, (GuiFloat)attr);
                            }
                            catch (Exception)
                            {
                                ImGui.Text(string.Format("{0} could not be displayed as a float.", p.Name));
                            }

                        }
                        else if (attr is GuiBoolean)
                        {
                            try
                            {
                                RenderBoolean(c, p);
                            }
                            catch (Exception)
                            {
                                ImGui.Text(string.Format("{0} could not be displayed as a bool.", p.Name));
                            }
                        }
                        else if (attr is GuiVector2)
                        {
                            try
                            {
                                RenderVector2(c, p, (GuiVector2)attr);
                            }
                            catch (Exception)
                            {
                                ImGui.Text(string.Format("{0} could not be displayed as a Vector2.", p.Name));
                            }
                        }
                        else if (attr is GuiVector3)
                        {
                            try
                            {
                                RenderVector3(c, p, (GuiVector3)attr);
                            }
                            catch (Exception)
                            {
                                ImGui.Text(string.Format("{0} could not be displayed as a Vector3.", p.Name));
                            }
                        }
                        else if (attr is GuiVector4)
                        {
                            try
                            {
                                RenderVector4(c, p, (GuiVector4)attr);
                            }
                            catch (Exception)
                            {
                                ImGui.Text(string.Format("{0} could not be displayed as a Vector4.", p.Name));
                            }
                        }
                        else if (attr is GuiColor)
                        {
                            try
                            {
                                RenderColor(c, p, (GuiColor)attr);
                            }
                            catch (Exception)
                            {
                                ImGui.Text(String.Format("{0} could not be displayed as a Color.", p.Name));
                            }
                        }
                        else if (attr is GuiLabel)
                        {
                            if (((GuiLabel)attr).Label == null)
                            {
                                ImGui.Text(string.Format("{0}: {1}", p.Name, p.GetValue(c).ToString()));
                            }
                            else
                            {
                                ImGui.Text(string.Format("{0}: {1}", p.Name, ((GuiLabel)attr).Label));
                            }

                        }
                        else if (attr is GuiString)
                        {
                            try
                            {
                                RenderString(c, p, (GuiString)attr);
                            }
                            catch (Exception)
                            {
                                ImGui.Text(String.Format("{0} could not be displayed as an input String.", p.Name));
                            }
                        }
                        else if (attr is GuiEnum)
                        {
                            try
                            {
                                RenderEnum(c, p, (GuiEnum)attr);
                            }
                            catch (Exception)
                            {
                                ImGui.Text(String.Format("{0} could not be displayed as an Enum.", p.Name));
                            }
                        }
                        else if (attr is GuiEntitySelector)
                        {
                            try
                            {
                                RenderEntitySelector(c, p, (GuiEntitySelector)attr);
                            }
                            catch (Exception e)
                            {
                                ImGui.Text(String.Format("{0} could not be displayed as an Entity Selector.\n{1}", p.Name, e));
                            }
                        }
                    }

                }
            }
            var methods = c.GetType().GetMethods();
            foreach (MethodInfo m in methods)
            {
                foreach (object attr in m.GetCustomAttributes(true))
                {
                    if (attr is GuiButton)
                    {
                        try
                        {
                            RenderButton(c, m, (GuiButton)attr);
                        }
                        catch (Exception e)
                        {
                            ImGui.Text(String.Format("{0} could not be displayed as a Button.\n{1}", m.Name, e));
                        }
                    }
                }
            }
        }

        private static void RenderInt(Object c, PropertyInfo p, GuiInteger attribute)
        {
            int value = (int)p.GetValue(c);

            if (attribute.Mode == GuiIntegerMode.Default)
            {
                ImGui.InputInt(p.Name, ref value);
            }
            else if (attribute.Mode == GuiIntegerMode.Drag)
            {
                if (attribute.HasRange)
                {
                    ImGui.DragInt(p.Name, ref value, 0.05f, attribute.Min, attribute.Max);
                }
                else
                {
                    ImGui.DragInt(p.Name, ref value);
                }
                ImGui.SameLine(); 
                ImGuiLayout.HelpMarker("Click and drag to edit value.\nHold SHIFT/ALT for faster/slower edit.\nDouble-click or CTRL+click to input value.");
            }
            else if (attribute.Mode == GuiIntegerMode.Percent && attribute.HasRange)
            {
                ImGui.DragInt(p.Name, ref value, 0.05f, 0, 100, "%d%%");
            }
            else if (attribute.Mode == GuiIntegerMode.Slider && attribute.HasRange)
            {
                ImGui.SliderInt(p.Name, ref value, attribute.Min, attribute.Max);
            }
            else
            {
                return;
            }

            if (attribute.HasRange)
            {
                p.SetValue(c, Utility.Clamp(value, attribute.Min, attribute.Max));
            }
            else
            {
                p.SetValue(c, value);
            }
        }

        private static void RenderFloat(Object c, PropertyInfo p, GuiFloat attribute)
        {
            float value = (float)p.GetValue(c);
            if (attribute.Mode == GuiFloatMode.Default)
            {
                ImGui.InputFloat(p.Name, ref value);
            }
            else if (attribute.Mode == GuiFloatMode.Angle)
            {
                ImGui.SliderAngle(p.Name, ref value, 0);
            }
            else if (attribute.Mode == GuiFloatMode.Drag)
            {
                if (attribute.HasRange)
                {
                    ImGui.DragFloat(p.Name, ref value, 0.005f, attribute.Min, attribute.Max);
                }
                else
                {
                    ImGui.DragFloat(p.Name, ref value, 0.005f);
                }
            }
            else if (attribute.Mode == GuiFloatMode.Scientific)
            {
                ImGui.InputFloat(p.Name, ref value, 0.0f, 0.0f, "%e");
                ImGui.SameLine(); 
                ImGuiLayout.HelpMarker("Click and drag to edit value.\nHold SHIFT/ALT for faster/slower edit.\nDouble-click or CTRL+click to input value.");
            }
            else if (attribute.Mode == GuiFloatMode.Slider && attribute.HasRange)
            {
                ImGui.SliderFloat(p.Name, ref value, attribute.Min, attribute.Max);
            }
            else if (attribute.Mode == GuiFloatMode.Small)
            {
                ImGui.InputFloat(p.Name, ref value, 0.0f, 0.0f, "%.06f");
            }
            else if (attribute.Mode == GuiFloatMode.SmallDrag && attribute.HasRange)
            {
                ImGui.DragFloat(p.Name, ref value, 0.0001f, attribute.Min, attribute.Max, "%.06f");
            }

            if (attribute.HasRange)
            {
                p.SetValue(c, Utility.Clamp(value, attribute.Min, attribute.Max));
            }
            else
            {
                p.SetValue(c, value);
            }
        }

        private static void RenderBoolean(Object c, PropertyInfo p)
        {
            bool value = (bool)p.GetValue(c);
            ImGui.Checkbox(p.Name, ref value);
            p.SetValue(c, value);
        }

        private static void RenderVector2(Object c, PropertyInfo p, GuiVector2 attribute)
        {
            Vector2 value = (Vector2)p.GetValue(c);
            System.Numerics.Vector2 vec = new System.Numerics.Vector2(value.X, value.Y);
            if (attribute.HasRange)
            {
                ImGui.InputFloat2(p.Name, ref vec);
                p.SetValue(c, Vector2.Clamp(new Vector2(vec.X, vec.Y), attribute.Min, attribute.Max));
            }
            else
            {
                ImGui.InputFloat2(p.Name, ref vec);
                p.SetValue(c, new Vector2(vec.X, vec.Y));
            }
        }

        private static void RenderVector3(Object c, PropertyInfo p, GuiVector3 attribute)
        {
            Vector3 value = (Vector3)p.GetValue(c);
            System.Numerics.Vector3 vec = new System.Numerics.Vector3(value.X, value.Y, value.Z);
            if (attribute.HasRange)
            {
                ImGui.InputFloat3(p.Name, ref vec);
                p.SetValue(c, Vector3.Clamp(new Vector3(vec.X, vec.Y, vec.Z), attribute.Min, attribute.Max));
            }
            else
            {
                ImGui.InputFloat3(p.Name, ref vec);
                p.SetValue(c, new Vector3(vec.X, vec.Y, vec.Z));
            }
        }

        private static void RenderVector4(Object c, PropertyInfo p, GuiVector4 attribute)
        {
            Vector4 value = (Vector4)p.GetValue(c);
            System.Numerics.Vector4 vec = new System.Numerics.Vector4(value.X, value.Y, value.Z, value.W);
            if (attribute.HasRange)
            {
                ImGui.InputFloat4(p.Name, ref vec);
                p.SetValue(c, Vector4.Clamp(new Vector4(vec.X, vec.Y, vec.Z, vec.W), attribute.Min, attribute.Max));
            }
            else
            {
                ImGui.InputFloat4(p.Name, ref vec);
                p.SetValue(c, new Vector4(vec.X, vec.Y, vec.Z, vec.W));
            }
        }

        private static void RenderColor(Object c, PropertyInfo p, GuiColor attribute)
        {
            Vector4 value = ((Color)p.GetValue(c)).ToVector4();

            if (attribute.Mode == GuiColorMode.RGB)
            {
                System.Numerics.Vector3 vec = new System.Numerics.Vector3(value.X, value.Y, value.Z);
                ImGui.ColorEdit3(p.Name, ref vec);
                p.SetValue(c, new Color(new Vector4(vec.X, vec.Y, vec.Z, value.W)));
            }
            if (attribute.Mode == GuiColorMode.RGBA)
            {
                System.Numerics.Vector4 vec = new System.Numerics.Vector4(value.X, value.Y, value.Z, value.W);
                ImGui.ColorEdit4(p.Name, ref vec, ImGuiColorEditFlags.AlphaBar);
                p.SetValue(c, new Color(new Vector4(vec.X, vec.Y, vec.Z, vec.W)));
            }
        }

        private static void RenderString(Object c, PropertyInfo p, GuiString attribute)
        {
            string val = (string)p.GetValue(c);
            byte[] buff = Encoding.Default.GetBytes(val);
            if (!attribute.HasHint)
            {
                ImGui.InputText(p.Name, buff, (uint)buff.Length);
                p.SetValue(c, Encoding.Default.GetString(buff));
            }
            else if (attribute.HasHint)
            {
                ImGui.InputTextWithHint(p.Name, attribute.Hint, val, (uint)val.Length);
                p.SetValue(c, Encoding.Default.GetString(buff));
            }
        }

        private static void RenderEnum(Object c, PropertyInfo p, GuiEnum attribute)
        {
            object val = p.GetValue(c);
            string[] items = Enum.GetNames(val.GetType());
            int curr = (int)val;
            ImGui.Combo(p.Name, ref curr, items, items.Length);
            p.SetValue(c, curr);
        }

        private static void RenderEntitySelector(Object c, PropertyInfo p, GuiEntitySelector attribute)
        {
            List<string> names = new List<string>();
            int selected = -1;
            if (p.GetValue(c) == null)
            {
                foreach (Entity e in ProjectManager.Current.Manager.GetEntities().Values)
                {
                    names.Add(e.Name);
                }

                string[] arr = names.ToArray();

                ImGui.Combo(p.Name, ref selected, arr, arr.Length);
                p.SetValue(c, ProjectManager.Current.Manager.GetEntity(selected));

                return;
            }
            Entity val = (Entity)p.GetValue(c);

            foreach (Entity e in ProjectManager.Current.Manager.GetEntities().Values)
            {
                if (val.UID == e.UID)
                {
                    selected = e.UID;
                }
                names.Add(e.Name);
            }
            names.Add("(none)");

            string[] nameArray = names.ToArray();

            ImGui.Combo(p.Name, ref selected, nameArray, nameArray.Length);


            p.SetValue(c, ProjectManager.Current.Manager.GetEntity(selected));

            if (names[selected].Equals("(none)"))
            {
                p.SetValue(c, null);
                selected = -1;
            }
        }

        private static void RenderButton(Object c, MethodInfo p, GuiButton attribute)
        {
            if (attribute.ButtonText == null)
            {
                if (ImGui.Button(p.Name))
                {
                    try
                    {
                        p.Invoke(c, attribute.Params);
                    }
                    catch (Exception e) { }
                }
            }
            else
            {
                if (ImGui.Button(attribute.ButtonText))
                {
                    try
                    {
                        p.Invoke(c, attribute.Params);
                    }
                    catch (Exception e) { }
                }
            }

        }


    }
}
