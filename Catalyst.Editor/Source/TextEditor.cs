using Catalyst.Engine;
using ImGuiNET;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CatalystEditor.Source
{
    public class TextEditor
    {
        public bool WindowOpen = true;

        private string _file = null;
        public string Label = "##paosdj9ouiansdu8iabusyidbasd";

        private bool _unsaved = false;

        private string _oldContents = null;
        private string _buffer = null;

        public static List<TextEditor> Editors = new List<TextEditor>();
        public static List<TextEditor> _toRemove = new List<TextEditor>();

        public static void CreateTextEditor(string file)
        {
            foreach (TextEditor t in Editors)
            {
                if (t._file == file)
                    return;
            }
            Editors.Add(new TextEditor(file));
        }

        private TextEditor(string file)
        {
            _file = file;
            Console.WriteLine(file);
            _oldContents = File.ReadAllText(Path.GetFullPath(file), Encoding.UTF8);
            Label = $"{Path.GetFileName(file)} ## {file}";
            _buffer = _oldContents;
        }

        public void RenderWindow()
        {
            _unsaved = _buffer != _oldContents;

            ImGuiWindowFlags flags = ImGuiWindowFlags.None;

            if (_unsaved)
                flags = ImGuiWindowFlags.UnsavedDocument;

            if (ImGui.Begin(Label, ref WindowOpen, flags))
            {
                if (ImGui.IsWindowFocused())
                {
                    if ((Input.keyboardState.IsKeyDown(Keys.LeftControl) || Input.keyboardState.IsKeyDown(Keys.RightControl)) && Input.keyboardState.IsKeyDown(Keys.S))
                    {
                        File.WriteAllText(_file, _buffer);
                        _oldContents = _buffer;
                    }
                }

                ImGui.InputTextMultiline($"##{Label}", ref _buffer, Convert.ToUInt32(_buffer.Length + sizeof(char)*4096), ImGui.GetWindowSize() - System.Numerics.Vector2.UnitY * 70, ImGuiInputTextFlags.AlwaysInsertMode);

                
            }
            ImGui.End();

        }

        public static void RemoveClosed()
        {
            foreach (TextEditor t in Editors)
            {
                if (!t.WindowOpen)
                    _toRemove.Add(t);
            }

            foreach (TextEditor t in _toRemove)
            {
                Editors.Remove(t);
            }

            _toRemove.Clear();
            
        }
    }
}
