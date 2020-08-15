using Catalyst.Engine;
using Catalyst.Engine.Rendering;
using Catalyst.Engine.Input;
using Catalyst.Engine.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Runtime.Loader;
using System.Threading;
using Newtonsoft.Json;
using System.Text;

namespace Catalyst.Editor
{
    public class ProjectManager
    {
        public static Scene Current { get; set; }
        public static Scene Backup { get; set; }
        public static List<Type> Types { get; protected set; }
        public static List<string> Scenes { get; private set; } = new List<string>();
        public static bool ProjectLoaded = false;


        private static string _file;
        public static bool ChangeGrid = false;
        public static string FileName
        {
            get
            {
                return _file;
            }
            set
            {
                _file = value;
                string invalid = new string(Path.GetInvalidFileNameChars());

                foreach (char c in invalid)
                {
                    _file = _file.Replace(c.ToString(), "");
                }
            }
        }

        public static Version Version = new Version(1, 0, 0, 0);

        public static bool Unsaved { get; set; }

        private static string _projectPath;
        public static string ProjectPath
        {
            get
            {
                return _projectPath;
            }
            set
            {
                _projectPath = value;
                string invalid = new string(Path.GetInvalidPathChars());

                foreach (char c in invalid)
                {
                    _projectPath = _projectPath.Replace(c.ToString(), "");
                }
            }
        }

        private static string _levelPath;
        public static string LevelName
        {
            get
            {
                return _levelPath;
            }
            set
            {
                _levelPath = value;
                string invalid = new string(Path.GetInvalidPathChars());

                foreach (char c in invalid)
                {
                    _levelPath = _levelPath.Replace(c.ToString(), "");
                }
            }
        }

        public const string ProjectExtension = ".catalyst";
        public const string LevelExtension = ".level";

        
        public static void CreateNewProject(string name, string path, bool createFolder)
        {
            ProjectPath = path;
            FileName = name;

            if (!Directory.Exists(ProjectPath))
                throw new DirectoryNotFoundException($"Path \"{path}\" is not a valid directory.");

            if (createFolder)
            {
                Directory.CreateDirectory(Path.Combine(ProjectPath, FileName));
                ProjectPath = Path.Combine(ProjectPath, FileName);
            }
                

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            using (JsonWriter w = new JsonTextWriter(sw))
            {
                w.Formatting = Formatting.Indented;
                w.WriteStartObject();
                w.WritePropertyName("Name");
                w.WriteValue(FileName);
                w.WritePropertyName("Version");
                w.WriteValue(Version.ToString());
                w.WriteEndObject();
            }

            File.WriteAllText(Path.Combine(ProjectPath, FileName + ProjectExtension), sb.ToString());
            Directory.CreateDirectory(Path.Combine(ProjectPath, "Content"));
            Directory.CreateDirectory(Path.Combine(ProjectPath, "Content", "Audio"));
            Directory.CreateDirectory(Path.Combine(ProjectPath, "Content", "Dialog"));
            Directory.CreateDirectory(Path.Combine(ProjectPath, "Content", "Graphics"));
            Directory.CreateDirectory(Path.Combine(ProjectPath, "Content", "Graphics", "Atlases"));
            Directory.CreateDirectory(Path.Combine(ProjectPath, "Content", "Graphics", "Effects"));
            Directory.CreateDirectory(Path.Combine(ProjectPath, "Content", "Levels"));
            Directory.CreateDirectory(Path.Combine(ProjectPath, "Content", "Textures"));

        }


