using Microsoft.Xna.Framework;

namespace Chroma.Engine
{
    public class StateMachine
    {
        private IState _currentState;


        private void ChangeState(IState newState)
        {
            if (_currentState != null) _currentState.End();

            _currentState = newState;
            _currentState.Start();
        }

        public void Start()
        {
            if (_currentState != null) _currentState.Start();
        }

        public void BeforeUpdate(GameTime gameTime)
        {
            if (_currentState != null) _currentState.BeforeUpdate(gameTime);
        }

        public void Update(GameTime gameTime)
        {
            if (_currentState != null) _currentState.Update(gameTime);
        }

        public void AfterUpdate(GameTime gameTime)
        {
            if (_currentState != null) _currentState.AfterUpdate(gameTime);
        }

        public void End()
        {
            if (_currentState != null) _currentState.End();
        }
    }
}