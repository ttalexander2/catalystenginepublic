using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Chroma;
using Chroma.Engine;
using Chroma.Engine.Utilities;
using ImGuiNET;

namespace Catalyst.XNA
{
    public static class CatalystPropertyParser
    {

        public static void RenderComponentProperties(AComponent c)
        {
            var properties = c.GetType().GetProperties();
            foreach (PropertyInfo p in properties)
            {
                if (p.CanWrite)
                {
                    foreach(object attr in p.GetCustomAttributes(true))
                    {
                        if (attr is ImmediateInteger)
                        {
                            try
                            {
                                RenderInt(c, p, (ImmediateInteger)attr);
                            }
                            catch (Exception e)
                            {
                                ImGui.Text(string.Format("{0} could not be displayed as an integer.", p.Name));
                            }

                        }
                        else if (attr is ImmediateFloat)
                        {
                            try
                            {
                                RenderFloat(c, p, (ImmediateFloat)attr);
                            }
                            catch (Exception e)
                            {
                                ImGui.Text(string.Format("{0} could not be displayed as a float.", p.Name));
                            }

                        }
                        else if (attr is ImmediateBoolean)
                        {
                            try
                            {
                                RenderBoolean(c, p);
                            }
                            catch (Exception e)
                            {
                                ImGui.Text(string.Format("{0} could not be displayed as a bool.", p.Name));
                            }
                        }
                        else if (attr is ImmediateVector2)
                        {
                            try
                            {
                                RenderVector2(c, p, (ImmediateVector2)attr);
                            }
                            catch (Exception e)
                            {
                                ImGui.Text(string.Format("{0} could not be displayed as a Vector2.", p.Name));
                            }
                        }
                        else if (attr is ImmediateVector3)
                        {
                            try
                            {
                                RenderVector3(c, p, (ImmediateVector3)attr);
                            }
                            catch (Exception e)
                            {
                                ImGui.Text(string.Format("{0} could not be displayed as a Vector3.", p.Name));
                            }
                        }
                        else if (attr is ImmediateVector4)
                        {
                            try
                            {
                                RenderVector4(c, p, (ImmediateVector4)attr);
                            }
                            catch (Exception e)
                            {
                                ImGui.Text(string.Format("{0} could not be displayed as a Vector4.", p.Name));
                            }
                        }
                        else if (attr is ImmediateColor)
                        {
                            try
                            {
                                RenderColor(c, p, (ImmediateColor)attr);
                            }
                            catch (Exception e)
                            {
                                ImGui.Text(String.Format("{0} could not be displayed as a Color.", p.Name));
                            }
                        }
                        else if (attr is ImmediateLabel)
                        {
                            if (((ImmediateLabel)attr).Label == null)
                            {
                                ImGui.Text(string.Format("{0}: {1}", p.Name, p.GetValue(c).ToString()));
                            }
                            else
                            {
                                ImGui.Text(string.Format("{0}: {1}", p.Name, ((ImmediateLabel)attr).Label));
                            }

                        }
                        else if (attr is ImmediateString)
                        {
                            try
                            {
                                RenderString(c, p, (ImmediateString)attr);
                            }
                            catch (Exception e)
                            {
                                ImGui.Text(String.Format("{0} could not be displayed as an input String.", p.Name));
                            }
                        }
                        else if (attr is ImmediateEnum)
                        {
                            try
                            {
                                RenderEnum(c, p, (ImmediateEnum)attr);
                            }
                            catch (Exception e)
                            {
                                ImGui.Text(String.Format("{0} could not be displayed as an Enum.", p.Name));
                            }
                        }
                        else if (attr is ImmediateEntitySelector)
                        {
                            try
                            {
                                RenderEntitySelector(c, p, (ImmediateEntitySelector)attr);
                            }
                            catch (Exception e)
                            {
                                ImGui.Text(String.Format("{0} could not be displayed as an Entity Selector.\n{1}", p.Name, e));
                            }
                        }
                    }
                    
                }
            }
        }

