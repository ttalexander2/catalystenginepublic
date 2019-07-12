using System;
using System.Collections.Generic;
namespace Chroma
{
    public class ComponentList
    {

        public List<Component> Components;
        public int position = 0;

        public ComponentList()
        {
            Components = new List<Component>();
        }

        public bool MoveNext()
        {
            position++;
            return (position < Components.Count);
        }

        public void Reset()
        {
            position = 0;
        }

        public Component Current{ get {return Components[position];}}

        public void Add(Component component)
        {
            Components.Add(component);
        }

    }
}
