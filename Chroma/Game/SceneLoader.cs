using Chroma.Engine;
using Chroma.Engine.Graphics;
using Chroma.Engine.Input;
using Chroma.Engine.Physics;
using Chroma.Engine.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vector2 = Chroma.Engine.Utilities.Vector2;
using Color = Chroma.Engine.Utilities.Color;

namespace Chroma.Game
{
    public static class SceneLoader
    {
        public static World LoadScene(string ContentDirectory)
        {
            World world = new World();

            Scene scene = new Scene(Global.Width*2, Global.Height*2);
            scene.Systems.Add(new InputSystem(scene));
            scene.Systems.Add(new PlayerSystem(scene));
            scene.Systems.Add(new GravitySystem(scene));
            scene.Systems.Add(new MovementSystem(scene));
            scene.Systems.Add(new SpriteRenderSystem(scene));
            scene.Systems.Add(new ParticleSystem(scene));
            scene.Systems.Add(new CameraSystem(scene));


            var Content = Chroma.Engine.ChromaGame.Instance.Content;

            Texture2D[] atlas = new Texture2D[] {
                    Content.Load<Texture2D>(ContentDirectory + "/Sprites/Player/s_player_stationary/s_player_stationary_1"),
                    Content.Load<Texture2D>(ContentDirectory + "/Sprites/Player/s_player_stationary/s_player_stationary_2"),
                    Content.Load<Texture2D>(ContentDirectory + "/Sprites/Player/s_player_stationary/s_player_stationary_3"),
                    Content.Load<Texture2D>(ContentDirectory + "/Sprites/Player/s_player_stationary/s_player_stationary_4"),
                    Content.Load<Texture2D>(ContentDirectory + "/Sprites/Player/s_player_stationary/s_player_stationary_5"),
                    Content.Load<Texture2D>(ContentDirectory + "/Sprites/Player/s_player_stationary/s_player_stationary_6"),
                    Content.Load<Texture2D>(ContentDirectory + "/Sprites/Player/s_player_stationary/s_player_stationary_7") };
            Entity testEntity = scene.Manager.NewEntity();
            var sprite = new CSprite(testEntity, "test", 0, 0, atlas, Origin.TopLeft) { animationSpeed = 6.0f, layer = 1.0f };
            testEntity.AddComponent<CSprite>(sprite);
            testEntity.AddComponent<CPlayer>();

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
            var sprite2 = new CSprite(testEntity2, "test", 0, 200, atlas2, Origin.TopLeft) { animationSpeed = 6.0f };
            testEntity2.AddComponent<CSprite>(sprite2);
            testEntity2.AddComponent<CSolid>();

            world.CurrentScene = scene;

            for (int i = 0; i < 100; i++)
            {
                Entity grass = scene.Manager.NewEntity();
                var grass_sprite = new CSprite(grass, "grass_"+i, 16+32*i, 344+Global.Height, new Texture2D[] { Content.Load<Texture2D>(ContentDirectory + "/Sprites/Tiles/Grass/grass_top") }, Origin.Center);
                grass.GetComponent<CTransform>().CollisionDims = new Vector2(32, 32);
                grass.GetComponent<CTransform>().CollisionOffset = new Vector2(3, 3);
                grass.AddComponent<CSprite>(grass_sprite);
                grass.AddComponent<CSolid>();
            }

            Entity p = scene.Manager.NewEntity();
            p.AddComponent<CParticleEmitter>();
            p.GetComponent<CParticleEmitter>().Follow = testEntity;
            p.GetComponent<CParticleEmitter>().Position = new Vector2(scene.Width, scene.Height/2);
            p.GetComponent<CParticleEmitter>().Offset = new Vector2(30, 10);
            p.GetComponent<CParticleEmitter>().Launch();
            p.GetComponent<CParticleEmitter>().Launch();
            p.GetComponent<CParticleEmitter>().Launch();
            p.GetComponent<CParticleEmitter>().Launch();
            p.GetComponent<CParticleEmitter>().Launch();

            p.GetComponent<CParticleEmitter>().Texture = BasicShapes.GenerateCircleTexture(1, Color.White, 1.0f);

            return world;
        }
    }
}
