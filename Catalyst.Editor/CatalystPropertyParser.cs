using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
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
            var fields = c.GetType().GetFields();

            ImGui.BeginGroup();
            foreach (FieldInfo p in fields)
            {
                foreach (object attr in p.GetCustomAttributes(true))
                {
                    if (attr is CatalystAttribute)
                    {
                        ImGui.AlignTextToFramePadding();
                        ImGui.Text(p.Name);
                    }

                }
            }
            foreach (PropertyInfo p in properties)
            {
                foreach (object attr in p.GetCustomAttributes(true))
                {
                    if (attr is CatalystAttribute)
                    {
                        ImGui.AlignTextToFramePadding();
                        ImGui.Text(p.Name);
                    }

                }
            }
            var methods = c.GetType().GetMethods();
            foreach (MethodInfo m in methods)
            {
                foreach (object attr in m.GetCustomAttributes(true))
                {
                    if (attr is CatalystAttribute)
                    {
                        ImGui.AlignTextToFramePadding();
                        ImGui.Text(m.Name);
                    }
                }
            }
            ImGui.EndGroup();
            ImGui.SameLine(ImGui.GetWindowWidth() - 7*ImGui.GetWindowWidth()/10);
            ImGui.BeginGroup();

            foreach (FieldInfo p in fields)
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
            ImGui.EndGroup();
        }

        private static void RenderInt(Object c, PropertyInfo p, GuiInteger attribute)
        {
            int value = (int)p.GetValue(c);

            if (attribute.Mode == GuiIntegerMode.Default)
            {
                ImGui.InputInt(string.Format("##hidelabel {0}{1}", p.Name, c.GetHashCode()), ref value);
            }
            else if (attribute.Mode == GuiIntegerMode.Drag)
            {
                if (attribute.HasRange)
                {
                    ImGui.DragInt(string.Format("##hidelabel {0}{1}", p.Name, c.GetHashCode()), ref value, 0.05f, attribute.Min, attribute.Max);
                }
                else
                {
                    ImGui.DragInt(string.Format("##hidelabel {0}{1}", p.Name, c.GetHashCode()), ref value);
                }
                ImGui.SameLine(); 
                ImGuiLayout.HelpMarker("Click and drag to edit value.\nHold SHIFT/ALT for faster/slower edit.\nDouble-click or CTRL+click to input value.");
            }
            else if (attribute.Mode == GuiIntegerMode.Percent && attribute.HasRange)
            {
                ImGui.DragInt(string.Format("##hidelabel {0}{1}", p.Name, c.GetHashCode()), ref value, 0.05f, 0, 100, "%d%%");
            }
            else if (attribute.Mode == GuiIntegerMode.Slider && attribute.HasRange)
            {
                ImGui.SliderInt(string.Format("##hidelabel {0}{1}", p.Name, c.GetHashCode()), ref value, attribute.Min, attribute.Max);
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
                ImGui.InputFloat(string.Format("##hidelabel {0}{1}", p.Name, c.GetHashCode()), ref value);
            }
            else if (attribute.Mode == GuiFloatMode.Angle)
            {
                ImGui.SliderAngle(string.Format("##hidelabel {0}{1}", p.Name, c.GetHashCode()), ref value, 0);
            }
            else if (attribute.Mode == GuiFloatMode.Drag)
            {
                if (attribute.HasRange)
                {
                    ImGui.DragFloat(string.Format("##hidelabel {0}{1}", p.Name, c.GetHashCode()), ref value, 0.005f, attribute.Min, attribute.Max);
                }
                else
                {
                    ImGui.DragFloat(string.Format("##hidelabel {0}{1}", p.Name, c.GetHashCode()), ref value, 0.005f);
                }
            }
            else if (attribute.Mode == GuiFloatMode.Scientific)
            {
                ImGui.InputFloat(string.Format("##hidelabel {0}{1}", p.Name, c.GetHashCode()), ref value, 0.0f, 0.0f, "%e");
                ImGui.SameLine(); 
                ImGuiLayout.HelpMarker("Click and drag to edit value.\nHold SHIFT/ALT for faster/slower edit.\nDouble-click or CTRL+click to input value.");
            }
            else if (attribute.Mode == GuiFloatMode.Slider && attribute.HasRange)
            {
                ImGui.SliderFloat(string.Format("##hidelabel {0}{1}", p.Name, c.GetHashCode()), ref value, attribute.Min, attribute.Max);
            }
            else if (attribute.Mode == GuiFloatMode.Small)
            {
                ImGui.InputFloat(string.Format("##hidelabel {0}{1}", p.Name, c.GetHashCode()), ref value, 0.0f, 0.0f, "%.06f");
            }
            else if (attribute.Mode == GuiFloatMode.SmallDrag && attribute.HasRange)
            {
                ImGui.DragFloat(string.Format("##hidelabel {0}{1}", p.Name, c.GetHashCode()), ref value, 0.0001f, attribute.Min, attribute.Max, "%.06f");
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
            ImGui.Checkbox(string.Format("##hidelabel {0}{1}", p.Name, c.GetHashCode()), ref value);
            p.SetValue(c, value);
        }

        private static void RenderVector2(Object c, PropertyInfo p, GuiVector2 attribute)
        {
            Vector2 value = (Vector2)p.GetValue(c);
            System.Numerics.Vector2 vec = new System.Numerics.Vector2(value.X, value.Y);
            if (attribute.HasRange)
            {
                ImGui.InputFloat2(string.Format("##hidelabel {0}{1}", p.Name, c.GetHashCode()), ref vec);
                p.SetValue(c, Vector2.Clamp(new Vector2(vec.X, vec.Y), attribute.Min, attribute.Max));
            }
            else
            {
                ImGui.InputFloat2(string.Format("##hidelabel {0}{1}", p.Name, c.GetHashCode()), ref vec);
                p.SetValue(c, new Vector2(vec.X, vec.Y));
            }
        }

        private static void RenderVector3(Object c, PropertyInfo p, GuiVector3 attribute)
        {
            Vector3 value = (Vector3)p.GetValue(c);
            System.Numerics.Vector3 vec = new System.Numerics.Vector3(value.X, value.Y, value.Z);
            if (attribute.HasRange)
            {
                ImGui.InputFloat3(string.Format("##hidelabel {0}{1}", p.Name, c.GetHashCode()), ref vec);
                p.SetValue(c, Vector3.Clamp(new Vector3(vec.X, vec.Y, vec.Z), attribute.Min, attribute.Max));
            }
            else
            {
                ImGui.InputFloat3(string.Format("##hidelabel {0}{1}", p.Name, c.GetHashCode()), ref vec);
                p.SetValue(c, new Vector3(vec.X, vec.Y, vec.Z));
            }
        }

        private static void RenderVector4(Object c, PropertyInfo p, GuiVector4 attribute)
        {
            Vector4 value = (Vector4)p.GetValue(c);
            System.Numerics.Vector4 vec = new System.Numerics.Vector4(value.X, value.Y, value.Z, value.W);
            if (attribute.HasRange)
            {
                ImGui.InputFloat4(string.Format("##hidelabel {0}{1}", p.Name, c.GetHashCode()), ref vec);
                p.SetValue(c, Vector4.Clamp(new Vector4(vec.X, vec.Y, vec.Z, vec.W), attribute.Min, attribute.Max));
            }
            else
            {
                ImGui.InputFloat4(string.Format("##hidelabel {0}{1}", p.Name, c.GetHashCode()), ref vec);
                p.SetValue(c, new Vector4(vec.X, vec.Y, vec.Z, vec.W));
            }
        }

        private static void RenderColor(Object c, PropertyInfo p, GuiColor attribute)
        {
            Vector4 value = ((Color)p.GetValue(c)).ToVector4();

            if (attribute.Mode == GuiColorMode.RGB)
            {
                System.Numerics.Vector3 vec = new System.Numerics.Vector3(value.X, value.Y, value.Z);
                ImGui.ColorEdit3(string.Format("##hidelabel {0}{1}", p.Name, c.GetHashCode()), ref vec);
                p.SetValue(c, new Color(new Vector4(vec.X, vec.Y, vec.Z, value.W)));
            }
            if (attribute.Mode == GuiColorMode.RGBA)
            {
                System.Numerics.Vector4 vec = new System.Numerics.Vector4(value.X, value.Y, value.Z, value.W);
                ImGui.ColorEdit4(string.Format("##hidelabel {0}{1}", p.Name, c.GetHashCode()), ref vec, ImGuiColorEditFlags.AlphaBar);
                p.SetValue(c, new Color(new Vector4(vec.X, vec.Y, vec.Z, vec.W)));
            }
        }

        private static void RenderString(Object c, PropertyInfo p, GuiString attribute)
        {
            string val = (string)p.GetValue(c);
            byte[] buff = Encoding.Default.GetBytes(val);
            if (!attribute.HasHint)
            {
                ImGui.InputText(string.Format("##hidelabel {0}{1}", p.Name, c.GetHashCode()), buff, (uint)buff.Length);
                p.SetValue(c, Encoding.Default.GetString(buff));
            }
            else if (attribute.HasHint)
            {
                ImGui.InputTextWithHint(string.Format("##hidelabel {0}{1}", p.Name, c.GetHashCode()), attribute.Hint, val, (uint)val.Length);
                p.SetValue(c, Encoding.Default.GetString(buff));
            }
        }

        private static void RenderEnum(Object c, PropertyInfo p, GuiEnum attribute)
        {
            object val = p.GetValue(c);
            string[] items = Enum.GetNames(val.GetType());
            int curr = (int)val;
            ImGui.Combo(string.Format("##hidelabel {0}{1}", p.Name, c.GetHashCode()), ref curr, items, items.Length);
            p.SetValue(c, curr);
        }

        private static void RenderEntitySelector(Object c, PropertyInfo p, GuiEntitySelector attribute)
        {
            List<string> names = new List<string>();
            List<int> keys = new List<int>();
            int selected = -1;
            if (p.GetValue(c) == null)
            {
                foreach (KeyValuePair<int, Entity> e in ProjectManager.Current.Manager.GetEntities())
                {
                    names.Add(e.Value.Name);
                    keys.Add(e.Key);
                }

                string[] arr = names.ToArray();

                ImGui.Combo(string.Format("##hidelabel {0}{1}", p.Name, c.GetHashCode()), ref selected, arr, arr.Length);

                if (selected >= 0 && selected < keys.Count)
                    p.SetValue(c, ProjectManager.Current.Manager.GetEntity(keys[selected]));

                return;
            }


            Entity val = (Entity)p.GetValue(c);

            int i = 0;

            foreach (KeyValuePair<int, Entity> e in ProjectManager.Current.Manager.GetEntities())
            {
                if (val.UID == e.Value.UID)
                {
                    selected = i;
                }
                names.Add(e.Value.Name);
                keys.Add(e.Key);
                i++;
            }

            names.Add("(none)");

            string[] nameArray = names.ToArray();

            ImGui.Combo(string.Format("##hidelabel {0}{1}", p.Name, c.GetHashCode()), ref selected, nameArray, nameArray.Length);

            if (names[selected].Equals("(none)") || selected < 0 || selected >= keys.Count)
            {
                p.SetValue(c, null);
            }
            else
            {
                p.SetValue(c, ProjectManager.Current.Manager.GetEntity(keys[selected]));
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
                    catch (Exception e) 
                    { 
                        Console.WriteLine(string.Format("Action {0} could not be invoked. Exeption: {1}", p.Name, e.Message)); 
                    }
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
                    catch (Exception e)
                    {
                        Console.WriteLine(string.Format("Action {0} could not be invoked ({1}). Exeption: {2}", attribute.ButtonText, p.Name, e.Message));
                    }
                }
            }

        }

        private static void RenderInt(Object c, FieldInfo p, GuiInteger attribute)
        {
            int value = (int)p.GetValue(c);

            if (attribute.Mode == GuiIntegerMode.Default)
            {
                ImGui.InputInt(string.Format("##hidelabel {0}{1}", p.Name, c.GetHashCode()), ref value);
            }
            else if (attribute.Mode == GuiIntegerMode.Drag)
            {
                if (attribute.HasRange)
                {
                    ImGui.DragInt(string.Format("##hidelabel {0}{1}", p.Name, c.GetHashCode()), ref value, 0.05f, attribute.Min, attribute.Max);
                }
                else
                {
                    ImGui.DragInt(string.Format("##hidelabel {0}{1}", p.Name, c.GetHashCode()), ref value);
                }
                ImGui.SameLine();
                ImGuiLayout.HelpMarker("Click and drag to edit value.\nHold SHIFT/ALT for faster/slower edit.\nDouble-click or CTRL+click to input value.");
            }
            else if (attribute.Mode == GuiIntegerMode.Percent && attribute.HasRange)
            {
                ImGui.DragInt(string.Format("##hidelabel {0}{1}", p.Name, c.GetHashCode()), ref value, 0.05f, 0, 100, "%d%%");
            }
            else if (attribute.Mode == GuiIntegerMode.Slider && attribute.HasRange)
            {
                ImGui.SliderInt(string.Format("##hidelabel {0}{1}", p.Name, c.GetHashCode()), ref value, attribute.Min, attribute.Max);
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

        private static void RenderFloat(Object c, FieldInfo p, GuiFloat attribute)
        {
            float value = (float)p.GetValue(c);
            if (attribute.Mode == GuiFloatMode.Default)
            {
                ImGui.InputFloat(string.Format("##hidelabel {0}{1}", p.Name, c.GetHashCode()), ref value);
            }
            else if (attribute.Mode == GuiFloatMode.Angle)
            {
                ImGui.SliderAngle(string.Format("##hidelabel {0}{1}", p.Name, c.GetHashCode()), ref value, 0);
            }
            else if (attribute.Mode == GuiFloatMode.Drag)
            {
                if (attribute.HasRange)
                {
                    ImGui.DragFloat(string.Format("##hidelabel {0}{1}", p.Name, c.GetHashCode()), ref value, 0.005f, attribute.Min, attribute.Max);
                }
                else
                {
                    ImGui.DragFloat(string.Format("##hidelabel {0}{1}", p.Name, c.GetHashCode()), ref value, 0.005f);
                }
            }
            else if (attribute.Mode == GuiFloatMode.Scientific)
            {
                ImGui.InputFloat(string.Format("##hidelabel {0}{1}", p.Name, c.GetHashCode()), ref value, 0.0f, 0.0f, "%e");
                ImGui.SameLine();
                ImGuiLayout.HelpMarker("Click and drag to edit value.\nHold SHIFT/ALT for faster/slower edit.\nDouble-click or CTRL+click to input value.");
            }
            else if (attribute.Mode == GuiFloatMode.Slider && attribute.HasRange)
            {
                ImGui.SliderFloat(string.Format("##hidelabel {0}{1}", p.Name, c.GetHashCode()), ref value, attribute.Min, attribute.Max);
            }
            else if (attribute.Mode == GuiFloatMode.Small)
            {
                ImGui.InputFloat(string.Format("##hidelabel {0}{1}", p.Name, c.GetHashCode()), ref value, 0.0f, 0.0f, "%.06f");
            }
            else if (attribute.Mode == GuiFloatMode.SmallDrag && attribute.HasRange)
            {
                ImGui.DragFloat(string.Format("##hidelabel {0}{1}", p.Name, c.GetHashCode()), ref value, 0.0001f, attribute.Min, attribute.Max, "%.06f");
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

        private static void RenderBoolean(Object c, FieldInfo p)
        {
            bool value = (bool)p.GetValue(c);
            ImGui.Checkbox(string.Format("##hidelabel {0}{1}", p.Name, c.GetHashCode()), ref value);
            p.SetValue(c, value);
        }

        private static void RenderVector2(Object c, FieldInfo p, GuiVector2 attribute)
        {
            Vector2 value = (Vector2)p.GetValue(c);
            System.Numerics.Vector2 vec = new System.Numerics.Vector2(value.X, value.Y);
            if (attribute.HasRange)
            {
                ImGui.InputFloat2(string.Format("##hidelabel {0}{1}", p.Name, c.GetHashCode()), ref vec);
                p.SetValue(c, Vector2.Clamp(new Vector2(vec.X, vec.Y), attribute.Min, attribute.Max));
            }
            else
            {
                ImGui.InputFloat2(string.Format("##hidelabel {0}{1}", p.Name, c.GetHashCode()), ref vec);
                p.SetValue(c, new Vector2(vec.X, vec.Y));
            }
        }

        private static void RenderVector3(Object c, FieldInfo p, GuiVector3 attribute)
        {
            Vector3 value = (Vector3)p.GetValue(c);
            System.Numerics.Vector3 vec = new System.Numerics.Vector3(value.X, value.Y, value.Z);
            if (attribute.HasRange)
            {
                ImGui.InputFloat3(string.Format("##hidelabel {0}{1}", p.Name, c.GetHashCode()), ref vec);
                p.SetValue(c, Vector3.Clamp(new Vector3(vec.X, vec.Y, vec.Z), attribute.Min, attribute.Max));
            }
            else
            {
                ImGui.InputFloat3(string.Format("##hidelabel {0}{1}", p.Name, c.GetHashCode()), ref vec);
                p.SetValue(c, new Vector3(vec.X, vec.Y, vec.Z));
            }
        }

        private static void RenderVector4(Object c, FieldInfo p, GuiVector4 attribute)
        {
            Vector4 value = (Vector4)p.GetValue(c);
            System.Numerics.Vector4 vec = new System.Numerics.Vector4(value.X, value.Y, value.Z, value.W);
            if (attribute.HasRange)
            {
                ImGui.InputFloat4(string.Format("##hidelabel {0}{1}", p.Name, c.GetHashCode()), ref vec);
                p.SetValue(c, Vector4.Clamp(new Vector4(vec.X, vec.Y, vec.Z, vec.W), attribute.Min, attribute.Max));
            }
            else
            {
                ImGui.InputFloat4(string.Format("##hidelabel {0}{1}", p.Name, c.GetHashCode()), ref vec);
                p.SetValue(c, new Vector4(vec.X, vec.Y, vec.Z, vec.W));
            }
        }

        private static void RenderColor(Object c, FieldInfo p, GuiColor attribute)
        {
            Vector4 value = ((Color)p.GetValue(c)).ToVector4();

            if (attribute.Mode == GuiColorMode.RGB)
            {
                System.Numerics.Vector3 vec = new System.Numerics.Vector3(value.X, value.Y, value.Z);
                ImGui.ColorEdit3(string.Format("##hidelabel {0}{1}", p.Name, c.GetHashCode()), ref vec);
                p.SetValue(c, new Color(new Vector4(vec.X, vec.Y, vec.Z, value.W)));
            }
            if (attribute.Mode == GuiColorMode.RGBA)
            {
                System.Numerics.Vector4 vec = new System.Numerics.Vector4(value.X, value.Y, value.Z, value.W);
                ImGui.ColorEdit4(string.Format("##hidelabel {0}{1}", p.Name, c.GetHashCode()), ref vec, ImGuiColorEditFlags.AlphaBar);
                p.SetValue(c, new Color(new Vector4(vec.X, vec.Y, vec.Z, vec.W)));
            }
        }

        private static void RenderString(Object c, FieldInfo p, GuiString attribute)
        {
            string val = (string)p.GetValue(c);
            byte[] buff = Encoding.Default.GetBytes(val);
            if (!attribute.HasHint)
            {
                ImGui.InputText(string.Format("##hidelabel {0}{1}", p.Name, c.GetHashCode()), buff, (uint)buff.Length);
                p.SetValue(c, Encoding.Default.GetString(buff));
            }
            else if (attribute.HasHint)
            {
                ImGui.InputTextWithHint(string.Format("##hidelabel {0}{1}", p.Name, c.GetHashCode()), attribute.Hint, val, (uint)val.Length);
                p.SetValue(c, Encoding.Default.GetString(buff));
            }
        }

        private static void RenderEnum(Object c, FieldInfo p, GuiEnum attribute)
        {
            object val = p.GetValue(c);
            string[] items = Enum.GetNames(val.GetType());
            int curr = (int)val;
            ImGui.Combo(string.Format("##hidelabel {0}{1}", p.Name, c.GetHashCode()), ref curr, items, items.Length);
            p.SetValue(c, curr);
        }

        private static void RenderEntitySelector(Object c, FieldInfo p, GuiEntitySelector attribute)
        {
            List<string> names = new List<string>();
            List<int> keys = new List<int>();
            int selected = -1;
            if (p.GetValue(c) == null)
            {
                foreach (KeyValuePair<int, Entity> e in ProjectManager.Current.Manager.GetEntities())
                {
                    names.Add(e.Value.Name);
                    keys.Add(e.Key);
                }

                string[] arr = names.ToArray();

                ImGui.Combo(string.Format("##hidelabel {0}{1}", p.Name, c.GetHashCode()), ref selected, arr, arr.Length);

                if (selected >= 0 && selected < keys.Count)
                    p.SetValue(c, ProjectManager.Current.Manager.GetEntity(keys[selected]));

                return;
            }


            Entity val = (Entity)p.GetValue(c);

            int i = 0;

            foreach (KeyValuePair<int, Entity> e in ProjectManager.Current.Manager.GetEntities())
            {
                if (val.UID == e.Value.UID)
                {
                    selected = i;
                }
                names.Add(e.Value.Name);
                keys.Add(e.Key);
                i++;
            }

            names.Add("(none)");

            string[] nameArray = names.ToArray();

            ImGui.Combo(string.Format("##hidelabel {0}{1}", p.Name, c.GetHashCode()), ref selected, nameArray, nameArray.Length);

            if (names[selected].Equals("(none)") || selected < 0 || selected >= keys.Count)
            {
                p.SetValue(c, null);
            }
            else
            {
                p.SetValue(c, ProjectManager.Current.Manager.GetEntity(keys[selected]));
            }
        }

    }
}
