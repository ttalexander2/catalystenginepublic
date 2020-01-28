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
using Catalyst.Game;
using Microsoft.Build;
using Microsoft.Build.Construction;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Catalyst.XNA
{
    public class ProjectManager
    {

        public static Scene Current { get; set; }
        public static List<Type> Types { get; protected set; }
        public static bool scene_loaded = false;

        private static string _file;
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
            Current.Systems.Add(new GravitySystem(Current));
            Current.Systems.Add(new MovementSystem(Current));
            Current.Systems.Add(new SpriteRenderSystem(Current));
            Current.Systems.Add(new ParticleSystem(Current));
            Current.Systems.Add(new CameraSystem(Current));

            FileName = file;
            Unsaved = true;

            ProjectManager.scene_loaded = true;
        }

        public static void Save()
        {
            Directory.CreateDirectory(ProjectPath);

            CatalystSerializer.SerializeToFile<Scene>(Current, Path.Combine(ProjectPath, "Content", "Scenes", FileName + Extension), SerializationMode.Binary);

            Unsaved = false;
        }

        public static void Open(string path)
        {

            Current = CatalystSerializer.DeserializeFromFile<Scene>(path, SerializationMode.Binary);
            ProjectPath = Path.GetDirectoryName(path);
            FileName = Path.GetFileNameWithoutExtension(path);

            Unsaved = true;
            scene_loaded = true;
        }

        public static Scene Load()
        {
            return CatalystSerializer.DeserializeFromFile<Scene>(Path.Combine(ProjectPath, FileName + Extension), SerializationMode.Binary);
        }

        public static void RefreshTypes()
        {
            Types = Assembly.GetAssembly(typeof(AComponent)).GetTypes()
            .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(AComponent))).ToList<Type>();
        }

        public static Scene LoadTestWorld()
        {
            Scene scene = new Scene(Graphics.Width * 2, Graphics.Height * 2);
            scene.Systems.Add(new InputSystem(scene));
            scene.Systems.Add(new PlayerSystem(scene));
            scene.Systems.Add(new GravitySystem(scene));
            scene.Systems.Add(new MovementSystem(scene));
            scene.Systems.Add(new SpriteRenderSystem(scene));
            scene.Systems.Add(new ParticleSystem(scene));
            scene.Systems.Add(new CameraSystem(scene));

            Entity testEntity = scene.Manager.NewEntity();
            var sprite = new Sprite(testEntity, "test", 0, 0, new Engine.Rendering.MTexture[] { }, Origin.TopLeft) { AnimationSpeed = 6.0f, Layer = 1.0f };
            testEntity.AddComponent<Sprite>(sprite);
            testEntity.AddComponent<Player>();

            scene.Camera.Following = testEntity;


            Entity testEntity2 = scene.Manager.NewEntity();
            var sprite2 = new Sprite(testEntity2, "test", 0, 200, new Engine.Rendering.MTexture[] { }, Origin.TopLeft) { AnimationSpeed = 6.0f };
            testEntity2.AddComponent<Sprite>(sprite2);
            testEntity2.AddComponent<Solid>();

            for (int i = 0; i < 100; i++)
            {
                Entity grass = scene.Manager.NewEntity();
                var grass_sprite = new Sprite(grass, "grass_" + i, 16 + 32 * i, 344 + Graphics.Height, new Engine.Rendering.MTexture[] { }, Origin.Center);
                grass.GetComponent<Transform>().CollisionDims = new Vector2(32, 32);
                grass.GetComponent<Transform>().CollisionOffset = new Vector2(3, 3);
                grass.AddComponent<Sprite>(grass_sprite);
                grass.AddComponent<Solid>();
            }

            return scene;
        }
    }
    
}
