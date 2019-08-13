using Microsoft.Xna.Framework;

namespace Chroma.Engine.Utilities
{
    public class StateMachine
    {
        private IState _currentState;


        public void ChangeState(IState newState)
        {
            _currentState?.End();

            _currentState = newState;
            _currentState?.Start();
        }

        public void Start()
        {
            _currentState?.Start();
        }

        public void BeforeUpdate(GameTime gameTime)
        {
            _currentState?.BeforeUpdate(gameTime);
        }

        public void Update(GameTime gameTime)
        {
            _currentState?.Update(gameTime);
        }

        public void AfterUpdate(GameTime gameTime)
        {
            _currentState?.AfterUpdate(gameTime);
        }

        public void End()
        {
            _currentState?.End();
        }
    }
}