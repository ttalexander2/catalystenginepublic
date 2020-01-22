using System;
using System.Reflection;
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
                    Console.WriteLine(p.ToString());
                    if (Attribute.GetCustomAttribute(c.GetType(), typeof(ImmediateInteger)) != null)
                    {
                    Console.WriteLine("OOF");
                        ImmediateInteger attr = c.GetType().GetCustomAttribute<ImmediateInteger>(true);
                        RenderInt(c, p, attr);
                    }
                    else if (Attribute.GetCustomAttribute(c.GetType(), typeof(ImmediateFloat)) != null)
                    {
                            
                    }
                    else if (Attribute.GetCustomAttribute(c.GetType(), typeof(ImmediateInteger)) != null)
                    {
                            
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
                if (attribute.HasRange)
                {
                    p.SetValue(c, value);
                }
                else
                {
                    p.SetValue(c, Utility.Clamp<int>(value, attribute.Min, attribute.Max));
                }
            }

        }
    }
}
