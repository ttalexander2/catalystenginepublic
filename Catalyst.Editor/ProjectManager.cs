using Catalyst.Engine;
using Catalyst.Engine.Rendering;
using Catalyst.Engine.Input;
using Catalyst.Engine.Physics;
using Catalyst.Engine.Utilities;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Vector2 = Catalyst.Engine.Utilities.Vector2;
using System.Diagnostics;
using System.Threading.Tasks;
using CatalystEditor;
using static Catalyst.Engine.Rendering.Sprite;
using Catalyst.GameLogic;

namespace Catalyst.Editor
{
    public class ProjectManager
    {

        public static Scene Current { get; set; }
        public static Scene Backup { get; set; }
        public static List<Type> Types { get; protected set; }
        public static bool scene_loaded = false;

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

        public const string Extension = ".chroma";

        public static void CreateNew(string file)
        {
            Current = new Scene(1920, 1080);
            Current.Systems.Add(new InputSystem(Current));
            Current.Systems.Add(new PlayerSystem(Current));
            Current.Systems.Add(new CollisionSystem(Current));
            //Current.Systems.Add(new MovementSystem(Current));
            Current.Systems.Add(new SpriteRenderer(Current));
            Current.Systems.Add(new ParticleSystem(Current));
            Current.Systems.Add(new CameraSystem(Current));

            //RefreshTypes();

            FileName = file;
            Unsaved = true;

            scene_loaded = true;
            ChangeGrid = true;
        }

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

        public static Scene LoadTestWorld()
        {
            Scene scene = new Scene(Graphics.Width * 8, Graphics.Height*3);
            scene.Systems.Add(new InputSystem(scene));
            //scene.Systems.Add(new MovementSystem(scene));
            scene.Systems.Add(new SpriteRenderer(scene));
            scene.Systems.Add(new ParticleSystem(scene));
            scene.Systems.Add(new CameraSystem(scene));

            Atlas atlas = TexturePacker.AtlasFromBinary(Path.Combine(CatalystEditor.AssemblyDirectory, "Content", "Atlases", "test.atlas"), Path.Combine(CatalystEditor.AssemblyDirectory, "Content", "Atlases", "test.meta"));



            Entity testEntity = scene.Manager.NewEntity();
            var sprite = new Sprite(testEntity, atlas.Textures[0]);
            testEntity.AddComponent<Sprite>(sprite);

            scene.Camera.Following = testEntity;


            Entity testEntity2 = scene.Manager.NewEntity();
            var sprite2 = new Sprite(testEntity2, atlas.Textures[0]);
            testEntity2.AddComponent<Sprite>(sprite2);
            //testEntity2.AddComponent<Solid>();

            ChangeGrid = true;

            return scene;
        }
    }
    
}
