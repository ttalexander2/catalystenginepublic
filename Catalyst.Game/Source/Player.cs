using Catalyst.Engine;
using Catalyst.Engine.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq.Expressions;

namespace Catalyst.Game.Source
{
    [Serializable]
    public class Player : Actor
    {
        private float _scale = 100;

        [GuiInteger]
        public int HorizontalSpeed = 1000;
        [GuiInteger]
        public int VerticalSpeed = 1000;

        [GuiFloat]
        public float Gravity = 5000;

        [NonSerialized]
        private KeyboardState _keyState;
        [NonSerialized]
        private Coroutine _coroutine;


        public Player(Scene scene) : base(scene)
        {
            Collider = new BoxCollider2D(this);
            Collider.Bounds = new Engine.Utilities.Rectangle(0, 0, 50, 50);
        }

        public override void Initialize()
        {
            //_coroutine = new Coroutine(this, this.MoveBackAndForth(HorizontalSpeed), true);
        }

        public override void Load()
        {
            
        }

        public void Update(GameTime gameTime)
        {
            _keyState = Keyboard.GetState();

            MoveY(Gravity*Time.DeltaTimeF * _scale);

            if (_keyState.IsKeyDown(Keys.Up))
            {
                MoveY(-VerticalSpeed * Time.DeltaTimeF * _scale);
            }

            if (_keyState.IsKeyDown(Keys.Down))
            {
                MoveY(VerticalSpeed * Time.DeltaTimeF * _scale);
            }

            if (_keyState.IsKeyDown(Keys.Left))
            {
                MoveX(-HorizontalSpeed * Time.DeltaTimeF * _scale);
            }

            if (_keyState.IsKeyDown(Keys.Right))
            {
                MoveX(HorizontalSpeed * Time.DeltaTimeF * _scale);
            }


        }


    }
}
