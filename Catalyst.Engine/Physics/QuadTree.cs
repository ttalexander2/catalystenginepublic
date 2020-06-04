using Catalyst.Engine.Rendering;
using Catalyst.Engine.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalyst.Engine.Physics
{
    [Serializable]
    public class QuadTree
    {
        private int _maxObjects = 10;
        private int _maxLevels = 5;

        private int _level;
        private List<Collider2D> _objects;
        private Utilities.Rectangle _bounds;
        private QuadTree[] _nodes;


        private QuadTree(int parentLevel, Utilities.Rectangle parentBounds)
        {
            _level = parentLevel;
            _objects = new List<Collider2D>();
            _bounds = parentBounds;
            _nodes = new QuadTree[4];
        }

        public QuadTree(Utilities.Rectangle parentBounds)
        {
            _level = 0;
            _objects = new List<Collider2D>();
            _bounds = parentBounds;
            _nodes = new QuadTree[4];
        }

        public void Clear()
        {

            for (int i = 0; i < _nodes.Length; i++)
            {
                if (_nodes[i] != null)
                {
                    _nodes[i].Clear();
                    _nodes[i] = null;
                }
            }
            _objects.Clear();
        }

        private void Split()
        {

            int subWidth = (int)_bounds.Width / 2;
            int subHeight = (int)_bounds.Height / 2;
            int x = _bounds.X;
            int y = _bounds.Y;

            _nodes[0] = new QuadTree(_level + 1, new Rectangle(x + subWidth, y, subWidth, subHeight));
            _nodes[1] = new QuadTree(_level + 1, new Rectangle(x, y, subWidth, subHeight));
            _nodes[2] = new QuadTree(_level + 1, new Rectangle(x, y + subHeight, subWidth, subHeight));
            _nodes[3] = new QuadTree(_level + 1, new Rectangle(x + subWidth, y + subHeight, subWidth, subHeight));
        }

        private int GetIndex(Collider2D collider)
        {
            int index = -1;
            double verticalMidpoint = _bounds.X + (_bounds.Width / 2);
            double horizontalMidpoint = _bounds.Y + (_bounds.Height / 2);

            // Object can completely fit within the top quadrants
            bool topQuadrant = (pRect.getY() < horizontalMidpoint && pRect.getY() + pRect.getHeight() < horizontalMidpoint);
            // Object can completely fit within the bottom quadrants
            bool bottomQuadrant = (pRect.getY() > horizontalMidpoint);

            // Object can completely fit within the left quadrants
            if (pRect.getX() < verticalMidpoint && pRect.getX() + pRect.getWidth() < verticalMidpoint)
            {
                if (topQuadrant)
                {
                    index = 1;
                }
                else if (bottomQuadrant)
                {
                    index = 2;
                }
            }
            // Object can completely fit within the right quadrants
            else if (pRect.getX() > verticalMidpoint)
            {
                if (topQuadrant)
                {
                    index = 0;
                }
                else if (bottomQuadrant)
                {
                    index = 3;
                }
            }

            return index;
        }

        public void Insert(Collider2D collider)
        {

            if (_nodes[0] != null)
            {
                int index = GetIndex(collider);

                if (index != -1)
                {
                    _nodes[index].Insert(collider);
                    return;
                }
            }

            _objects.Add(collider);

            if(_objects.Count > _maxObjects && _level < _maxLevels)
            {
                if (_nodes[0] == null)
                {
                    Split();
                }

                int i = 0;
                while(i < _objects.Count)
                {
                    int index = GetIndex(_objects[i]);
                    if (index != -1)
                    {
                        _nodes[index].Insert(_objects[i]);
                        _objects.RemoveAt(i);
                    }
                    else
                    {
                        i++;
                    }
                }
            }
        }

        public List<Collider2D> Retrieve(List<Collider2D> returnObjects, Collider2D pRect)
        {
            int index = GetIndex(pRect);
            if (index != -1 && _nodes[0] != null)
            {
                _nodes[index].Retrieve(returnObjects, pRect);
            }

            returnObjects.AddRange(_objects);

            return returnObjects;
        }


    }
}
