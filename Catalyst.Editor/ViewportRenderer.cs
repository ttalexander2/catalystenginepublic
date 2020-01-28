using Catalyst.Engine;
using Catalyst.XNA;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace CatalystEditor
{
    public static class ViewportRenderer
    {
        public static float MaxZoom = 10;
        public static float MinZoom = 0.1f;
        public static float Zoom = 1;

        public static bool Playing = false;
        public static bool Grid = true;
        public static int GridSize = 32;

        public static void RenderViewPort(GameTime gameTime)
        {




        }

    }
}
