using ImGuiNET;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace CatalystEditor.Widgets
{
    public class FileBrowser
    {
        private string _popupId = "Open...##FilePickerID";
        public string PopupId { 
            get 
            {
                return _popupId;
            }
            set 
            {
                _popupId = string.Format("{0}##FilePickerID", value);
            }
        }

        private static readonly Vector2 DefaultFilePickerSize = new Vector2(1000, 725);
        private bool isOpen = false;

        private string CurrentFolder = "";
        public string SelectedFile = "";


        private string Filter = "";


        private string startingPath;
        private int ExtensionOrAll = 0;
        public bool AllowAll;
        public bool MultiSelect;
        public List<string> Extensions = new List<string>();
        public List<string> Selected = new List<string>();
        public FileBrowserResult Result;

        public FileBrowser(string startPath = null, bool allowAll = false, bool multiselect = false, params string[] extensions)
        {
            startingPath = startPath;
            AllowAll = allowAll;
            MultiSelect = multiselect;
            Extensions.AddRange(extensions);
            isOpen = false;
            if (File.Exists(startingPath))
            {
                startingPath = new FileInfo(startingPath).DirectoryName;
            }
            else if (string.IsNullOrEmpty(startingPath) || !Directory.Exists(startingPath))
            {
                startingPath = Catalyst.Editor.CatalystEditor.Instance.Content.RootDirectory;
                if (string.IsNullOrEmpty(startingPath))
                {
                    startingPath = AppContext.BaseDirectory;
                }
            }
        }

        public bool OpenModalPopup()
        {
            if (!isOpen)
            {
                CurrentFolder = startingPath;
                ExtensionOrAll = 0;
                SelectedFile = null;
                Selected.Clear();
                isOpen = true;
            }

            bool result = false;
            ImGui.SetNextWindowSize(DefaultFilePickerSize, ImGuiCond.FirstUseEver);
            if (ImGui.BeginPopupModal(PopupId))
            {

                result = DrawFolder();
                ImGui.EndPopup();
            }

            return result;
        }

        private bool DrawFolder()
        {
            ImGui.Text("Current Folder: " + CurrentFolder);

            ImGui.SameLine(ImGui.GetColumnWidth() - 500);
            string buff = Filter;
            
            ImGui.Text("Search: ");
            ImGui.SameLine();
            ImGui.SetNextItemWidth(443);
            ImGui.InputText("##Search:23897429847", ref buff, 128);
            Filter = buff.Trim();

            bool result = false;
            //Display Environment Folders
            if (ImGui.BeginChildFrame(1, new Vector2(200, 600)))
            {
                if (ImGui.Selectable($"{Path.GetFileName(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile))}##pathselection", false, ImGuiSelectableFlags.DontClosePopups))
                {
                    CurrentFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                    SelectedFile = "";
                    Selected.Clear();
                }

                if (ImGui.Selectable($"{Path.GetFileName(Environment.GetFolderPath(Environment.SpecialFolder.Desktop))}##pathselection", false, ImGuiSelectableFlags.DontClosePopups))
                {
                    CurrentFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    SelectedFile = "";
                    Selected.Clear();
                }
                if (ImGui.Selectable($"{Path.GetFileName(Environment.GetFolderPath(Environment.SpecialFolder.Personal))}##pathselection", false, ImGuiSelectableFlags.DontClosePopups))
                {
                    CurrentFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    SelectedFile = "";
                    Selected.Clear();
                }

                if (ImGui.Selectable($"{Path.GetFileName(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures))}##pathselection", false, ImGuiSelectableFlags.DontClosePopups))
                {
                    CurrentFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                    SelectedFile = "";
                    Selected.Clear();
                }
                if (ImGui.Selectable($"{Path.GetFileName(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic))}##pathselection", false, ImGuiSelectableFlags.DontClosePopups))
                {
                    CurrentFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
                    SelectedFile = "";
                    Selected.Clear();
                }
                if (ImGui.Selectable($"{Path.GetFileName(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos))}##pathselection", false, ImGuiSelectableFlags.DontClosePopups))
                {
                    CurrentFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
                    SelectedFile = "";
                    Selected.Clear();
                }


                ImGui.Separator();
                foreach (DriveInfo drive in DriveInfo.GetDrives())
                {
                    if (string.IsNullOrWhiteSpace(drive.VolumeLabel))
                    {
                        if (ImGui.Selectable($"Local Disk ({drive.Name.Trim('\\')})##driveselection", false, ImGuiSelectableFlags.DontClosePopups))
                        {
                            CurrentFolder = drive.Name;
                        }
                    }
                    else
                    {
                        if (ImGui.Selectable($"{drive.VolumeLabel} ({drive.Name.Trim('\\')})##driveselection", false, ImGuiSelectableFlags.DontClosePopups))
                        {
                            CurrentFolder = drive.Name;
                        }
                    }

                }
            }
            ImGui.EndChildFrame();
            ImGui.SameLine();
            if (ImGui.BeginChildFrame(2, new Vector2(0, 600)))
            {
                DirectoryInfo di = new DirectoryInfo(CurrentFolder);
                if (di.Exists)
                {
                    if (di.Parent != null)
                    {
                        if (ImGui.Selectable("../", false, ImGuiSelectableFlags.DontClosePopups))
                        {
                            CurrentFolder = di.Parent.FullName;
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
                                CurrentFolder = fse;
                                Selected.Clear();

                            }
                        }
                        else
                        {
                            if (ExtensionOrAll == 0 && !Extensions.Contains(Path.GetExtension(fse)))
                                continue;

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
                            if (ImGui.IsMouseDoubleClicked(0))
                            {
                                ImGui.CloseCurrentPopup();
                            }
                        }
                    }
                }
            }
            ImGui.EndChildFrame();
            string buffer = "";
            if (Selected.Count <= 1)
                buffer = Path.GetFileName(SelectedFile);


            if (string.IsNullOrWhiteSpace(buffer))
            {
                buffer = "";
            }

            ImGui.Text("File Name: ");
            ImGui.SameLine();
            ImGui.InputText("##FileName:23498324", ref buffer, 128);
            SelectedFile = Path.Combine(CurrentFolder, buffer.Trim());

            ImGui.SameLine();
            ImGui.SetNextItemWidth(260);
            if (AllowAll)
                ImGui.Combo("##ExtensionSelector3482098", ref ExtensionOrAll, new string[] { string.Join(", ", Extensions), "All (\"*.*\")" }, 2);
            else
            {
                ImGui.Combo("##ExtensionSelector3482098", ref ExtensionOrAll, new string[] { string.Join(", ", Extensions) }, 1);
            }

            ImGui.Text("");
            ImGui.SameLine(ImGui.GetColumnWidth() - 250);
            if (ImGui.Button("Cancel", Vector2.UnitX*125f))
            {
                result = true;
                ImGui.CloseCurrentPopup();
                Extensions.Clear();
                Result = FileBrowserResult.Canceled;
            }

            bool active = true;

            if (Selected.Count < 1 && (SelectedFile == null || !File.Exists(SelectedFile)))
            {
                active = false;
            }
            foreach (string s in Selected)
            {
                if (!File.Exists(s))
                    active = false;
            }
            if (!active)
                ImGui.PushStyleVar(ImGuiStyleVar.Alpha, 0.5f);

            if (!string.IsNullOrWhiteSpace(SelectedFile) && !Selected.Contains(SelectedFile))
                Selected.Add(SelectedFile);

            ImGui.SameLine();
            if (ImGui.Button("Open", Vector2.UnitX * 125f))
            {
                if (active)
                {
                    result = true;
                    ImGui.CloseCurrentPopup();
                    Extensions.Clear();
                    if (Selected.Count > 1)
                        Result = FileBrowserResult.MultiSelect;
                    else
                        Result = FileBrowserResult.SingleSelect;
                }
            }

            if (!active)
                ImGui.PopStyleVar();




            return result;
        }

        public enum FileBrowserResult
        {
            SingleSelect,
            MultiSelect,
            Canceled,
        }
    }

}
