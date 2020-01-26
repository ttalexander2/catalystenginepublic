using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalyst.Engine
{
    public static class ECDictionaryExtensions
    {
        public static Dictionary<Entity, AComponent> Filter<A>(this Dictionary<Entity, AComponent> components) where A : AComponent
        {
            Dictionary<Entity, AComponent> returnDict = new Dictionary<Entity, AComponent>(components);
            foreach (Entity entity in returnDict.Keys)
            {
                if (!entity.HasComponent<A>())
                {
                    returnDict.Remove(entity);
                }
            }

            return returnDict;
        }

        public static Dictionary<Entity, AComponent> Filter<A, B>(this Dictionary<Entity, AComponent> components) where A : AComponent where B : AComponent
        {
            Dictionary<Entity, AComponent> returnDict = new Dictionary<Entity, AComponent>(components);
            foreach (Entity entity in returnDict.Keys)
            {
                if (!entity.HasComponent<A>() || !entity.HasComponent<B>())
                {
                    returnDict.Remove(entity);
                }
            }

            return returnDict;
        }

        public static Dictionary<Entity, AComponent> Filter<A, B, C>(this Dictionary<Entity, AComponent> components) where A : AComponent where B : AComponent where C  : AComponent
        {
            Dictionary<Entity, AComponent> returnDict = new Dictionary<Entity, AComponent>(components);
            foreach (Entity entity in returnDict.Keys)
            {
                if (!entity.HasComponent<A>() || !entity.HasComponent<B>() || !entity.HasComponent<C>())
                {
                    returnDict.Remove(entity);
                }
            }

            return returnDict;
        }

        public static Dictionary<Entity, AComponent> Filter<A, B, C, D>(this Dictionary<Entity, AComponent> components) where A : AComponent where B : AComponent where C : AComponent where D : AComponent
        {
            Dictionary<Entity, AComponent> returnDict = new Dictionary<Entity, AComponent>(components);
            foreach (Entity entity in returnDict.Keys)
            {
                if (!entity.HasComponent<A>() || !entity.HasComponent<B>() || !entity.HasComponent<C>() || !entity.HasComponent<D>())
                {
                    returnDict.Remove(entity);
                }
            }

            return returnDict;
        }

        public static Dictionary<Entity, AComponent> Filter<A, B, C, D, E>(this Dictionary<Entity, AComponent> components) where A : AComponent where B : AComponent where C : AComponent where D : AComponent where E : AComponent
        {
            Dictionary<Entity, AComponent> returnDict = new Dictionary<Entity, AComponent>(components);
            foreach (Entity entity in returnDict.Keys)
            {
                if (!entity.HasComponent<A>() || !entity.HasComponent<B>() || !entity.HasComponent<C>() || !entity.HasComponent<D>() || !entity.HasComponent<E>())
                {
                    returnDict.Remove(entity);
                }
            }

            return returnDict;
        }

        public static Dictionary<Entity, AComponent> Filter<A, B, C, D, E, F>(this Dictionary<Entity, AComponent> components) where A : AComponent where B : AComponent where C : AComponent where D : AComponent where E : AComponent where F : AComponent
        {
            Dictionary<Entity, AComponent> returnDict = new Dictionary<Entity, AComponent>(components);
            foreach (Entity entity in returnDict.Keys)
            {
                if (!entity.HasComponent<A>() || !entity.HasComponent<B>() || !entity.HasComponent<C>() || !entity.HasComponent<D>() || !entity.HasComponent<E>() || !entity.HasComponent<F>())
                {
                    returnDict.Remove(entity);
                }
            }

            return returnDict;
        }

        public static Dictionary<Entity, AComponent> Filter<A, B, C, D, E, F, G>(this Dictionary<Entity, AComponent> components) where A : AComponent where B : AComponent where C : AComponent where D : AComponent where E : AComponent where F : AComponent where G : AComponent
        {
            Dictionary<Entity, AComponent> returnDict = new Dictionary<Entity, AComponent>(components);
            foreach (Entity entity in returnDict.Keys)
            {
                if (!entity.HasComponent<A>() || !entity.HasComponent<B>() || !entity.HasComponent<C>() || !entity.HasComponent<D>() || !entity.HasComponent<E>() || !entity.HasComponent<F>() || !entity.HasComponent<G>())
                {
                    returnDict.Remove(entity);
                }
            }

            return returnDict;
        }

        public static Dictionary<int, Entity> Filter<A>(this Dictionary<int, Entity> entities) where A : AComponent
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

        public static Dictionary<int, Entity> Filter<A, B>(this Dictionary<int, Entity> entities) where A : AComponent where B : AComponent
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

        public static Dictionary<int, Entity> Filter<A, B, C>(this Dictionary<int, Entity> entities) where A : AComponent where B : AComponent where C : AComponent
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

        public static Dictionary<int, Entity> Filter<A, B, C, D>(this Dictionary<int, Entity> entities) where A : AComponent where B : AComponent where C : AComponent where D : AComponent
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

        public static Dictionary<int, Entity> Filter<A, B, C, D, E>(this Dictionary<int, Entity> entities) where A : AComponent where B : AComponent where C : AComponent where D : AComponent where E : AComponent
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

        public static Dictionary<int, Entity> Filter<A, B, C, D, E, F>(this Dictionary<int, Entity> entities) where A : AComponent where B : AComponent where C : AComponent where D : AComponent where E : AComponent where F : AComponent
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

        public static Dictionary<int, Entity> Filter<A, B, C, D, E, F, G>(this Dictionary<int, Entity> entities) where A : AComponent where B : AComponent where C : AComponent where D : AComponent where E : AComponent where F : AComponent where G : AComponent
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
