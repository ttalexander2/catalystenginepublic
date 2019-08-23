using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chroma.Engine.Input
{
    public class InputSystem
    {
        public Entity controlling { get; set; }
        public Scene scene { get; set; }

        public InputSystem(Scene scene)
        {
            this.scene = scene;
        }
        public void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                testEntity.MoveY(-5.0f, () => { });
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                testEntity.MoveX(-5.0f, () => { });
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                testEntity.MoveY(5.0f, () => { });
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                testEntity.MoveX(5.0f, () => { });
            }
        }
    }
}
