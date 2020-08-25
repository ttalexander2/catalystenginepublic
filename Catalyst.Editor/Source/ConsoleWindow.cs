using Catalyst.Editor;
using Catalyst.Engine.Utilities;
using ImGuiNET;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;

namespace CatalystEditor.Source
{
    public static class ConsoleWindow
    {
        public static bool WindowOpen = true;
        private static byte[] _inputBuf;
        private static string _stringBuffer;
        private static List<string> _items;
        public static Dictionary<string, Action> Commands;
        private static List<string> _history;
        private static int _historyPos;    // -1: new line, 0..History.Size-1 browsing history.
        private static string _filter;
        private static bool _autoScroll;
        private static bool _scrollToBottom;

        public static void Init()
        {
            Log.SetWriteFunction(WriteStringToLog);
            Log.SetWriteFunction(WriteCharToLog);
            _inputBuf = new byte[512];
            _items = new List<string>();
            Commands = new Dictionary<string, Action>();
            _history = new List<string>();
            _historyPos = -1;
            _autoScroll = true;
            _scrollToBottom = true;
            _filter = "";
            Commands.Add("clear", () => { ClearLog(); });
            Commands.Add("exit", () => 
            {
                Mouse.SetPosition((int)ImGuiLayout.xButtonLocation.X-5, (int)ImGuiLayout.xButtonLocation.Y-5);
                Log.WriteLine("Do it yourself.");
            });
        }


        

        public static void Render()
        {

            if (ImGui.BeginPopupContextItem())
            {
                if (ImGui.MenuItem("Close Console"))
                    WindowOpen = false;
                ImGui.EndPopup();
            }


            if (ImGui.Button("Clear##clear_log"))
            {
                _items.Clear();
            }

            ImGui.SameLine();

            bool copyToClip = ImGui.Button("Copy##console_copy_to_clipboard");



            if (ImGui.BeginPopup("Options"))
            {
                ImGui.Checkbox("Auto-scroll", ref _autoScroll);
                ImGui.EndPopup();
            }

            ImGui.SameLine();

            if (ImGui.Button("Options"))
                ImGui.OpenPopup("Options");

            ImGui.SameLine();
            ImGui.Text("Filter: ");
            ImGui.SameLine();
            ImGui.InputText("##console_log_filter", ref _filter, 256);
            ImGui.Separator();

            // Reserve enough left-over height for 1 separator + 1 input text
            float footer_height_to_reserve = ImGui.GetStyle().ItemSpacing.Y + ImGui.GetFrameHeightWithSpacing();
            ImGui.BeginChild("ScrollingRegion", new System.Numerics.Vector2(0, -footer_height_to_reserve), false, ImGuiWindowFlags.HorizontalScrollbar);
            if (ImGui.BeginPopupContextWindow())
            {
                if (ImGui.Selectable("Clear")) ClearLog();
                ImGui.EndPopup();
            }

            ImGui.PushStyleVar(ImGuiStyleVar.ItemSpacing, new System.Numerics.Vector2(4, 1)); // Tighten spacing
            if (copyToClip)
                ImGui.LogToClipboard();
            for (int i = 0; i < _items.Count; i++)
            {
                string item = _items[i];

                if (!Regex.IsMatch(item, _filter))
                    continue;

                // Normally you would store more information in your item than just a string.
                // (e.g. make Items[] an array of structure, store color/type etc.)
                System.Numerics.Vector4 color = System.Numerics.Vector4.Zero;
                bool has_color = false;
                if (item.StartsWith("[error]")) 
                {
                    color = new System.Numerics.Vector4(1.0f, 0.4f, 0.4f, 1.0f); 
                    has_color = true; 
                }
                else if (item.StartsWith("# "))
                { 
                    color = new System.Numerics.Vector4(1.0f, 0.8f, 0.6f, 1.0f); 
                    has_color = true; 
                }
                if (has_color)
                    ImGui.PushStyleColor(ImGuiCol.Text, color);
                ImGui.TextUnformatted(item);
                if (has_color)
                    ImGui.PopStyleColor();
            }
            if (copyToClip)
                ImGui.LogFinish();

            if (_scrollToBottom || (_autoScroll && ImGui.GetScrollY() >= ImGui.GetScrollMaxY()))
                ImGui.SetScrollHereY(1.0f);
            _scrollToBottom = false;

            ImGui.PopStyleVar();
            ImGui.EndChild();
            ImGui.Separator();


            // Command-line

            ImGui.Text("Input: ");
            ImGui.SameLine();

            bool reclaim_focus = false;
            ImGuiInputTextFlags input_text_flags = ImGuiInputTextFlags.EnterReturnsTrue;
            if (ImGui.InputText("##Input_console_log", _inputBuf, 512, input_text_flags))
            {
                string s = Encoding.UTF8.GetString(_inputBuf);
                string s_cleaned = s.Replace("\n", "").Replace("\r", "").Replace("\0", "").Trim();
                if (!string.IsNullOrEmpty(s_cleaned))
                    ExecCommand(s_cleaned);
                _inputBuf = new byte[512];
                reclaim_focus = true;
            }

            // Auto-focus on window apparition
            ImGui.SetItemDefaultFocus();
            if (reclaim_focus)
                ImGui.SetKeyboardFocusHere(-1); // Auto focus previous widget

        }

        private static void ExecCommand(string s)
        {
            if (Commands.ContainsKey(s.ToLower()))
            {
                Commands[s].Invoke();
            }
            else
            {
                _items.Add($"[error]: \"{s}\" not a valid command.");
            }
        }

        private static void ClearLog()
        {
            _items.Clear();
        }

        public static void WriteStringToLog(string value)
        {
            value = value.Trim();
            foreach (string s in value.Split('\n'))
            {
                _items.Add(s);
            }
            
        }

        public static void WriteCharToLog(char value)
        {
            _stringBuffer += value;
            if (_stringBuffer.Length >= 256 || value == '\n')
            {
                _items.Add(_stringBuffer);
                _stringBuffer = "";
            }
        }
    }
}
