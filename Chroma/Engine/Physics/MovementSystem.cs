using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Chroma.Engine.Physics
{
    public class MovementSystem : ASystem
    {
        public MovementSystem(Scene scene) : base(scene) { }

        private QuadTree quad = new QuadTree(new Rectangle(0, 0, (int)(Global.NativeWidth * Global.PixelScale), (int)(Global.NativeHeight * Global.PixelScale)));

        public override void Update(GameTime gameTime)
        {
            MoveActors(gameTime);
        }

        private void MoveActors(GameTime gameTime)
        {
            foreach (Entity e in Manager.GetEntities().Values)
            {
                if (e.HasComponent<CActor>() && e.HasComponent<CVelocity>() && e.HasComponent<CTransform>())
                {

                }
            }
        }


        private void MoveX(int UID, Action onCollide)
        {
            CTransform t = Manager.GetComponent<CTransform>(UID);
            CVelocity v = Manager.GetComponent<CVelocity>(UID);

            int move = (int)Math.Round(v.Vector.X);
            
            if (move != 0)
            {
                v.Vector = new Vector2(v.Vector.X - move, v.Vector.Y);
                int sign = Math.Sign(move);

                
                while (move != 0)
                {
                    if (!ChromaGame.Instance.world.CurrentScene.EntityCollision(this.UID, Position + new Vector2(sign, 0)))
                    {
                        //No solid immediately beside us
                        t.Position += new Vector2(sign, 0);
                        move -= sign;
                    }
                    else
                    {
                        //Hit a solid!
                        onCollide?.Invoke();
                        break;
                    }
                }
            }
        }

        private bool CheckActorCollisionWithSolids(int UID)
        {


            quad.Clear();
            for (int j = 0; j < layer.Sprites.Count; j++)
            {
                if (layer.Sprites[j].collidable)
                {
                    quad.insert(layer.Sprites[j]);
                }
            }
            List<Sprite> returnObjects = new List<Sprite>();
            for (int i = 0; i < _layers.Count; i++)
            {
                SceneLayer layer = _layers[i];
                if (layer.hasCollisions)
                {
                    for (int j = 0; j < layer.Sprites.Count; j++)
                    {
                        if (layer.Sprites[j].collidable)
                        {
                            returnObjects.Clear();
                            quad.retrieve(returnObjects, layer.Sprites[j]);

                            for (int x = 0; x < returnObjects.Count; x++)
                            {
                                //Check Collision
                            }
                        }
                    }
                }
            }



    }
}
