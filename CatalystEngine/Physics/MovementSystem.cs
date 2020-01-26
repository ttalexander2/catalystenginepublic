using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Catalyst.Engine.Rendering;
using Microsoft.Xna.Framework;

namespace Catalyst.Engine.Physics
{
    [Serializable]
    public class MovementSystem : ASystem
    {

        private QuadTree quad;
        public MovementSystem(Scene scene) : base(scene)
        {
            quad = new QuadTree(new Utilities.Rectangle(0, 0, Graphics.Width, Graphics.Height));
        }

        public override void Update(GameTime gameTime)
        {
            MoveActors(gameTime);
        }

        private void MoveActors(GameTime gameTime)
        {
            foreach (Actor actor in Manager.GetComponents<Actor>().Values)
            {
                    MoveX(actor);
                    MoveY(actor);
            }
        }


        private void MoveX(Actor actor)
        {
            Transform t = actor.Entity.GetComponent<Transform>();
            Velocity v = actor.Entity.GetComponent<Velocity>(); 
            int move = (int)Math.Round(v.V.X);
            if (move != 0)
            {
                v.V = new Utilities.Vector2(v.V.X - move, v.V.Y);
                int sign = Math.Sign(move);


                while (move != 0)
                {
                    Transform collision = ActorColliding(t, new Utilities.Vector2(sign, 0));
                    if (collision == null)
                    {
                        //No solid immediately beside us
                        t.Position += new Utilities.Vector2(sign, 0);
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

        private void MoveY(Actor actor)
        {
            Transform t = actor.Entity.GetComponent<Transform>();
            Velocity v = actor.Entity.GetComponent<Velocity>();
            int move = (int)Math.Round(v.V.Y);
            if (move != 0)
            {
                v.V = new Utilities.Vector2(v.V.X, v.V.Y - move);
                int sign = Math.Sign(move);


                while (move != 0)
                {
                    Transform collision = ActorColliding(t, new Utilities.Vector2(0, sign));
                    if (collision == null)
                    {
                        //No solid immediately beside us
                        t.Position += new Utilities.Vector2(0, sign);
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
        private Transform ActorColliding(Transform actor, Utilities.Vector2 offset)
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

                Utilities.Vector2 actorOffset = actor.Position + actor.CollisionOffset + offset - actor.Origin;
                Utilities.Vector2 solidOffset = solid.Position + solid.CollisionOffset - solid.Origin;

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
            
            foreach (Solid s in scene.Manager.GetComponents<Solid>().Values)
            {
                if (s.Entity.HasComponent<Transform>())
                {
                    Transform solid = s.Entity.GetComponent<Transform>();

                    Utilities.Vector2 actorOffset = actor.Position + actor.CollisionOffset + offset - actor.Origin;
                    Utilities.Vector2 solidOffset = solid.Position + solid.CollisionOffset - solid.Origin;

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