        public static void CreateNewLevel(string file)
        {
            Current = new Scene(1920, 1080);
            Current.Systems.Add(new InputSystem(Current));
            Current.Systems.Add(new SpriteRenderer(Current));
            Current.Systems.Add(new ParticleSystem(Current));
            Current.Systems.Add(new CameraSystem(Current));

            RefreshTypes();

            string valid = GenerateClassName(file);

            FileName = valid;
            Unsaved = true;

            ProjectLoaded = true;
            ChangeGrid = true;

            /**

            //Create VS 2019 solution
            ZipFile.ExtractToDirectory("catalyst_project_template.zip", ProjectPath);

            File.Move(Path.Combine(ProjectPath, "catalyst_project_template.sln"), Path.Combine(ProjectPath, string.Concat(valid, ".sln")));
            File.Copy(Path.Combine(CatalystEditor.AssemblyDirectory, "Catalyst.Engine.dll"), Path.Combine(ProjectPath, "catalyst_project_template.Game", "Catalyst.Engine.dll"));
            File.Copy(Path.Combine(CatalystEditor.AssemblyDirectory, "Catalyst.Engine.dll"), Path.Combine(ProjectPath, "catalyst_project_template.Source", "Catalyst.Engine.dll"));

            Directory.Move(Path.Combine(ProjectPath, "catalyst_project_template.Game"), Path.Combine(ProjectPath, string.Concat(valid, ".Game")));
            Directory.Move(Path.Combine(ProjectPath, "catalyst_project_template.Source"), Path.Combine(ProjectPath, string.Concat(valid, ".Source")));

            string text = File.ReadAllText(Path.Combine(ProjectPath, string.Concat(valid, ".sln")));
            text = text.Replace("catalyst_project_template", valid);
            File.WriteAllText(Path.Combine(ProjectPath, string.Concat(valid, ".sln")), text);

            File.Move(Path.Combine(ProjectPath, string.Concat(valid, ".Game"), "catalyst_project_template.Game.csproj"), Path.Combine(ProjectPath, string.Concat(valid, ".Game"), string.Concat(valid, ".Game.csproj")));
            File.Move(Path.Combine(ProjectPath, string.Concat(valid, ".Source"), "catalyst_project_template.Source.csproj"), Path.Combine(ProjectPath, string.Concat(valid, ".Source"), string.Concat(valid, ".Source.csproj")));

            text = File.ReadAllText(Path.Combine(ProjectPath, string.Concat(valid, ".Game"), string.Concat(valid, ".Game.csproj")));
            text = text.Replace("catalyst_project_template", valid);
            File.WriteAllText(Path.Combine(ProjectPath, string.Concat(valid, ".Game"), string.Concat(valid, ".Game.csproj")), text);

            text = File.ReadAllText(Path.Combine(ProjectPath, string.Concat(valid, ".Source"), string.Concat(valid, ".Source.csproj")));
            text = text.Replace("catalyst_project_template", valid);
            File.WriteAllText(Path.Combine(ProjectPath, string.Concat(valid, ".Source"), string.Concat(valid, ".Source.csproj")), text);

            text = File.ReadAllText(Path.Combine(ProjectPath, string.Concat(valid, ".Game"), "Program.cs"));
            text = text.Replace("catalyst_project_template", valid);
            File.WriteAllText(Path.Combine(ProjectPath, string.Concat(valid, ".Game"), "Program.cs"), text);

            //Create project watching
            FileSystemWatcher watcher = new FileSystemWatcher();

            dll_directory = Path.Combine(ProjectPath, string.Concat(valid, ".Source"), "bin");
            dll_file_debug = Path.Combine(dll_directory, "Debug", string.Concat(valid, ".Source", ".dll"));
            dll_file_release = Path.Combine(dll_directory, "Release", string.Concat(valid, ".Source", ".dll"));
            dll_name = string.Concat(valid, ".Source", ".dll");
            Console.WriteLine($"Running from: {Environment.CurrentDirectory}");
            Console.WriteLine($"Sources from: {dll_directory}");

            if (!Directory.Exists(dll_directory))
            {
                Directory.CreateDirectory(dll_directory);
            }

            watcher.Path = dll_directory;
            watcher.Changed += OnChanged;
            watcher.EnableRaisingEvents = true;

            */

            SaveLevel();
        }

        public static void SaveLevel()
        {
            if (Current != null)
            {
                Serializer.SerializeToFile<Scene>(Current, Path.Combine(ProjectPath, "Content", "Levels", LevelName), SerializationMode.Binary);

                Unsaved = false;
            }

        }


        public static bool OpenProject(string path)
        {
            if (!File.Exists(path) || Path.GetExtension(path) != ProjectExtension)
                return false;

            string dirPath = Path.GetDirectoryName(path);

            _ = Directory.CreateDirectory(Path.Combine(dirPath, "Content"));
            _ = Directory.CreateDirectory(Path.Combine(dirPath, "Content", "Audio"));
            _ = Directory.CreateDirectory(Path.Combine(dirPath, "Content", "Dialog"));
            _ = Directory.CreateDirectory(Path.Combine(dirPath, "Content", "Graphics"));
            _ = Directory.CreateDirectory(Path.Combine(dirPath, "Content", "Graphics", "Atlases"));
            _ = Directory.CreateDirectory(Path.Combine(dirPath, "Content", "Graphics", "Effects"));
            _ = Directory.CreateDirectory(Path.Combine(dirPath, "Content", "Levels"));
            _ = Directory.CreateDirectory(Path.Combine(dirPath, "Content", "Textures"));

            ProjectPath = dirPath;
            FileName = Path.GetFileName(path);

            Scenes.Clear();

            

            string[] directoryFiles = Directory.GetFiles(Path.Combine(dirPath, "Content", "Levels"));

            foreach (string s in directoryFiles)
            {
                if (Path.GetExtension(s) == LevelExtension)
                {
                    Scenes.Add(s);
                    Console.WriteLine(s);
                }
            }

            Viewport.Playing = false;

            return true;
        }

        public static void OpenLevel(string path)
        {

            Current = Serializer.DeserializeFromFile<Scene>(path, SerializationMode.Binary);
            LevelName = Path.GetFileName(path);

            Unsaved = true;
            ChangeGrid = true;

            Viewport.Playing = false;
        }


        public static void RefreshTypes()
        {
            Types = Assembly.GetAssembly(typeof(Component)).GetTypes()
            .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Component))).ToList<Type>();

           foreach (Type type in
           Assembly.GetAssembly(typeof(Component)).GetTypes()
           .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Component))))
            {
                if (!Types.Contains(type))
                {
                    Types.Add(type);
                }
            }

            foreach (Assembly ass in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in ass.GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Component))))
                {
                    if (!Types.Contains(type))
                    {
                        Types.Add(type);
                    }
                }
            }
        }

        public static string RemoveInvalidChars(string filename)
        {
            return string.Concat(filename.Split(Path.GetInvalidFileNameChars()));
        }

        private static string GenerateClassName(string value)
        {
            string className = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value);

            // File name contains invalid chars, remove them
            Regex regex = new Regex(@"[^\p{Ll}\p{Lu}\p{Lt}\p{Lo}\p{Nd}\p{Nl}\p{Mn}\p{Mc}\p{Cf}\p{Pc}\p{Lm}]");
            className = regex.Replace(className, "");

            // Class name doesn't begin with a letter, insert an underscore
            if (!char.IsLetter(className, 0))
            {
                className = className.Insert(0, "_");
            }

            return className.Replace(" ", string.Empty);
        }
    }

}
