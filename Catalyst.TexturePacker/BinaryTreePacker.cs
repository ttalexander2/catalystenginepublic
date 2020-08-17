using Catalyst.Engine.Utilities;
using System;
using System.Drawing;

namespace Catalyst.ContentManager
{
    public class PackedNode
    {
        public PackedNode Left { get; private set; }
        public PackedNode Right { get; private set; }
        public System.Drawing.Rectangle Rect { get; private set; }
        public Bitmap Image { get; private set; }

        public int Padding { get; private set; }
        public bool Rotate { get; private set; }

        public PackedNode(System.Drawing.Rectangle rect, int padding, bool rotate)
        {
            Rect = rect;
            Padding = padding;
            Rotate = rotate;
        }

        internal PackedNode Insert(Bitmap img)
        {

            if (Image == null && (img.Width + Padding <= Rect.Width && img.Height + Padding <= Rect.Height))
            {
                Image = img;

                if (Rect.Width - img.Width > 0)
                    Right = new PackedNode(new System.Drawing.Rectangle(Rect.X + img.Width + Padding, Rect.Y, Rect.Width - img.Width - Padding, img.Height), Padding, Rotate);
                if (Rect.X <= Padding)
                {
                    Left = new PackedNode(new System.Drawing.Rectangle(Padding, Rect.Y + img.Height + Padding, Rect.Width, Rect.Height), Padding, Rotate);
                }
                Rect = new System.Drawing.Rectangle(Rect.X, Rect.Y, img.Width, img.Height);
                Rotate = false;

                if (TexturePacker.Verbose)
                    Log.WriteLine(String.Format("Inserted texure [{0}] at location: [{1}], with rotation [{2}]", img.Tag, Rect, Rotate));


                return this;
            }

            if (Image == null && Rotate)
            {
                img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                Rotate = false;
                PackedNode ret = Insert(img);
                if (ret == null)
                    img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                return ret;
            }

            if (Right != null)
            {
                PackedNode ret = Right.Insert(img);
                if (ret == null)
                {
                    if (Left != null)
                        return Left.Insert(img);
                }
                return ret;
            }

            if (Left != null)
            {
                return Left.Insert(img);
            }

            return null;

        }



    }

    public class BinaryTreePacker
    {
        internal PackedNode Root;
        private int _maxX;
        private int _maxY;
        private int _padding;
        private bool _rotate;
        public BinaryTreePacker(int max_X, int max_Y, int padding, bool rotate)
        {
            _maxX = max_X;
            _maxY = max_Y;
            _padding = padding;
            _rotate = rotate;
            Root = null;
        }

        public PackedNode Insert(Bitmap img)
        {
            if (Root == null)
            {
                Root = new PackedNode(new System.Drawing.Rectangle(_padding, _padding, _maxX, _maxY), _padding, _rotate);
                return Root;
            }

            return Root.Insert(img);
        }
        
    }
}
