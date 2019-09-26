using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Catalyst.Controller
{
    public static class ProjectManager
    {
        public static string WorkSpacePath { get; set; } = (Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Catalyst Projects");

        public static readonly string Extention = ".cytp";

        public static void CreateNewProject(string name)
        {
            System.IO.Directory.CreateDirectory(WorkSpacePath + "\\" + name + "\\");
            File.Create(WorkSpacePath + "\\" + name + "\\" + name + Extention);
        }


    }
}
