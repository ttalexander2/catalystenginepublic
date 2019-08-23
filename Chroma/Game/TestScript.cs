﻿using Chroma.Engine.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chroma.Game
{
    public class TestScript : IScript
    {
        public object Execute(object[] args)
        {
            String toPrint = (string)args[0];
            Console.WriteLine(toPrint);
            return null;
        }
    }
}