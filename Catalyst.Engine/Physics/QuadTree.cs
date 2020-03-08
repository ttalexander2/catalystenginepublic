using Catalyst.Engine.Rendering;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Catalyst.Engine.Physics
{
    [Serializable]
    public class QuadTree
    {
        private int _maxObjects = 10;
        private int _maxLevels = 5;

        private int _level;
        private List<Position> _objects;
        private Utilities.Rectangle _bounds;
        private QuadTree[] _nodes;


        private QuadTree(int parentLevel, Utilities.Rectangle parentBounds)
        {
            throw new Exception("QUAD TREE IS BROKEN");
            _level = parentLevel;
            _objects = new List<Position>();
            _bounds = parentBounds;
            _nodes = new QuadTree[4];
        }

        public QuadTree(Utilities.Rectangle parentBounds)
        {
            _level = 0;
            _objects = new List<Position>();
            _bounds = parentBounds;
            _nodes = new QuadTree[4];
        }

        public void Clear()
        {
            _objects.Clear();

            for (int i = 0; i < _nodes.Length; i++)
            {
                if (_nodes[i] != null)
                {
                    _nodes[i].Clear();
                    _nodes[i] = null;
                }
            }
        }

        private void Split()
        {

            int subWidth = (int)_bounds.Width / 2;
            int subHeight = (int)_bounds.Height / 2;
            int x = _bounds.X;
            int y = _bounds.Y;

            _nodes[0] = new QuadTree(_level + 1, new Utilities.Rectangle(x + subWidth, y, subWidth, subHeight));
            _nodes[1] = new QuadTree(_level + 1, new Utilities.Rectangle(x, y, subWidth, subHeight));
            _nodes[2] = new QuadTree(_level + 1, new Utilities.Rectangle(x, y + subHeight, subWidth, subHeight));
            _nodes[3] = new QuadTree(_level + 1, new Utilities.Rectangle(x + subWidth, y + subHeight, subWidth, subHeight));
        }

        private int GetIndex(Position collider)
        {
            int index = -1;
            double verticalMidpoint = _bounds.X + (_bounds.Width / 2);
            double horizontalMidpoint = _bounds.Y + (_bounds.Height / 2);
            /**
            bool topQuadrant = (collider.Position.Y + collider.Origin.Y < horizontalMidpoint && collider.Position.Y + collider.Origin.Y + collider.CollisionDims.Y < horizontalMidpoint);
            bool bottomQuadrant = (collider.Position.Y + collider.Origin.Y > horizontalMidpoint);

            if (collider.Position.X + collider.Origin.X < verticalMidpoint && collider.Position.X + collider.Origin.X + collider.CollisionDims.X < verticalMidpoint)
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
            else if (collider.Position.X + collider.Origin.X > verticalMidpoint)
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

            */
            return index;
        }

        public void Insert(Position collider)
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

        public List<Position> Retrieve(List<Position> returnObjects, Position pRect)
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
