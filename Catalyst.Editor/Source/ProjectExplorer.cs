using Catalyst.Editor;
using ImGuiNET;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace CatalystEditor.Source
{
    public static class ProjectExplorer
    {
        private static string _windowId = "Open...##ProjectExplorerID";
        public static string WindowId
        {
            get
            {
                return _windowId;
            }
            set
            {
                _windowId = string.Format("{0}##ProjectExplorerID", value);
            }
        }

        public static bool WindowOpen = true;

        public static string SelectedFile = "";


        private static string Filter = "";


        private static string _startingPath
        {
            get
            {
                return ProjectManager.ProjectPath;
            }
        }
        public static bool AllowAll = true;
        public static bool MultiSelect = false;
        public static List<string> Extensions = new List<string>();
        public static List<string> Selected = new List<string>();

        private static string _currentFolder;

        public static bool RenderWindow()
        {

            if (ProjectManager.ProjectPath == null)
                return false;

            if (_currentFolder == null)
                _currentFolder = ProjectManager.ProjectPath;

            string buff = Filter;

            ImGui.Text("Search: ");
            ImGui.SameLine();
            ImGui.SetNextItemWidth(443);
            ImGui.InputText("##Search:23897429847", ref buff, 128);
            Filter = buff.Trim();

            bool result = false;
            
            DirectoryInfo di = new DirectoryInfo(_currentFolder);
            if (di.Exists)
            {
                if (_currentFolder != ProjectManager.ProjectPath)
                {
                    if (ImGui.Selectable("../", false, ImGuiSelectableFlags.DontClosePopups))
                    {
                        _currentFolder = di.Parent.FullName;
                    }
                }
                foreach (var fse in Directory.EnumerateFileSystemEntries(di.FullName))
                {
                    string name = Path.GetFileName(fse);
                    if (!string.IsNullOrWhiteSpace(Filter))
                    {
                        if (!name.ToUpper().StartsWith(Filter.ToUpper()))
                            continue;
                    }

                    if (Directory.Exists(fse))
                    {
                        if (((new DirectoryInfo(fse).Attributes) & FileAttributes.Hidden) != 0)
                            continue;
                        ImGui.Image(IconLoader.Folder, IconLoader.Icon16Size);
                        ImGui.SameLine(0);
                        if (ImGui.Selectable(name, false, ImGuiSelectableFlags.DontClosePopups))
                        {
                            _currentFolder = fse;
                            Selected.Clear();

                        }
                    }
                    else
                    {

                        if ((File.GetAttributes(fse) & FileAttributes.Hidden) != 0)
                            continue;

                        bool isSelected = Selected.Contains(fse) || SelectedFile == fse;

                        if (ImGui.Selectable(name, isSelected, ImGuiSelectableFlags.DontClosePopups))
                        {
                            if (!Keyboard.GetState().IsKeyDown(Keys.LeftControl) && !Keyboard.GetState().IsKeyDown(Keys.RightControl))
                                Selected.Clear();
                            else
                            {
                                if (MultiSelect)
                                    Selected.Add(fse);
                                else
                                    Selected.Clear();
                                if (!Selected.Contains(SelectedFile) && MultiSelect)
                                    Selected.Add(SelectedFile);
                            }
                            SelectedFile = fse;

                        }
                        if (ImGui.IsWindowFocused() && ImGui.IsMouseDoubleClicked(0))
                        {
                            if (SelectedFile != null)
                                HandleFileOpen();
                        }
                    }


                    
                }

            }

            return result;
        }

        private static void HandleFileOpen()
        {
            string extension = Path.GetExtension(SelectedFile);

            switch (extension)
            {
                case ProjectManager.LevelExtension:
                    {
                        ProjectManager.OpenLevel(SelectedFile);
                        break;
                    }
                default:
                    {
                        TextEditor.CreateTextEditor(SelectedFile);
                        break;
                    }
            }

            SelectedFile = null;
        }
    }
}
