using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Catalyst.Engine.Utilities;
using Vector2 = Catalyst.Engine.Utilities.Vector2;
using Vector3 = Catalyst.Engine.Utilities.Vector3;
using Matrix = Catalyst.Engine.Utilities.Matrix;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Catalyst.Engine.Rendering
{
    [Serializable]
    public class Camera:GameObject
    {
        internal static int CameraID { get; private set; }
        public Scene Scene { get; set; }
        [GuiEntitySelector]
        public Entity Following { get; set; }
        [GuiFloat(0,float.MaxValue)]
        public float Speed { get; set; }
        
        public Matrix Transform { get; set; }
        
        private Vector2 _position;
        [GuiVector2]
        public Vector2 Position 
        { 
            get { return _position; }
            set
            {
                _position = Vector2.Clamp(value, Vector2.Zero, Bounds-Size);
            }
        }

        public Vector2 Bounds { get; set; }
        
        private float _zoom;
        [GuiFloat(0.01f, float.MaxValue)]
        public float Zoom
        {
            get { return _zoom; }
            set { _zoom = value; if (Zoom < 0.1f) _zoom = 0.1f; }
        }
        
        private float _rotation;
        [GuiFloat(0,360)]
        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        [GuiVector2]
        public Vector2 Offset { get; set; }
        [GuiVector2]
        public Vector2 Size { get; private set; }

        public Camera(Scene scene, Vector2 bounds)
        {
            Size = new Vector2(Graphics.Width, Graphics.Height);
            Bounds = bounds;
            Scene = scene;
            Transform = new Matrix();
            Position = new Vector2();
            _zoom = 1;
            _rotation = 0;
            Speed = 0.07f;
            Name = string.Format("Camera_{0}", CameraID++);
        }

        private Camera() { }

        public void Move(Vector2 amount)
        {
            Position = amount;
        }

        public void MoveTowards(Vector2 amount)
        {
            Position += amount;
        }

        public Matrix GetTransformation(GraphicsDevice graphicsDevice)
        {
            Transform =       
              Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
                                         Matrix.CreateRotationZ(Rotation) *
                                         Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                                         Matrix.CreateTranslation(new Vector3(0, 0, 0));
            return Transform;
        }

        public Matrix GetScaledTransformation(GraphicsDevice graphicsDevice, float scale)
        {
            Transform =
              Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
                                         Matrix.CreateRotationZ(Rotation) *
                                         Matrix.CreateScale(new Vector3(Zoom * scale, Zoom * scale, 1)) *
                                         Matrix.CreateTranslation(new Vector3(0, 0, 0));
            return Transform;
        }
    }
}
