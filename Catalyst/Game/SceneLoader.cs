using Catalyst.Engine;
using Catalyst.Engine.Rendering;
using Catalyst.Engine.Input;
using Catalyst.Engine.Physics;
using Catalyst.Engine.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vector2 = Catalyst.Engine.Utilities.Vector2;
using Color = Catalyst.Engine.Utilities.Color;

namespace Catalyst.Game
{
    public static class SceneLoader
    {
        public static World LoadScene(string ContentDirectory)
        {
            World world = new World();

            Scene scene = new Scene(Graphics.Width*2, Graphics.Height*2);
            scene.Systems.Add(new InputSystem(scene));
            scene.Systems.Add(new PlayerSystem(scene));
            scene.Systems.Add(new GravitySystem(scene));
            scene.Systems.Add(new MovementSystem(scene));
            scene.Systems.Add(new SpriteRenderSystem(scene));
            scene.Systems.Add(new ParticleSystem(scene));
            scene.Systems.Add(new CameraSystem(scene));


            var Content = Catalyst.Engine.CatalystEngine.Instance.Content;
            /**
            Texture2D[] atlas = new Texture2D[] {
                    Content.Load<Texture2D>(ContentDirectory + "/Sprites/Player/s_player_stationary/s_player_stationary_1"),
                    Content.Load<Texture2D>(ContentDirectory + "/Sprites/Player/s_player_stationary/s_player_stationary_2"),
                    Content.Load<Texture2D>(ContentDirectory + "/Sprites/Player/s_player_stationary/s_player_stationary_3"),
                    Content.Load<Texture2D>(ContentDirectory + "/Sprites/Player/s_player_stationary/s_player_stationary_4"),
                    Content.Load<Texture2D>(ContentDirectory + "/Sprites/Player/s_player_stationary/s_player_stationary_5"),
                    Content.Load<Texture2D>(ContentDirectory + "/Sprites/Player/s_player_stationary/s_player_stationary_6"),
                    Content.Load<Texture2D>(ContentDirectory + "/Sprites/Player/s_player_stationary/s_player_stationary_7") };
            Entity testEntity = scene.Manager.NewEntity();
            var sprite = new Sprite(testEntity, "test", 0, 0, atlas, Origin.TopLeft) { AnimationSpeed = 6.0f, Layer = 1.0f };
            testEntity.AddComponent<Sprite>(sprite);
            testEntity.AddComponent<Player>();

            scene.Camera.Following = testEntity;


            Texture2D[] atlas2 = new Texture2D[] {
                Content.Load<Texture2D>(ContentDirectory + "/Sprites/Player/s_player_stationary/s_player_stationary_1"),
                Content.Load<Texture2D>(ContentDirectory + "/Sprites/Player/s_player_stationary/s_player_stationary_2"),
                Content.Load<Texture2D>(ContentDirectory + "/Sprites/Player/s_player_stationary/s_player_stationary_3"),
                Content.Load<Texture2D>(ContentDirectory + "/Sprites/Player/s_player_stationary/s_player_stationary_4"),
                Content.Load<Texture2D>(ContentDirectory + "/Sprites/Player/s_player_stationary/s_player_stationary_5"),
                Content.Load<Texture2D>(ContentDirectory + "/Sprites/Player/s_player_stationary/s_player_stationary_6"),
                Content.Load<Texture2D>(ContentDirectory + "/Sprites/Player/s_player_stationary/s_player_stationary_7") };
            Entity testEntity2 = scene.Manager.NewEntity();
            var sprite2 = new Sprite(testEntity2, "test", 0, 200, atlas2, Origin.TopLeft) { AnimationSpeed = 6.0f };
            testEntity2.AddComponent<Sprite>(sprite2);
            testEntity2.AddComponent<Solid>();

            world.CurrentScene = scene;

            for (int i = 0; i < 100; i++)
            {
                Entity grass = scene.Manager.NewEntity();
                var grass_sprite = new Sprite(grass, "grass_"+i, 16+32*i, 344+Graphics.Height, new Texture2D[] { Content.Load<Texture2D>(ContentDirectory + "/Sprites/Tiles/Grass/grass_top") }, Origin.Center);
                grass.GetComponent<Transform>().CollisionDims = new Vector2(32, 32);
                grass.GetComponent<Transform>().CollisionOffset = new Vector2(3, 3);
                grass.AddComponent<Sprite>(grass_sprite);
                grass.AddComponent<Solid>();
            }

            Entity p = scene.Manager.NewEntity();
            p.AddComponent<ParticleEmitter>();
            p.GetComponent<ParticleEmitter>().Follow = testEntity;
            p.GetComponent<ParticleEmitter>().Position = new Vector2(scene.Width, scene.Height/2);
            p.GetComponent<ParticleEmitter>().Offset = new Vector2(30, 10);
            p.GetComponent<ParticleEmitter>().Launch();

            p.GetComponent<ParticleEmitter>().Texture = BasicShapes.GenerateCircleTexture(1, Color.White, 1.0f);
            **/

            return world;
        }
    }
}

