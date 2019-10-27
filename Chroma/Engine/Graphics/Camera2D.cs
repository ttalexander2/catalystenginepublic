using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chroma.Engine.Graphics
{
    public class Camera2D
    {
        public Scene Scene { get; private set; }
        public Entity Following { get; set; }
        public float Speed { get; set; }
        public Matrix Transform { get; set; }
        private Vector2 _position;
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
        public float Zoom
        {
            get { return _zoom; }
            set { _zoom = value; if (Zoom < 0.1f) _zoom = 0.1f; }
        }

        private float _rotation;
        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }
        public Vector2 Size { get; private set; }

        public Camera2D(Scene scene, Vector2 bounds)
        {
            Size = new Vector2(Global.Width, Global.Height);
            Bounds = bounds;
            Scene = scene;
            Transform = new Matrix();
            Position = new Vector2();
            _zoom = 1;
            _rotation = 0;
            Speed = 0.07f;
        }

        public void Move(Vector2 amount)
        {
            Position = amount;
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
    }
}
