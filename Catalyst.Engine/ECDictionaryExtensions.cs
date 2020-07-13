using Catalyst.Engine.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalyst.Engine
{
    public static class ECDictionaryExtensions
    {
        public static Dictionary<Entity, Component> Filter<A>(this Dictionary<Entity, Component> components) where A : Component
        {
            Dictionary<Entity, Component> returnDict = new Dictionary<Entity, Component>(components);
            foreach (Entity entity in returnDict.Keys)
            {
                if (!entity.HasComponent<A>())
                {
                    returnDict.Remove(entity);
                }
            }

            return returnDict;
        }

        public static Dictionary<Entity, Component> Filter<A, B>(this Dictionary<Entity, Component> components) where A : Component where B : Component
        {
            Dictionary<Entity, Component> returnDict = new Dictionary<Entity, Component>(components);
            foreach (Entity entity in returnDict.Keys)
            {
                if (!entity.HasComponent<A>() || !entity.HasComponent<B>())
                {
                    returnDict.Remove(entity);
                }
            }

            return returnDict;
        }

        public static Dictionary<Entity, Component> Filter<A, B, C>(this Dictionary<Entity, Component> components) where A : Component where B : Component where C  : Component
        {
            Dictionary<Entity, Component> returnDict = new Dictionary<Entity, Component>(components);
            foreach (Entity entity in returnDict.Keys)
            {
                if (!entity.HasComponent<A>() || !entity.HasComponent<B>() || !entity.HasComponent<C>())
                {
                    returnDict.Remove(entity);
                }
            }

            return returnDict;
        }

        public static Dictionary<Entity, Component> Filter<A, B, C, D>(this Dictionary<Entity, Component> components) where A : Component where B : Component where C : Component where D : Component
        {
            Dictionary<Entity, Component> returnDict = new Dictionary<Entity, Component>(components);
            foreach (Entity entity in returnDict.Keys)
            {
                if (!entity.HasComponent<A>() || !entity.HasComponent<B>() || !entity.HasComponent<C>() || !entity.HasComponent<D>())
                {
                    returnDict.Remove(entity);
                }
            }

            return returnDict;
        }

        public static Dictionary<Entity, Component> Filter<A, B, C, D, E>(this Dictionary<Entity, Component> components) where A : Component where B : Component where C : Component where D : Component where E : Component
        {
            Dictionary<Entity, Component> returnDict = new Dictionary<Entity, Component>(components);
            foreach (Entity entity in returnDict.Keys)
            {
                if (!entity.HasComponent<A>() || !entity.HasComponent<B>() || !entity.HasComponent<C>() || !entity.HasComponent<D>() || !entity.HasComponent<E>())
                {
                    returnDict.Remove(entity);
                }
            }

            return returnDict;
        }

        public static Dictionary<Entity, Component> Filter<A, B, C, D, E, F>(this Dictionary<Entity, Component> components) where A : Component where B : Component where C : Component where D : Component where E : Component where F : Component
        {
            Dictionary<Entity, Component> returnDict = new Dictionary<Entity, Component>(components);
            foreach (Entity entity in returnDict.Keys)
            {
                if (!entity.HasComponent<A>() || !entity.HasComponent<B>() || !entity.HasComponent<C>() || !entity.HasComponent<D>() || !entity.HasComponent<E>() || !entity.HasComponent<F>())
                {
                    returnDict.Remove(entity);
                }
            }

            return returnDict;
        }

        public static Dictionary<Entity, Component> Filter<A, B, C, D, E, F, G>(this Dictionary<Entity, Component> components) where A : Component where B : Component where C : Component where D : Component where E : Component where F : Component where G : Component
        {
            Dictionary<Entity, Component> returnDict = new Dictionary<Entity, Component>(components);
            foreach (Entity entity in returnDict.Keys)
            {
                if (!entity.HasComponent<A>() || !entity.HasComponent<B>() || !entity.HasComponent<C>() || !entity.HasComponent<D>() || !entity.HasComponent<E>() || !entity.HasComponent<F>() || !entity.HasComponent<G>())
                {
                    returnDict.Remove(entity);
                }
            }

            return returnDict;
        }

        public static Dictionary<int, Entity> Filter<A>(this Dictionary<int, Entity> entities) where A : Component
        {
            Dictionary<int, Entity> returnDict = new Dictionary<int, Entity>(entities);
            foreach (Entity entity in returnDict.Values)
            {
                if (!entity.HasComponent<A>())
                {
                    returnDict.Remove(entity.UID);
                }
            }

            return returnDict;
        }

        public static Dictionary<int, Entity> Filter<A, B>(this Dictionary<int, Entity> entities) where A : Component where B : Component
        {
            Dictionary<int, Entity> returnDict = new Dictionary<int, Entity>(entities);
            foreach (Entity entity in returnDict.Values)
            {
                if (!entity.HasComponent<A>() || !entity.HasComponent<B>())
                {
                    returnDict.Remove(entity.UID);
                }
            }

            return returnDict;
        }

        public static Dictionary<int, Entity> Filter<A, B, C>(this Dictionary<int, Entity> entities) where A : Component where B : Component where C : Component
        {
            Dictionary<int, Entity> returnDict = new Dictionary<int, Entity>(entities);
            foreach (Entity entity in returnDict.Values)
            {
                if (!entity.HasComponent<A>() || !entity.HasComponent<B>() || !entity.HasComponent<C>())
                {
                    returnDict.Remove(entity.UID);
                }
            }

            return returnDict;
        }

        public static Dictionary<int, Entity> Filter<A, B, C, D>(this Dictionary<int, Entity> entities) where A : Component where B : Component where C : Component where D : Component
        {
            Dictionary<int, Entity> returnDict = new Dictionary<int, Entity>(entities);
            foreach (Entity entity in returnDict.Values)
            {
                if (!entity.HasComponent<A>() || !entity.HasComponent<B>() || !entity.HasComponent<C>() || !entity.HasComponent<D>())
                {
                    returnDict.Remove(entity.UID);
                }
            }

            return returnDict;
        }

        public static Dictionary<int, Entity> Filter<A, B, C, D, E>(this Dictionary<int, Entity> entities) where A : Component where B : Component where C : Component where D : Component where E : Component
        {
            Dictionary<int, Entity> returnDict = new Dictionary<int, Entity>(entities);
            foreach (Entity entity in returnDict.Values)
            {
                if (!entity.HasComponent<A>() || !entity.HasComponent<B>() || !entity.HasComponent<C>() || !entity.HasComponent<D>() || !entity.HasComponent<E>())
                {
                    returnDict.Remove(entity.UID);
                }
            }

            return returnDict;
        }

        public static Dictionary<int, Entity> Filter<A, B, C, D, E, F>(this Dictionary<int, Entity> entities) where A : Component where B : Component where C : Component where D : Component where E : Component where F : Component
        {
            Dictionary<int, Entity> returnDict = new Dictionary<int, Entity>(entities);
            foreach (Entity entity in returnDict.Values)
            {
                if (!entity.HasComponent<A>() || !entity.HasComponent<B>() || !entity.HasComponent<C>() || !entity.HasComponent<D>() || !entity.HasComponent<E>() || !entity.HasComponent<F>())
                {
                    returnDict.Remove(entity.UID);
                }
            }

            return returnDict;
        }

        public static Dictionary<int, Entity> Filter<A, B, C, D, E, F, G>(this Dictionary<int, Entity> entities) where A : Component where B : Component where C : Component where D : Component where E : Component where F : Component where G : Component
        {
            Dictionary<int, Entity> returnDict = new Dictionary<int, Entity>(entities);
            foreach (Entity entity in returnDict.Values)
            {
                if (!entity.HasComponent<A>() || !entity.HasComponent<B>() || !entity.HasComponent<C>() || !entity.HasComponent<D>() || !entity.HasComponent<E>() || !entity.HasComponent<F>() || !entity.HasComponent<G>())
                {
                    returnDict.Remove(entity.UID);
                }
            }

            return returnDict;
        }
    }
}
