﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chroma.Engine.Graphics;
using Microsoft.Xna.Framework;

namespace Chroma.Engine.Physics
{
    [Serializable]
    public class MovementSystem : ASystem
    {

        private QuadTree quad;
        public MovementSystem(Scene scene) : base(scene)
        {
            quad = new QuadTree(new Rectangle(0, 0, Global.Width, Global.Height));
        }

        public override void Update(GameTime gameTime)
        {
            MoveActors(gameTime);
        }

        private void MoveActors(GameTime gameTime)
        {
            foreach (CActor actor in Manager.GetComponents<CActor>().Values)
            {
                    MoveX(actor);
                    MoveY(actor);
            }
        }


        private void MoveX(CActor actor)
        {
            CTransform t = actor.Entity.GetComponent<CTransform>();
            CVelocity v = actor.Entity.GetComponent<CVelocity>(); 
            int move = (int)Math.Round(v.Velocity.X);
            if (move != 0)
            {
                v.Velocity = new Vector2(v.Velocity.X - move, v.Velocity.Y);
                int sign = Math.Sign(move);


                while (move != 0)
                {
                    CTransform collision = ActorColliding(t, new Vector2(sign, 0));
                    if (collision == null)
                    {
                        //No solid immediately beside us
                        t.Position += new Vector2(sign, 0);
                        move -= sign;
                    }
                    else
                    {
                        if (collision.CollisionAction != null)
                        {
                            collision.CollisionAction.Invoke();
                        }
                        break;
                    }
                }
            }
        }

        private void MoveY(CActor actor)
        {
            CTransform t = actor.Entity.GetComponent<CTransform>();
            CVelocity v = actor.Entity.GetComponent<CVelocity>();
            int move = (int)Math.Round(v.Velocity.Y);
            if (move != 0)
            {
                v.Velocity = new Vector2(v.Velocity.X, v.Velocity.Y - move);
                int sign = Math.Sign(move);


                while (move != 0)
                {
                    CTransform collision = ActorColliding(t, new Vector2(0, sign));
                    if (collision == null)
                    {
                        //No solid immediately beside us
                        t.Position += new Vector2(0, sign);
                        move -= sign;
                    }
                    else
                    {
                        if (collision.CollisionAction != null)
                        {
                            collision.CollisionAction.Invoke();
                        }
                        break;
                    }
                }
            }
        }

        //Uses AABB Algorithm -- Needs working QuadTree to increase efficiency
        private CTransform ActorColliding(CTransform actor, Vector2 offset)
        {
            if (actor == null) { return null; }
            
            /*
            quad.Clear();
     
            foreach (CSolid solid in scene.Manager.GetComponents<CSolid>().Values)
            {
                if (solid.Entity.HasComponent<CTransform>())
                {
                    quad.Insert(solid.Entity.GetComponent<CTransform>());
                }   
            }
            
            List<CTransform> returnObjects = new List<CTransform>();
            quad.Retrieve(returnObjects, actor.Entity.GetComponent<CTransform>());

            foreach (CTransform solid in returnObjects)
            {

                Vector2 actorOffset = actor.Position + actor.CollisionOffset + offset - actor.Origin;
                Vector2 solidOffset = solid.Position + solid.CollisionOffset - solid.Origin;

                if (actorOffset.X < solidOffset.X + solid.CollisionDims.X &&
                    actorOffset.X + actor.CollisionDims.X > solidOffset.X &&
                    actorOffset.Y < solidOffset.Y + solid.CollisionDims.Y &&
                    actorOffset.Y + actor.CollisionDims.Y > solidOffset.Y)
                {
                    return solid;
                }
                

            }
            return null;
            */
            
            foreach (CSolid s in scene.Manager.GetComponents<CSolid>().Values)
            {
                if (s.Entity.HasComponent<CTransform>())
                {
                    CTransform solid = s.Entity.GetComponent<CTransform>();

                    Vector2 actorOffset = actor.Position + actor.CollisionOffset + offset - actor.Origin;
                    Vector2 solidOffset = solid.Position + solid.CollisionOffset - solid.Origin;

                    if (actorOffset.X < solidOffset.X + solid.CollisionDims.X &&
                        actorOffset.X + actor.CollisionDims.X > solidOffset.X &&
                        actorOffset.Y < solidOffset.Y + solid.CollisionDims.Y &&
                        actorOffset.Y + actor.CollisionDims.Y > solidOffset.Y)
                    {
                        return solid;
                    }
                }

            }
            return null;
            

        }



    }
}
