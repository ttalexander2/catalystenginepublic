﻿using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Chroma
{
    public static class Global
    {
        public static ContentManager Content;
        public static SpriteBatch SpriteBatch;
        public static GraphicsDeviceManager Graphics;
        public static float Scale = 1;
        public static int WindowScale = 2;
        public static float UIScale = 1;
    }
}
