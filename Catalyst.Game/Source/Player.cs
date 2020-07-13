using Catalyst.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace Catalyst.Game.Source
{
    [Serializable]
    public class Player : Actor, IUpdate
    {
        [GuiInteger]
        public int HorizontalSpeed = 10;
        [GuiInteger]
        public int VerticalSpeed = 10;

        [GuiFloat]
        public float Gravity = 5;

        [NonSerialized]
        private KeyboardState _keyState;
        [NonSerialized]
        private Coroutine _coroutine;


        public Player(Scene scene) : base(scene)
        {
            Collider = new BoxCollider2D(this);
            Collider.Bounds = new Rectangle(0, 0, 50, 50);
        }

        public override void Initialize()
        {
            //_coroutine = new Coroutine(this, this.MoveBackAndForth(HorizontalSpeed), true);
        }

        public void Update(GameTime gameTime)
        {
            _keyState = Keyboard.GetState();

            MoveY(Gravity);

            if (_keyState.IsKeyDown(Keys.Up))
            {
                MoveY(-VerticalSpeed);
            }

            if (_keyState.IsKeyDown(Keys.Down))
            {
                MoveY(VerticalSpeed);
            }

            if (_keyState.IsKeyDown(Keys.Left))
            {
                MoveX(-HorizontalSpeed);
            }

            if (_keyState.IsKeyDown(Keys.Right))
            {
                MoveX(HorizontalSpeed);
            }


        }


    }
}
