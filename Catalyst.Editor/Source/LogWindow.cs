using Catalyst.Engine.Utilities;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace CatalystEditor.Source
{
    public static class LogWindow
    {
        public static bool WindowOpen = true;

        private static string _log = "";

        public static void Init()
        {
            Log.SetWriteFunction(WriteCharToLog);
        }

        public static void Render()
        {
            if (ImGui.Button("Clear##clear_log"))
            {
                _log = "";
            }
            ImGui.InputTextMultiline("##log_text_body", ref _log, uint.MaxValue, ImGui.GetWindowSize() - System.Numerics.Vector2.UnitY*70, ImGuiInputTextFlags.ReadOnly);
        }

        public static void WriteCharToLog(char value)
        {
            _log += value;
        }
    }
}
