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

namespace Catalyst.Editor
{
    public class ProjectManager
    {

        public static Scene Current { get; set; }
        public static Scene Backup { get; set; }
        public static List<Type> Types { get; protected set; }
        public static bool scene_loaded = false;

        /**
        public static FileSystemWatcher systemWatcher;
        private static WeakReference _hostAlcWeakRef;
        private static SimpleUnloadableAssemblyLoadContext _context;
        private static string dll_directory;
        private static string dll_file_debug;
        private static string dll_file_release;
        private static string dll_name;
        */

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

        public static bool Unsaved { get; set; }

        private static string _path;
        public static string ProjectPath
        {
            get
            {
                return _path;
            }
            set
            {
                _path = value;
                string invalid = new string(Path.GetInvalidPathChars());

                foreach (char c in invalid)
                {
                    _path = _path.Replace(c.ToString(), "");
                }
            }
        }

        public const string Extension = ".bin";

        public static void CreateNew(string file)
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

            scene_loaded = true;
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

            Save();
        }

        /**

        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            ReloadAssembly();
        }

        public static void ReloadAssembly()
        {
            string filepath = dll_file_debug;
            if (!File.Exists(dll_file_debug))
            {
                if (!File.Exists(dll_file_release))
                {
                    Console.WriteLine(string.Format("Failed to find file: {0}", filepath));
                    return;
                }
                else
                {
                    filepath = dll_file_release;
                }
            }


            if (_hostAlcWeakRef != null && _hostAlcWeakRef.IsAlive)
            {
                _context.Unload();
                // Poll and run GC until the AssemblyLoadContext is unloaded.
                // You don't need to do that unless you want to know when the context
                // got unloaded. You can just leave it to the regular GC.

                for (int i = 0; _hostAlcWeakRef.IsAlive && (i < 10); i++)
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }

            try
            {

                if (!Directory.Exists(Path.Combine(CatalystEditor.AssemblyDirectory, "Runtime Compilations")))
                {
                    Console.WriteLine($"CREATING DIRECTORY: {Path.Combine(CatalystEditor.AssemblyDirectory, "Runtime Compilations")}");
                    Directory.CreateDirectory(Path.Combine(CatalystEditor.AssemblyDirectory, "Runtime Compilations"));
                }

                File.Copy(filepath, Path.Combine(CatalystEditor.AssemblyDirectory, "Runtime Compilations", dll_name), true);

                _context = new SimpleUnloadableAssemblyLoadContext();
                _ = _context.LoadFromAssemblyPath(Path.Combine(CatalystEditor.AssemblyDirectory, "Runtime Compilations", dll_name));

                _hostAlcWeakRef = new WeakReference(_context, trackResurrection: true);

                Console.WriteLine("Successfully loaded runtime assembly.");

                var result = AppDomain
                      .CurrentDomain
                      .GetAssemblies()
                      .SelectMany(asm => asm.GetModules())
                      .Select(module => $"{module.Name,-40} {module.Assembly.GetName().Version}");

            } 
            catch(Exception e)
            {

            }





            Current.Manager.RefreshTypes();
        }

        */

        public static void Save()
        {
            Directory.CreateDirectory(ProjectPath);

            Console.WriteLine(ProjectPath);

            Serializer.SerializeToFile<Scene>(Current, Path.Combine(ProjectPath, FileName + Extension), SerializationMode.Binary);

            Unsaved = false;
        }

        public static void Open(string path)
        {

            Current = Serializer.DeserializeFromFile<Scene>(path, SerializationMode.Binary);
            ProjectPath = Path.GetDirectoryName(path);
            FileName = Path.GetFileNameWithoutExtension(path);

            Unsaved = true;
            scene_loaded = true;
            ChangeGrid = true;
        }

        public static Scene Load()
        {
            return Serializer.DeserializeFromFile<Scene>(Path.Combine(ProjectPath, FileName + Extension), SerializationMode.Binary);
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

        public static Scene LoadTestWorld()
        {
            Scene scene = new Scene(Graphics.Width * 8, Graphics.Height*3);
            scene.Systems.Add(new InputSystem(scene));
            scene.Systems.Add(new SpriteRenderer(scene));
            scene.Systems.Add(new ParticleSystem(scene));
            scene.Systems.Add(new CameraSystem(scene));

            Atlas atlas = ContentManager.TexturePacker.AtlasFromBinary(Path.Combine(CatalystEditor.AssemblyDirectory, "Content", "Atlases", "test.atlas"), Path.Combine(CatalystEditor.AssemblyDirectory, "Content", "Atlases", "test.meta"));



            Entity testEntity = scene.Manager.NewEntity();
            var sprite = new Sprite(testEntity, atlas.Textures[0]);
            testEntity.AddComponent<Sprite>(sprite);

            scene.Camera.Following = testEntity;


            Entity testEntity2 = scene.Manager.NewEntity();
            var sprite2 = new Sprite(testEntity2, atlas.Textures[0]);
            testEntity2.AddComponent<Sprite>(sprite2);

            ChangeGrid = true;

            return scene;
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


    internal class SimpleUnloadableAssemblyLoadContext : AssemblyLoadContext
    {
        public SimpleUnloadableAssemblyLoadContext()
            : base(true)
        {
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            return null;
        }
    }

}
