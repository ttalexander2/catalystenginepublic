using Microsoft.Xna.Framework;

namespace Chroma.Engine.Utilities
{
    internal interface IState
    {
        void Start();
        void BeforeUpdate(GameTime gameTime);
        void Update(GameTime gameTime);
        void AfterUpdate(GameTime gameTime);
        void End();
    }
}