        private static void RenderInt(AComponent c, PropertyInfo p, ImmediateInteger attribute)
        {
            int value = (int)p.GetValue(c);

            if (attribute.Mode == ImmediateIntegerMode.Default)
            {
                ImGui.InputInt(p.Name, ref value);
            }
            else if (attribute.Mode == ImmediateIntegerMode.Drag)
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
            else if (attribute.Mode == ImmediateIntegerMode.Percent && attribute.HasRange)
            {
                ImGui.DragInt(p.Name, ref value, 0.05f, 0, 100, "%d%%");
            }
            else if (attribute.Mode == ImmediateIntegerMode.Slider && attribute.HasRange)
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

        private static void RenderFloat(AComponent c, PropertyInfo p, ImmediateFloat attribute)
        {
            float value = (float)p.GetValue(c);
            if (attribute.Mode == ImmediateFloatMode.Default)
            {
                ImGui.InputFloat(p.Name, ref value);
            }
            else if (attribute.Mode == ImmediateFloatMode.Angle)
            {
                ImGui.SliderAngle(p.Name, ref value);
            }
            else if (attribute.Mode == ImmediateFloatMode.Drag)
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
            else if (attribute.Mode == ImmediateFloatMode.Scientific)
            {
                ImGui.InputFloat(p.Name, ref value, 0.0f, 0.0f, "%e");
                ImGui.SameLine(); 
                ImGuiLayout.HelpMarker("Click and drag to edit value.\nHold SHIFT/ALT for faster/slower edit.\nDouble-click or CTRL+click to input value.");
            }
            else if (attribute.Mode == ImmediateFloatMode.Slider && attribute.HasRange)
            {
                ImGui.SliderFloat(p.Name, ref value, attribute.Min, attribute.Max);
            }
            else if (attribute.Mode == ImmediateFloatMode.Small)
            {
                ImGui.InputFloat(p.Name, ref value, 0.0f, 0.0f, "%.06f");
            }
            else if (attribute.Mode == ImmediateFloatMode.SmallDrag && attribute.HasRange)
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

        private static void RenderBoolean(AComponent c, PropertyInfo p)
        {
            bool value = (bool)p.GetValue(c);
            ImGui.Checkbox(p.Name, ref value);
            p.SetValue(c, value);
        }

        private static void RenderVector2(AComponent c, PropertyInfo p, ImmediateVector2 attribute)
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

        private static void RenderVector3(AComponent c, PropertyInfo p, ImmediateVector3 attribute)
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

        private static void RenderVector4(AComponent c, PropertyInfo p, ImmediateVector4 attribute)
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

        private static void RenderColor(AComponent c, PropertyInfo p, ImmediateColor attribute)
        {
            Vector4 value = ((Color)p.GetValue(c)).ToVector4();

            if (attribute.Mode == ImmediateColorMode.RGB)
            {
                System.Numerics.Vector3 vec = new System.Numerics.Vector3(value.X, value.Y, value.Z);
                ImGui.ColorEdit3(p.Name, ref vec);
                p.SetValue(c, new Color(new Vector4(vec.X, vec.Y, vec.Z, value.W)));
            }
            if (attribute.Mode == ImmediateColorMode.RGBA)
            {
                System.Numerics.Vector4 vec = new System.Numerics.Vector4(value.X, value.Y, value.Z, value.W);
                ImGui.ColorEdit4(p.Name, ref vec);
                p.SetValue(c, new Color(new Vector4(vec.X, vec.Y, vec.Z, vec.W)));
            }
        }

        private static void RenderString(AComponent c, PropertyInfo p, ImmediateString attribute)
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

        private static void RenderEnum(AComponent c, PropertyInfo p, ImmediateEnum attribute)
        {
            object val = p.GetValue(c);
            string[] items = Enum.GetNames(val.GetType());
            int curr = (int)val;
            ImGui.Combo(p.Name, ref curr, items, items.Length);
            p.SetValue(c, curr);
        }

        private static void RenderEntitySelector(AComponent c, PropertyInfo p, ImmediateEntitySelector attribute)
        {
            List<string> names = new List<string>();
            int selected = 0;
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

            string[] nameArray = names.ToArray();

            ImGui.Combo(p.Name, ref selected, nameArray, nameArray.Length);

            p.SetValue(c, ProjectManager.Current.Manager.GetEntity(selected));
        }


    }
}
