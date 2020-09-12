using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalyst.Engine.Rendering
{
    public class FrameList: List<Frame>
    {
        public int FrameNum = 0;
        public Frame CurrentFrame { 
            get
            {
                return this[FrameNum];
            } 
        }
        public float AnimationSpeed = 1.0f;

        public FrameList()
        { 
        }

        public FrameList Duplicate()
        {
            return new FrameList(this);
        }

        public FrameList(int capacity) : base(capacity)
        {
        }

        public FrameList(IEnumerable<Frame> collection) : base(collection)
        {
        }

        public FrameList(FrameList list) : base(list)
        {
            AnimationSpeed = list.AnimationSpeed;
            FrameNum = list.FrameNum;
            CurrentFrame = 0;
        }
    }
}
